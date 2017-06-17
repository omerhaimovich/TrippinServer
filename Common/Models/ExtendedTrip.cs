using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ExtendedTrip : Trip
    {
        public List<Attraction> FullGoodAttractions { get; set; }

        public List<Attraction> FullBadAttractions { get; set; }

        public List<Attraction> FullUnratedAttractions { get; set; }

        public List<Attraction> FullAllAttractions { get; set; }

        public ExtendedTrip(Trip objTrip)
        {
            this.Year = objTrip.Year;
            this.WantedAttractionsTypes = objTrip.WantedAttractionsTypes;
            this.UserEmail = objTrip.UserEmail;
            this.UnratedAttractions = objTrip.UnratedAttractions;
            this.IsActive = objTrip.IsActive;
            this.Id = objTrip.Id;
            this.GoodAttractions = objTrip.GoodAttractions;
            this.EndDate = objTrip.EndDate;
            this.CreationDate = objTrip.CreationDate;
            this.Country = objTrip.Country;
            this.BadAttractions = objTrip.BadAttractions;
            this.AllAttractions = objTrip.AllAttractions;
        }

    }
}
