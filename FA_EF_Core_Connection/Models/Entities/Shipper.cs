using System;
using System.Collections.Generic;

#nullable disable

namespace FakeAtlas.Models.Entities
{
    public partial class Shipper
    {
        public Shipper()
        {
            AvailableOrders = new HashSet<AvailableOrder>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string VehicleType { get; set; }
        public int? VehicleNum { get; set; }

        public virtual ICollection<AvailableOrder> AvailableOrders { get; set; }
    }
}
