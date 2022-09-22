using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using FakeAtlas.Context.UnitOfWork;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows;
using FakeAtlas.ViewModels.Management;
using FakeAtlas.Models.Entities;
using FakeAtlas.FakeAtlasUIComponents;

namespace FakeAtlas.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private UserOrder selectedOrder;
        public UserOrder SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));

            }
        }

        private List<UserOrder> _orders = new();

        public List<UserOrder> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));

            }
        }

        public OrdersViewModel()
        {
            using (UnitOfWork unit = new())
            {
                Orders = new List<UserOrder>((from order in unit.UserOrderRepository.GetWithInclude(p => p.AvailableOrders, u => u.Client)
                    where order.Client.Id == MainWindowViewModel.User.Id select order).ToList());
            }
            
        }

        private ICommand removeOrderCommand;
        public ICommand RemoveOrderCommand => removeOrderCommand ??= new DelegateCommand(RemoveOrder);

        private void RemoveOrder()
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    unit.UserOrderRepository.Remove(SelectedOrder);
                    unit.Save();
                    Orders = new List<UserOrder>((from order in unit.UserOrderRepository.GetWithInclude(p => p.AvailableOrders) select order).ToList());
                }
            }
            catch (Exception e)
            {
                FakeAtlasMessageBoxService box = new();
                box.ShowMessage(FakeAtlasMessageBox.MessageType.DeleteOrderError, CurrentLocalization);
            }
        }
    }
}
