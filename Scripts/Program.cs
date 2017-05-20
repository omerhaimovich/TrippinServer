using Common;
using Common.Enums;
using Common.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts
{
    class Program
    {
        static void Main(string[] args)
        {
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
            }


            

            //GMapsUtilities.GetPhotoURLOfAttraction("CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU");
            //var attractions = GMapsUtilities.GetAttractionsAroundPoint(32.075842, 34.889338, new List<AttractionType>()
            //{
            //    AttractionType.BarsPubs
            //});

        //}
    }
}
