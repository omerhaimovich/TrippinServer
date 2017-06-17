using Algorithm.Attractions;
using Algorithm.Trips;
using Common;
using Common.Enums;
using Common.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrippinServer.Controllers;

namespace Scripts
{
    class Program
    {
        static void Main(string[] args)
        {

            //TripsBL.GetTrip("592338eb4fca0e3434b8651f","omer.haimovich@gmail.com", 32.075842, 34.889338);
    
            //AttractionsBL.GetNewAttractions("593863f340e70634a0e4fe3e", 55.738778, 37.623157);
            //AttractionsBL.AttractionEnd("5923362b4fca0e302c2e8239", "ChIJd6ZQrYBLHRURHries2H7EfM");

            MongoAccess.Access<Trip>().DeleteMany(new FilterDefinitionBuilder<Trip>().Where(x=>x.UserEmail== "bareliah@gmail.com"));
            MongoAccess.Access<User>().UpdateMany(new FilterDefinitionBuilder<User>().Empty, new UpdateDefinitionBuilder<User>().Set(y=>y.Radius, 20));

            //User bar = new User()
            //{
            //    Email = "bareliah@gmail.com",
            //    NotificationsOn = true,
            //    Radius = 20
            //};

            //MongoAccess.Access<User>().InsertOne(bar);

            //Attraction pp = GMapsUtilities.GetAttractionById("ChIJseyMKsdLHRURwNM13e99zSc");

            Trip objTripTwo = new Trip()
            {
                Year = 2017,
                Country = "Israel",
                UserEmail = "check@gmail.com",
                CreationDate = new DateTime(),
                IsActive = true,
                WantedAttractionsTypes = Enum.GetValues(typeof(AttractionType)).Cast<AttractionType>().ToList(),
                GoodAttractions = new List<CoreAttraction>()
                 {
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJseyMKsdLHRURwNM13e99zSc"
                     }
                 },
                BadAttractions = new List<CoreAttraction>(),
                UnratedAttractions = new List<CoreAttraction>()
                {
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJ15hlI51MHRURCLZn_nCoAu0"
                     }
                }
            };
            MongoAccess.Access<Trip>().InsertOne(objTripTwo);

            var attractions = GMapsUtilities.GetAttractionsAroundPoint(32.075842, 34.889338, new List<AttractionType>()
            {
                AttractionType.BarsPubs
            });

            String a = "";

            foreach (var item in attractions)
            {
                a += item.Id;
                a += " ";
            }

            for (int i = 0; i < 50; i++)
            {
                Trip objTrip = new Trip()
                {
                    Year = 2017,
                    Country = "Israel",
                    UserEmail = "fake" +i+"@gmail.com",
                    CreationDate = new DateTime(),
                    IsActive = true,
                    WantedAttractionsTypes = Enum.GetValues(typeof(AttractionType)).Cast<AttractionType>().ToList(),
                    GoodAttractions = new List<CoreAttraction>()
                 {
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJseyMKsdLHRURwNM13e99zSc"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJ15hlI51MHRURCLZn_nCoAu0"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJyTsyPXxMHRURPZDSzopJdcw"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJd6ZQrYBLHRURHries2H7EfM"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJfYmZgxJIHRURXTmR1d1k4jM"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJazJD6fJIHRURi2Ov5OOfGls"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJ00CsYv9JHRURgOxEk-9yxqU"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJ_2arJzi0AhURu_q7iSb1_zI"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJh3qlRrlMHRURaCYlxGaOM90"
                     },
                     new CoreAttraction()
                     {
                         StartDate = DateTime.Now - TimeSpan.FromDays(10),
                         EndDate = DateTime.Now - TimeSpan.FromDays(7),
                         Id = "ChIJnf4xRGBJHRURIyrZXpt8w6c"
                     }
                 },
                    BadAttractions = new List<CoreAttraction>(),
                    UnratedAttractions = new List<CoreAttraction>()
                };

               
            }

            

           

          
            MongoAccess.Access<Trip>().InsertOne(objTripTwo);
            //    List<string> FakeEmails = new List<string>()
            //    {
            //        "omerh98@gmail.com",
            //        "ferby2@walla.com",
            //        "rayman12@gmail.com",
            //        "rayman1234@gmail.com",
            //    };

            //    foreach (var item in FakeEmails)
            //    {
            //        User objUser = new User()
            //        {
            //            Email = item,
            //            NotificationsOn = true
            //        };

            //        Trip objTrip = new Trip()
            //        {
            //            BadAttractionsIds = new List<string>(),
            //            GoodAttractionsIds = new List<string>()
            //            {
            //                "ChIJseyMKsdLHRURwNM13e99zSc",
            //                "ChIJ15hlI51MHRURCLZn_nCoAu0",
            //                "ChIJd6ZQrYBLHRURHries2H7EfM"
            //            },
            //            CreationDate = DateTime.Now,
            //            UserEmail = item,
            //            Year = 2017,
            //            Country = "Israel",
            //            UnratedAttractionsIds = new List<string>(),
            //            WantedAttractionsTypes = Enum.GetValues(typeof(AttractionType)).Cast<AttractionType>().ToList(),
            //        };



            //        MongoAccess.Access<Trip>().InsertOne(objTrip);
            //MongoAccess.Access<User>().InsertOne(objUser);
            //MongoAccess.Access<Trip>().DeleteMany(new FilterDefinitionBuilder<Trip>().Empty)}




            //GMapsUtilities.GetPhotoURLOfAttraction("CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU");
           

        }
    }
}
