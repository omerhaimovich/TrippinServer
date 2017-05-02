using Common.Enums;
using Common.Models;
using Newtonsoft.Json.Linq;
using System;
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
        /// <summary>
        /// Getting attractions from a certain type around point
        /// </summary>
        /// <param name="pLng"></param>
        /// <param name="pLat"></param>
        /// <param name="pRadius"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        /// Example: https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=41.903639,12.483153&radius=10000&keyword=restaurant&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60
        public static List<Attraction> GetAttractionsAroundPoint(double pLng, double pLat, double pRadius = 10000, AttractionType pType = AttractionType.Attraction)
        {
            List<Attraction> lstAttractions = new List<Attraction>();

            string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=";
            url += pLng;
            url += ",";
            url += pLat;
            url += "&radius=";
            url += pRadius;
            url += "&keyword=";
            url += Enum.GetName(typeof(AttractionType), pType);
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
                        if (attraction["photos"].Count() > 0)
                        {
                            photoReference = attraction["photos"][0]["photo_reference"].ToString();
                        }
                        

                        Attraction objAttraction = new Attraction()
                        {
                            ID = id,
                            IsOpenNow = open,
                            Latitude = lat,
                            Longitude = lng,
                            Name = name,
                            Rating = (float)rating,
                            PhotoReference = photoReference
                        };

                        lstAttractions.Add(objAttraction);

                    }

                }
            }

            return lstAttractions;
        }

        // TODO: Omer
        // Example:
        // https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=<PHOTO_REFERENCE>&key=<API_KEY>
        // https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60
        public static string GetPhotoURLOfAttraction(string PhotoReference)
        {
            return null;
        }

        // Example: http://maps.googleapis.com/maps/api/geocode/json?latlng=50.602472,9.987603
        // TODO: Limay
        public static string GetCountryOfPoint(double lat, double lng)
        {
            // Search for country in address_components
            return null;
        }

        // TODO: Limay
        // Example: https://maps.googleapis.com/maps/api/place/details/json?placeid=ChIJN1t_tDeuEmsRUsoyG83frY4&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60
        public static Attraction GetAttractionById(string PlaceId)
        {
            return null;
        }

        // Get attractions In country - No Need For Now.
        //https://maps.googleapis.com/maps/api/place/textsearch/json?query=restaurants+in+rome&key=AIzaSyBoB1pwTp2TzJbiHdkfl8loYsbTGYL_w60

    }
}
