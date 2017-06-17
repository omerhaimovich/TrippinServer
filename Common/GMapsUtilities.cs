using Common.Enums;
using Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GMapsUtilities
    {
        public static Dictionary<AttractionType, string> TypesToSearchString = new Dictionary<AttractionType, string>()
        {
            { AttractionType.BarsPubs, "bars,pubs"},
            { AttractionType.Children, "children"},
            { AttractionType.History, "history"},
            { AttractionType.Market, "market"},
            { AttractionType.Museum, "museum"},
            { AttractionType.Nature, "nature"},
            { AttractionType.Restaurant, "restaurant"},
            { AttractionType.ShoppingMall, "shopping,mall"},
            { AttractionType.Theater, "theater"},
            { AttractionType.Landmark, "landmark"},
        };

        /// <summary>
        /// Getting attractions from a certain type around point
        /// </summary>
        /// <param name="pLng"></param>
        /// <param name="pLat"></param>
        /// <param name="pRadius"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        /// Example: https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=41.903639,12.483153&radius=10000&keyword=restaurant&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60
        public static List<Attraction> GetAttractionsAroundPoint(double pLat, double pLng, List<AttractionType> attractionTypes, double pRadius = 20000)
        {

            if (attractionTypes.Count == 0)
                attractionTypes.Add(AttractionType.Landmark);

            ConcurrentBag<Attraction> lstAttractions = new ConcurrentBag<Attraction>();

            int MaximumAttractions = 30;
            int AttractionPerType = (int)Math.Floor((decimal)MaximumAttractions / (decimal)attractionTypes.Count);
            Parallel.ForEach(attractionTypes, attType =>
            {
                List<Attraction> lstAttractionsFromThisType = new List<Attraction>();

                string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=";
                url += pLat;
                url += ",";
                url += pLng;
                url += "&radius=";
                url += pRadius * 1000;
                url += "&keyword=";
                url += TypesToSearchString[attType];
                url += "%20";
                url += "&key=";
                url += ConfigurationSettings.AppSettings["GoogleKey"];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // New code:
                    Task<HttpResponseMessage> response = client.GetAsync(client.BaseAddress);

                    if (response.Result.IsSuccessStatusCode)
                    {

                        var g = response.Result.Content.ReadAsStringAsync().Result;
                        var lstResults = JObject.Parse(g).GetValue("results");

                        foreach (var attraction in lstResults)
                        {

                            double lat = attraction["geometry"]["location"]["lat"].ToObject<double>();
                            double lng = attraction["geometry"]["location"]["lng"].ToObject<double>();
                            double rating = 2.5;
                            if (attraction["rating"] != null)
                                rating = attraction["rating"].ToObject<double>();
                            string name = attraction["name"].ToString();
                            bool open = true;
                            if (attraction["opening_hours"] != null)
                                open = attraction["opening_hours"]["open_now"].ToObject<bool>();
                            string id = attraction["place_id"].ToString();
                            string photoReference = null;
                            if (attraction["photos"] != null && attraction["photos"].Count() > 0)
                            {
                                photoReference = attraction["photos"][0]["photo_reference"].ToString();
                            }


                            Attraction objAttraction = new Attraction()
                            {
                                Id = id,
                                IsOpenNow = open,
                                Latitude = lat,
                                Longitude = lng,
                                Name = name,
                                Rating = (float)rating,
                                PhotoUrl = GetPhotoURLOfAttraction(photoReference)
                            };

                            lstAttractionsFromThisType.Add(objAttraction);

                        }

                    }
                }

                foreach (var attraction in lstAttractionsFromThisType.OrderByDescending(x=>x.Rating).Take(AttractionPerType))
                {
                    lstAttractions.Add(attraction);
                }
            });

            
            return lstAttractions.ToList().OrderByDescending(x=>x.Rating).ToList();
        }

        // TODO: Omer
        // Example:
        // https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=<PHOTO_REFERENCE>&key=<API_KEY>
        // https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60
        public static string GetPhotoURLOfAttraction(string PhotoReference)
        {
            string url = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=";
            url += PhotoReference;
            url += "&key=";
            url+= ConfigurationSettings.AppSettings["GoogleKey"];

            return url;
        }

        // Example: http://maps.googleapis.com/maps/api/geocode/json?latlng=50.602472,9.987603
        public static string GetCountryOfPoint(double p_lat, double p_lng)
        {
            string url = "http://maps.googleapis.com/maps/api/geocode/json?latlng=";
            url += p_lat;
            url += ",";
            url += p_lng;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                Task<HttpResponseMessage> response = client.GetAsync(client.BaseAddress);

                if (response.Result.IsSuccessStatusCode)
                {

                    var g = response.Result.Content.ReadAsStringAsync().Result;
                    var lstResults = JObject.Parse(g).GetValue("results");

                    if(lstResults.Count() == 0 )
                    {
                        return "Unknown";
                    }
                    else
                    {
                        foreach (var objAddressComponent in lstResults[0]["address_components"])
                        {
                            foreach (var strAdressType in objAddressComponent["types"])
                            {
                                if(strAdressType.ToString() == "country")
                                {
                                    return objAddressComponent["long_name"].ToString();
                                }
                            }                            
                        } 
                    }

                    return "Unknown";
                }
            }

            // Search for country in address_components
            return "Unknown";
        }

        // TODO: Limay
        // Example: https://maps.googleapis.com/maps/api/place/details/json?placeid=ChIJN1t_tDeuEmsRUsoyG83frY4&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60
        public static Attraction GetAttractionByCore(CoreAttraction Attraction)
        {
            string url = "https://maps.googleapis.com/maps/api/place/details/json?placeid=";
            url += Attraction.Id;          
            url += "&key=";
            url += ConfigurationSettings.AppSettings["GoogleKey"];

            Attraction objAttraction = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                Task<HttpResponseMessage> response = client.GetAsync(client.BaseAddress);

                if (response.Result.IsSuccessStatusCode)
                {

                    var g = response.Result.Content.ReadAsStringAsync().Result;
                    var attraction = JObject.Parse(g).GetValue("result");

                    if (attraction != null)
                    {
                        double lat = attraction["geometry"]["location"]["lat"].ToObject<double>();
                        double lng = attraction["geometry"]["location"]["lng"].ToObject<double>();
                        double rating = 2.5;
                        if (attraction["rating"] != null)
                            rating = attraction["rating"].ToObject<double>();
                        string name = attraction["name"].ToString();
                        bool open = true;
                        if (attraction["opening_hours"] != null)
                            open = attraction["opening_hours"]["open_now"].ToObject<bool>();
                        string id = attraction["place_id"].ToString();
                        string photoReference = null;
                        if (attraction["photos"] != null && attraction["photos"].Count() > 0)
                        {
                            photoReference = attraction["photos"][0]["photo_reference"].ToString();
                        }


                        objAttraction = new Attraction()
                        {
                            Id = id,
                            IsOpenNow = open,
                            Latitude = lat,
                            Longitude = lng,
                            Name = name,
                            Rating = (float)rating,
                            PhotoUrl = GetPhotoURLOfAttraction(photoReference),
                            EndDate = Attraction.EndDate,
                            StartDate = Attraction.StartDate                            
                        };
                    }
                }
            }

            return objAttraction;
        }

        // Get attractions In country - No Need For Now.
        //https://maps.googleapis.com/maps/api/place/textsearch/json?query=restaurants+in+rome&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60

    }
}
