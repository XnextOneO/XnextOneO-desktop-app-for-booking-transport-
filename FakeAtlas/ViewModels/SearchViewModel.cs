using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FakeAtlas.Context.UnitOfWork;
using FakeAtlas.FakeAtlasUIComponents;
using FakeAtlas.Models.Entities;
using FakeAtlas.Services;
using FakeAtlas.ViewModels.Management;
using FluentValidation.Results;
using Microsoft.VisualStudio.PlatformUI;

namespace FakeAtlas.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private ObservableCollection<AvailableOrder> _selectedOrders = new();

        public ObservableCollection<AvailableOrder> SelectedOrders
        {
            get { return _selectedOrders; }
            set
            {
                _selectedOrders = value;
                OnPropertyChanged(nameof(SelectedOrders));
            }
        }

        private AvailableOrder _selectedOrder = new() { DepartureTime = DateTime.Now};

        public AvailableOrder SelectedOrder
        {
            get { return _selectedOrder; }
            set 
            { 
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }


        public SearchViewModel()
        {
            
        }

        private ICommand selectCommand;
        public ICommand SelectCommand => selectCommand ??= new DelegateCommand(Select);

        private void Select()
        {
            var order = new UserOrder { AvailableOrdersId = SelectedOrder.Id, ClientId = MainWindowViewModel.User.Id, OrderTime = DateTime.Now};
            FakeAtlasMessageBoxService box = new();
            using (UnitOfWork unit = new())
            {
                var exsitedOrder = (from neworder in unit.UserOrderRepository.Get()
                                    where neworder.ClientId == order.ClientId &&
                                    neworder.AvailableOrdersId == order.AvailableOrdersId
                                    select neworder).SingleOrDefault();
                
                if(!(exsitedOrder == null))
                {
                    if (exsitedOrder.AvailableOrdersId == order.AvailableOrdersId
                                        && exsitedOrder.ClientId == order.ClientId)
                    {
                        box.ShowMessage(FakeAtlasMessageBox.MessageType.AlrdyExistingOrder, CurrentLocalization);
                        return;
                    }
                }
                if (box.Show())
                {
                    unit.UserOrderRepository.Create(order);
                    unit.Save();
                }
                else
                {
                    return;
                }
            }
            
        }

        private ICommand findCommand;
        public ICommand FindCommand => findCommand ??= new DelegateCommand(Find);

        private void Find()
        {
            try
            {
                if (SelectedOrder.PathTo == null)
                {
                    MessageBox.Show("Выберите пункт назначения");
                    return;
                }

                if (SelectedOrder.PathFrom == null)
                {
                    MessageBox.Show("Выберите пункт отправления");
                    return;
                }

                ValidatorPath validatorPathTo = new ValidatorPath();
                ValidationResult validationPathToResult = validatorPathTo.Validate(SelectedOrder.PathTo);
                if (!validationPathToResult.IsValid)
                {
                    MessageBox.Show(validationPathToResult.Errors.First().ErrorMessage);
                    return;
                }

                ValidatorPath validatorPathFrom = new ValidatorPath();
                ValidationResult validationPathFromResult = validatorPathFrom.Validate(SelectedOrder.PathFrom);
                if (!validationPathFromResult.IsValid)
                {
                    MessageBox.Show(validationPathFromResult.Errors.First().ErrorMessage);
                    return;
                }   



                using (UnitOfWork unit = new())  //конструктор получает параметры из конфигурационного файла
                
                {
                var availableOrders = from order in unit.AvailableOrdersRepository.Get() 
                                          where order.DepartureTime.GetValueOrDefault().Year == SelectedOrder.DepartureTime.GetValueOrDefault().Year &&
                 order.DepartureTime.GetValueOrDefault().Month == SelectedOrder.DepartureTime.GetValueOrDefault().Month &&
                 order.DepartureTime.GetValueOrDefault().Day == SelectedOrder.DepartureTime.GetValueOrDefault().Day &&
                 order.PathFrom.Equals(SelectedOrder.PathFrom) && order.PathTo.Equals(SelectedOrder.PathTo)
                                      select order; //смотрит в базу на наличие записей с такими же параметрами

                    SelectedOrders = new ObservableCollection<AvailableOrder>(availableOrders); // выводит в коллекцию все найденные записи
                }
            }
            catch (Exception e)
            {
                FakeAtlasMessageBoxService box = new();
                box.ShowMessage(e.Message);
            }
        }
    }
}
