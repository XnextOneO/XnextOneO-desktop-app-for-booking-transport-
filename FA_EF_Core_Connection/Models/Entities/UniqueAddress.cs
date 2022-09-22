using System;
using System.Collections.Generic;

#nullable disable

namespace FakeAtlas.Models.Entities
{
    public partial class UniqueAddress
    {
        public UniqueAddress()
        {
            BookingUsers = new HashSet<BookingUser>();
        }

        public int Id { get; set; }
        public string City { get; set; }
        public string Oblast { get; set; }
        public string StreetName { get; set; }
        public int? BuildingNum { get; set; }

        public virtual ICollection<BookingUser> BookingUsers { get; set; }
    }
}
