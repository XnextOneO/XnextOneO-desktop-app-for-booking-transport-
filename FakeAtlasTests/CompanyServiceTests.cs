using FakeAtlas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FakeAtlasTests
{
    public class CompanyServiceTests
    {

        [Fact]
        public void ValidateVehicleAmount_WithNegativeAmount_ReturnsFalse()
        {
            INumberValidator service = new CompanyService();
            Assert.False(service.ValidateVehiclesAmount(-3));
        }

        [Fact]
        public void ValidateVehicleAmount_WithPositiveAmount_ReturnsTrue()
        {
            INumberValidator service = new CompanyService();
            Assert.True(service.ValidateVehiclesAmount(4));
        }

        [Fact]
        public void ValidateVehicleAmount_WithZeroAmount_ReturnsFalse()
        {
            INumberValidator service = new CompanyService();
            Assert.False(service.ValidateVehiclesAmount(0));
        }


        [Fact]
        public void ValidateRouteCost_WithNegativeAmount_ReturnsFalse()
        {
            INumberValidator service = new CompanyService();
            Assert.False(service.ValidateRouteCost(-3));
        }

        [Fact]
        public void ValidateRouteCost_WithPositiveAmount_ReturnsTrue()
        {
            INumberValidator service = new CompanyService();
            Assert.True(service.ValidateRouteCost(4));
        }

        [Fact]
        public void ValidateRouteCost_WithZeroAmount_ReturnsFalse()
        {
            INumberValidator service = new CompanyService();
            Assert.False(service.ValidateRouteCost(0));
        }
    }
}
