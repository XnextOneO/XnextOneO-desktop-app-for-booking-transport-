using FakeAtlas.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using FakeAtlas.Context.UnitOfWork;
using System.Text.RegularExpressions;
using FakeAtlas.ViewModels.Management;
using FakeAtlas.FakeAtlasUIComponents;
using FakeAtlas.Services;
using System.Diagnostics;
using System.Windows.Forms;

namespace FakeAtlas.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        #region field regex
        public static Regex CostRegex = new(@"^\d{1, }$"); 
        public static Regex TimeRegex = new(@"^(\d{4})\\(\d{2})\\(\d{2}) (\d{2}):(\d{2}):(\d{2})&"); 
        public static Regex DigitalRegex = new(@"^\d$");
        #endregion

        #region Collections
        private ObservableCollection<AvailableOrder> _selectedOrders = new(); //коллекция для выбранных заказов

        public ObservableCollection<AvailableOrder> SelectedOrders 
        {
            get { return _selectedOrders; }
            set
            {
                _selectedOrders = value;
                OnPropertyChanged(nameof(SelectedOrders));
            }
        }

        private ObservableCollection<Shipper> _selectedShippers = new(); // коллекция для выбранных перевозчиков

        public ObservableCollection<Shipper> SelectedShippers
        {
            get { return _selectedShippers; }
            set
            {
                _selectedShippers = value;
                OnPropertyChanged(nameof(SelectedShippers));
            }
        }

        private ObservableCollection<BookingUser> _selectedUsers = new(); //коллекция для выбранных пользователей

        public ObservableCollection<BookingUser> SelectedUsers
        {
            get { return _selectedUsers; }
            set { _selectedUsers = value; OnPropertyChanged(nameof(SelectedUsers)); }
        }

        #endregion

        #region SelectedItems in ListView
        private AvailableOrder _selectedOrder = new(); //выбранный заказ

        public AvailableOrder SelectedOrder
        {
            get { return _selectedOrder; }
            set
            { try
                {
                    _selectedOrder = value;
                    CompanyName = value.Shipper == null ? CompanyName : value.Shipper.FullName;
                    OnPropertyChanged(nameof(SelectedOrder));
                }
                catch (Exception ex)
                {
                 
                }
            }
        }

        private Shipper _selectedShipper = new(); //выбранный перевозчик

        public Shipper SelectedShipper
        {
            get { return _selectedShipper; }
            set
            {
                _selectedShipper = value;
                CompanyName = value.FullName;
                OnPropertyChanged(nameof(SelectedShipper));
            }
        }

        private string _companyName = "";

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private BookingUser _selectedUser; // выбранный пользователь

        public BookingUser SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }
        #endregion

        #region Items binding to creation

        private AvailableOrder _selectedOrderCreating = new(); 

        public AvailableOrder SelectedOrderCreating
        {
            get { return _selectedOrderCreating; }
            set
            {
                _selectedOrderCreating = value;
                CompanyName = value.Shipper == null ? CompanyName : value.Shipper.FullName;
                OnPropertyChanged(nameof(SelectedOrderCreating));
            }
        }

        private Shipper _selectedShipperCreating = new();

        public Shipper SelectedShipperCreating
        {
            get { return _selectedShipperCreating; }
            set
            {
                _selectedShipperCreating = value;
                CompanyName = value.FullName;
                OnPropertyChanged(nameof(SelectedShipperCreating));
            }
        }

        #endregion

        public AdminViewModel()
        {
            using (UnitOfWork unit = new())
            {
                SelectedOrders = new ObservableCollection<AvailableOrder>(from item in unit.AvailableOrdersRepository.GetWithInclude(o => o.Shipper) select item);
                SelectedShippers = new ObservableCollection<Shipper>(from item in unit.ShipperRepository.GetWithInclude(o => o.AvailableOrders) select item);
                SelectedUsers = new ObservableCollection<BookingUser>(from item in unit.BookingUsers.Get() where !item.IsAdmin select item);
            }
        }

        private ICommand saveCompanyCommand;
        public ICommand SaveCompanyCommand => saveCompanyCommand ??= new DelegateCommand(SaveCompany);

        private void SaveCompany()
        {
            INumberValidator ordersService = new CompanyService(); 
            IMessageBoxService box = new FakeAtlasMessageBoxService();
            try
            {
                using (UnitOfWork unit = new())
                {
                    try
                    {
                        if (!ordersService.ValidateVehiclesAmount(SelectedShipperCreating.VehicleNum.Value))
                        {
                            box.ShowMessage(FakeAtlasMessageBox.MessageType.InvalidNumber, CurrentLocalization);
                            return;
                        }
                    }
                    finally
                    {
                        unit.ShipperRepository.Create(SelectedShipperCreating);
                        unit.Save();
                        SelectedShippers = new ObservableCollection<Shipper>(from item in unit.ShipperRepository.GetWithInclude(o => o.AvailableOrders) select item);
                        SelectedOrder = new();
                        SelectedShipper = new();
                        SelectedOrderCreating = new();
                        SelectedShipperCreating = new();
                    }
                }
            }

            catch (Exception e)
            {
                box.ShowMessage(e.Message);
            }
        }

        private ICommand saveRouteCommand;
        public ICommand SaveRouteCommand => saveRouteCommand ??= new DelegateCommand(SaveRoute);

        private void SaveRoute()
        {
            INumberValidator ordersService = new CompanyService();
            IMessageBoxService box = new FakeAtlasMessageBoxService();
            try
            {

                if (!ordersService.ValidateRouteCost(SelectedOrderCreating.Cost.Value))
                {
                    box.ShowMessage(FakeAtlasMessageBox.MessageType.InvalidNumber, CurrentLocalization);
                    return;
                }

                using (UnitOfWork unit = new())
                {
                    // Find CompanyName and set SelectedOrderCreating.shipperId
                    var shipper = unit.ShipperRepository.GetWithInclude(o => o.FullName == CompanyName).FirstOrDefault();
                    if (shipper == null)
                    {
                        MessageBox.Show("Не найдена компания");
                        return;
                    }
                    SelectedOrderCreating.ShipperId = shipper.Id;

                    unit.AvailableOrdersRepository.Create(SelectedOrderCreating);
                    unit.Save();
                    SelectedOrders = new ObservableCollection<AvailableOrder>(from item in unit.AvailableOrdersRepository.GetWithInclude(o => o.Shipper) select item);
                    
                    SelectedOrder = new();
                    SelectedShipper = new();
                    SelectedOrderCreating = new();
                    SelectedShipperCreating = new();
                }
            }
            catch (Exception e)
            {
                box.ShowMessage(e.Message);
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
                    unit.AvailableOrdersRepository.Remove(SelectedOrder);
                    unit.Save();
                    SelectedOrders = new ObservableCollection<AvailableOrder>(from item in unit.AvailableOrdersRepository.GetWithInclude(o => o.Shipper) select item);
                    SelectedOrder = new();
                    SelectedShipper = new();
                }
            }
            catch (Exception e)
            {
                FakeAtlasMessageBoxService box = new();
                box.ShowMessage(FakeAtlasMessageBox.MessageType.DeleteOrderError, CurrentLocalization);
            }
        }

        private ICommand removeCompanyCommand;
        public ICommand RemoveCompanyCommand => removeCompanyCommand ??= new DelegateCommand(RemoveCompany);

        private void RemoveCompany()
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    unit.ShipperRepository.Remove(SelectedShipper);
                    unit.Save();
                    SelectedOrders = new ObservableCollection<AvailableOrder>(from item in unit.AvailableOrdersRepository.GetWithInclude(o => o.Shipper) select item);
                    SelectedShippers = new ObservableCollection<Shipper>(from item in unit.ShipperRepository.GetWithInclude(o => o.AvailableOrders) select item);
                    SelectedOrder = new();
                    SelectedShipper = new();
                }
            }
            catch (Exception e)
            {
                FakeAtlasMessageBoxService box = new();
                box.ShowMessage(FakeAtlasMessageBox.MessageType.DeleteCompanyError, CurrentLocalization);
            }
        }

        private ICommand removeUserCommand;
        public ICommand RemoveUserCommand => removeUserCommand ??= new DelegateCommand(RemoveUser);

        private void RemoveUser()
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    unit.BookingUsers.Remove(SelectedUser);
                    unit.Save();
                    SelectedUsers = new ObservableCollection<BookingUser>(from item in unit.BookingUsers.Get() where !item.IsAdmin select item);
                    SelectedUser = new();
                    SelectedOrder = new();
                    SelectedShipper = new();
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
