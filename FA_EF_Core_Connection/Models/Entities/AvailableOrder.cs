using System;
using System.Collections.Generic;

#nullable disable

namespace FakeAtlas.Models.Entities
{
    public partial class AvailableOrder
    {
        public AvailableOrder()
        {
            UserOrders = new HashSet<UserOrder>();
        }

        public int Id { get; set; }
        public string PathFrom { get; set; }
        public string PathTo { get; set; }
        public DateTime? DepartureTime { get; set; }
        public int? Cost { get; set; }
        public int? ShipperId { get; set; }

        public virtual Shipper Shipper { get; set; }
        public virtual ICollection<UserOrder> UserOrders { get; set; }
    }
}