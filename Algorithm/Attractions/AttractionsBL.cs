using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using AprioriAlgorithm;
using Common.Enums;

namespace Algorithm.Attractions
{
    public class AttractionsBL
    {
        public static List<Attraction> GetNewAttractions(string p_strTripId, double p_dLat, double p_dLng)
        {
            Trip objCurrUserTrip = MongoAccess.Access<Trip>().FindSync(objTrip => objTrip.Id == new ObjectId(p_strTripId)).FirstOrDefault();
            
            if(objCurrUserTrip != null)
            {
                var lstAttractions = GMapsUtilities.GetAttractionsAroundPoint(p_dLat, p_dLng, objCurrUserTrip.WantedAttractionsTypes);
                var dicAttractionsHash = lstAttractions.ToDictionary(x => x.ID);
                var AttractionToCharacterHash = new Dictionary<string, string>();
                var CharacterToAttractionHash = new Dictionary<string, string>();

                List<String> generatedcharacters = GenerateCharacters();

                var AdditionalTrips = MongoAccess.Access<Trip>().FindSync(objTrip => objTrip.Country == objCurrUserTrip.Country && objTrip.Year == DateTime.Now.Year).ToList();

                List<String> lstItems = new List<string>();
                List<String> lstTransactions = new List<string>();

                
                String UserTransaction = "";
                var characterIndex = 0;

                foreach (var attraction in lstAttractions.Take(generatedcharacters.Count))
                {
                    AttractionToCharacterHash.Add(attraction.ID, generatedcharacters[characterIndex]);
                    CharacterToAttractionHash.Add(generatedcharacters[characterIndex], attraction.ID);
                    lstItems.Add(generatedcharacters[characterIndex]);
                    characterIndex++;
                }


                foreach (var objCurrTrip in AdditionalTrips)
                {

                    string strTransaction = "";

                    foreach (var strAttractionId in objCurrTrip.GoodAttractionsIds)
                    {
                        if (AttractionToCharacterHash.ContainsKey(strAttractionId))
                        {
                            strTransaction += AttractionToCharacterHash[strAttractionId];
                        }
                    }

                    if (objCurrTrip.UserEmail != objCurrUserTrip.UserEmail)
                    {
                        if (strTransaction != "")
                        {
                            lstTransactions.Add(strTransaction);
                        }
                    }
                    else
                    {
                        UserTransaction = strTransaction;
                    }

                }

                IApriori objApriori = new Apriori();
                Output output = objApriori.ProcessTransaction(0.3, 0.2, lstItems, lstTransactions.ToArray());
                var orderedRules = output.StrongRules.Where(objRule => UserTransaction.Contains(objRule.X)).OrderByDescending(objRule => GetDifferenceByTransactions(objRule.X, UserTransaction)).ThenByDescending(objRule => objRule.Confidence);

                if (orderedRules.Count() == 0)
                {
                    return lstAttractions.Where(objAttraction => objAttraction.IsOpenNow && !objCurrUserTrip.UnratedAttractionsIds.Contains(objAttraction.ID) &&
                                                                                !objCurrUserTrip.GoodAttractionsIds.Contains(objAttraction.ID) &&
                                                                                !objCurrUserTrip.BadAttractionsIds.Contains(objAttraction.ID)).OrderByDescending(x => x.Rating).Take(10).ToList();
                }
                else
                {
                    List<Attraction> lstProposedAttractions = new List<Attraction>();

                    foreach (Rule objRule in orderedRules)
                    {
                        foreach (char proposedAttraction in objRule.Y)
                        {
                            if (!objCurrUserTrip.BadAttractionsIds.Contains(CharacterToAttractionHash[proposedAttraction.ToString()]) &&
                                !objCurrUserTrip.GoodAttractionsIds.Contains(CharacterToAttractionHash[proposedAttraction.ToString()]) &&
                                !objCurrUserTrip.UnratedAttractionsIds.Contains(CharacterToAttractionHash[proposedAttraction.ToString()]))
                            {
                                Attraction objAttraction = dicAttractionsHash[CharacterToAttractionHash[proposedAttraction.ToString()]];
                                if (objAttraction.IsOpenNow && !lstProposedAttractions.Contains(objAttraction))
                                {
                                    lstProposedAttractions.Add(objAttraction);
                                }

                                if (lstProposedAttractions.Count >= 5)
                                {
                                    break;
                                }
                            }

                        }

                        if (lstProposedAttractions.Count >= 5)
                        {
                            break;
                        }
                    }

                    if (lstProposedAttractions.Count > 5)
                    {
                        return lstProposedAttractions;
                    }
                    else
                    {
                        lstProposedAttractions.AddRange(lstAttractions);
                        return lstProposedAttractions.Take(10).ToList();
                    }
                }
            }


            return null;
        }

        public static int GetDifferenceByTransactions(string t1, string t2)
        {
            int diff = 0;

            foreach (char itemt1 in t1)
            {
                bool IsExist = false;

                foreach (char itemt2 in t2)
                {
                    if (itemt2 == itemt1)
                    {
                        IsExist = true;
                        break;
                    }
                }

                if (IsExist)
                    continue;

                diff++;
            }

            return diff;
        }

        public static void AttractionChosen(string p_strTripId, string p_strAttractionId)
        {
            Trip objCurrTrip = MongoAccess.Access<Trip>().FindSync(x => x.Id == new ObjectId(p_strTripId)).FirstOrDefault();

            if (objCurrTrip != null && !objCurrTrip.UnratedAttractionsIds.Contains(p_strAttractionId))
            {
                MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                    new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.UnratedAttractionsIds, p_strAttractionId));
            }

        }

        public static void AttractionRated(string p_strTripId, string p_strAttractionId, bool goodAttraction)
        {
            MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                new UpdateDefinitionBuilder<Trip>().Pull(x => x.UnratedAttractionsIds, p_strAttractionId));

            Trip objCurrTrip = MongoAccess.Access<Trip>().FindSync(x => x.Id == new ObjectId(p_strTripId)).FirstOrDefault();

            if (objCurrTrip != null)
            {
                if (goodAttraction)
                {
                    if (!objCurrTrip.GoodAttractionsIds.Contains(p_strAttractionId))
                    {
                        MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                        new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.GoodAttractionsIds, p_strAttractionId));
                    }
                }
                else
                {
                    if (!objCurrTrip.BadAttractionsIds.Contains(p_strAttractionId))
                    {
                        MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                        new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.BadAttractionsIds, p_strAttractionId));
                    }
                }
            }
        }

        public static List<String> GenerateCharacters()
        {
            return new List<string>()
            {
                "A",
                "B",
                "C",
                "D",
                "E",
                "F",
                "G",
                "H",
                "I",
                "J",
                "K",
                "L",
                "M",
                "N",
                "O",
                "P",
                "Q",
                "R",
                "S",
                "T",
                "U",
                "V",
                "W",
                "X",
                "Y",
                "Z",
                "@",
                "!",
                "$",
                "#",
                "%",
            };
        }

    }
}
