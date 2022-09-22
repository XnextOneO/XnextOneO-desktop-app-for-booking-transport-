using FakeAtlas.Context.Repository;
using FakeAtlas.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAtlas.Context.UnitOfWork
{
    public class UnitOfWork : IDisposable //закрывает работу с хранилищем   
    {
        private readonly BookingDBContext db = new();

        private BookingDBRepository<BookingUser> userRepository;

        public BookingDBRepository<BookingUser> BookingUsers
        {
            get
            {
                if (userRepository == null)
                    userRepository = new BookingDBRepository<BookingUser>(db);
                return userRepository;
            }
        }

        private BookingDBRepository<UniqueAddress> addressRepository;

        public BookingDBRepository<UniqueAddress> AddressRepository
        {
            get
            {
                if (addressRepository == null)
                    addressRepository = new BookingDBRepository<UniqueAddress>(db);
                return addressRepository;
            }
        }


        private BookingDBRepository<Order> ordersRepository;

        public BookingDBRepository<Order> OrdersRepository
        {
            get
            {
                if (ordersRepository == null)
                    ordersRepository = new BookingDBRepository<Order>(db);
                return ordersRepository;
            }
        }

        private BookingDBRepository<AvailableOrder> availableOrdersRepository;

        public BookingDBRepository<AvailableOrder> AvailableOrdersRepository
        {
            get
            {
                if (availableOrdersRepository == null)
                    availableOrdersRepository = new BookingDBRepository<AvailableOrder>(db);
                return availableOrdersRepository;
            }
        }

        private BookingDBRepository<Shipper> shipperRepository;

        public BookingDBRepository<Shipper> ShipperRepository
        {
            get
            {
                if (shipperRepository == null)
                    shipperRepository = new BookingDBRepository<Shipper>(db);
                return shipperRepository;
            }
        }

        private BookingDBRepository<UserOrder> userOrderRepository;

        public BookingDBRepository<UserOrder> UserOrderRepository
        {
            get
            {
                if (userOrderRepository == null)
                    userOrderRepository = new BookingDBRepository<UserOrder>(db);
                return userOrderRepository;
            }
        }


        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose(); // Освобождаем управляемые ресурсы
                }
                this.disposed = true;  // освобождаем неуправляемые объекты
            }
        }
        // освобождаем неуправляемые ресурсы
        public void Dispose()
        {
            Dispose(true);// подавляем финализацию
            GC.SuppressFinalize(this);
        }
    }
}

