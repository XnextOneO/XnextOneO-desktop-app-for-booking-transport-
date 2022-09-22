using System;
using System.Collections.Generic;

#nullable disable

namespace FakeAtlas.Models.Entities
{
    public partial class BookingUser
    {
        public BookingUser()
        {
            UserOrders = new HashSet<UserOrder>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public int? IdAddress { get; set; }
        public bool IsAdmin { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string Salt { get; set; }

        public virtual UniqueAddress IdAddressNavigation { get; set; }
        public virtual ICollection<UserOrder> UserOrders { get; set; }
    }
}
