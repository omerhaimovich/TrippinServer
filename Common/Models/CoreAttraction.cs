using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CoreAttraction : IComparable, System.IEquatable<CoreAttraction>
    {
        public string Id { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int CompareTo(object obj)
        {
            if(!object.ReferenceEquals(obj, null) && obj is CoreAttraction && (obj as CoreAttraction).Id == this.Id)
            {
                return 0;
            }

            return 1;
        }

        public bool Equals(CoreAttraction other)
        {
            if (!object.ReferenceEquals(other, null) && other is CoreAttraction && (other as CoreAttraction).Id == this.Id)
            {
                return true;
            }

            return false;
        }
    }
}
