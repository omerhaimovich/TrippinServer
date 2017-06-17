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
    public class AttractionComparer : IEqualityComparer<Attraction>
    {
        public bool Equals(Attraction x, Attraction y)
        {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode(Attraction obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class AttractionsBL
    {
        public static List<Attraction> GetNewAttractions(string p_strTripId, double p_dLat, double p_dLng)
        {
            Trip objCurrUserTrip = MongoAccess.Access<Trip>().FindSync(objTrip => objTrip.Id == new ObjectId(p_strTripId)).FirstOrDefault();
            objCurrUserTrip.GoodAttractions.RemoveAll(x => x == null);
            User objUSer = MongoAccess.Access<User>().FindSync(objCurrUSer => objCurrUSer.Email == objCurrUserTrip.UserEmail).FirstOrDefault();

            if (objCurrUserTrip != null)
            {
                var lstAttractions = GMapsUtilities.GetAttractionsAroundPoint(p_dLat, p_dLng, objCurrUserTrip.WantedAttractionsTypes, objUSer.Radius).Distinct(new AttractionComparer());
                var dicAttractionsHash = lstAttractions.ToDictionary(x => x.Id);
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
                    AttractionToCharacterHash.Add(attraction.Id, generatedcharacters[characterIndex]);
                    CharacterToAttractionHash.Add(generatedcharacters[characterIndex], attraction.Id);
                    lstItems.Add(generatedcharacters[characterIndex]);
                    characterIndex++;
                }


                foreach (var objCurrTrip in AdditionalTrips)
                {

                    string strTransaction = "";

                    foreach (var strAttractionId in objCurrTrip.GoodAttractions)
                    {
                        if (strAttractionId != null && AttractionToCharacterHash.ContainsKey(strAttractionId.Id))
                        {
                            strTransaction += AttractionToCharacterHash[strAttractionId.Id];
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
                    return lstAttractions.Where(objAttraction => objAttraction.IsOpenNow && !objCurrUserTrip.UnratedAttractions.Select(x => x.Id).Contains(objAttraction.Id) &&
                                                                                !objCurrUserTrip.GoodAttractions.Select(x => x.Id).Contains(objAttraction.Id) &&
                                                                                !objCurrUserTrip.BadAttractions.Select(x => x.Id).Contains(objAttraction.Id)).OrderByDescending(x => x.Rating).Take(10).ToList();
                }
                else
                {
                    List<Attraction> lstProposedAttractions = new List<Attraction>();

                    foreach (Rule objRule in orderedRules)
                    {
                        foreach (char proposedAttraction in objRule.Y)
                        {
                            if (!objCurrUserTrip.BadAttractions.Select(x => x.Id).Contains(CharacterToAttractionHash[proposedAttraction.ToString()]) &&
                                !objCurrUserTrip.GoodAttractions.Select(x => x.Id).Contains(CharacterToAttractionHash[proposedAttraction.ToString()]) &&
                                !objCurrUserTrip.UnratedAttractions.Select(x => x.Id).Contains(CharacterToAttractionHash[proposedAttraction.ToString()]))
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

            if (objCurrTrip != null && !objCurrTrip.UnratedAttractions.Select(x => x.Id).Contains(p_strAttractionId))
            {
                MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                    new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.UnratedAttractions, new CoreAttraction()
                    {
                        Id = p_strAttractionId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.MinValue
                    }));
            }

        }

        public static void AttractionEnd(string p_strTripId, string p_strAttractionId)
        {

            Trip objTrip = MongoAccess.Access<Trip>().FindSync(objCurrTrip => objCurrTrip.Id == new ObjectId(p_strTripId)).First();
            CoreAttraction att;
            if (( att= objTrip.UnratedAttractions.Where(x => x.Id == p_strAttractionId).FirstOrDefault()) != null)
                att.EndDate = DateTime.Now;
            objTrip = null;
            MongoAccess.Access<Trip>().FindOneAndUpdate(objCurrTrip => objCurrTrip.Id == new ObjectId(p_strTripId),
                new UpdateDefinitionBuilder<Trip>().PullFilter<CoreAttraction>(x => x.UnratedAttractions, x=>x.Id == p_strAttractionId));

            if(att != null)
            { 
            MongoAccess.Access<Trip>().FindOneAndUpdate(objCurrTrip => objCurrTrip.Id == new ObjectId(p_strTripId),
                new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.UnratedAttractions, att));
            }
        }

        public static void AttractionRated(string p_strTripId, string p_strAttractionId, bool goodAttraction)
        {
            Trip objCurrTrip = MongoAccess.Access<Trip>().FindSync(x => x.Id == new ObjectId(p_strTripId)).FirstOrDefault();
            CoreAttraction coreAtt = objCurrTrip.UnratedAttractions.Where(x => x.Id == p_strAttractionId).FirstOrDefault();


            MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
               new UpdateDefinitionBuilder<Trip>().PullFilter<CoreAttraction>(x => x.UnratedAttractions, x => x.Id == p_strAttractionId));


            if (objCurrTrip != null && coreAtt != null)
            {
                if (goodAttraction)
                {
                    if (!objCurrTrip.GoodAttractions.Select(objAtt => objAtt.Id).Contains(p_strAttractionId))
                    {
                        MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                        new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.GoodAttractions, coreAtt));
                    }
                }
                else
                {
                    if (!objCurrTrip.BadAttractions.Select(objAtt => objAtt.Id).Contains(p_strAttractionId))
                    {
                        MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(p_strTripId),
                        new UpdateDefinitionBuilder<Trip>().AddToSet(x => x.BadAttractions, coreAtt));
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
