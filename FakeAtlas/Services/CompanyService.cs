using FakeAtlas.FakeAtlasUIComponents;
using FakeAtlas.ViewModels;
using FakeAtlas.ViewModels.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeAtlas.Services
{
    public interface INumberValidator
    {
        bool ValidateRouteCost(int cost);

        bool ValidateVehiclesAmount(int num);
    }

    public class CompanyService : INumberValidator
    {

        public bool ValidateVehiclesAmount(int num) => num > 2 && num < 150;

        public bool ValidateRouteCost(int cost) => cost > 0 && cost < 599;

    }
}
