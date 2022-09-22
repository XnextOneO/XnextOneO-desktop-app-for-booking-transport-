use BookingDB;

create table unique_address(
id int constraint pk_address_id primary key(id) IDENTITY(1,1),
city nvarchar(20),
oblast nvarchar(20),
street_name nvarchar(20),
building_num int
);

create table booking_user(
id int constraint pk_user primary key(id) IDENTITY(1,1),
full_name nvarchar(100),
id_address int constraint fk_idx_address foreign key(id_address) references unique_address(id),
is_admin bit not null,
user_login nvarchar(256) unique,
user_password nvarchar(256),
salt nvarchar(256)
);

create table shippers(
id int constraint pk_id primary key(id) IDENTITY(1,1),
full_name nvarchar(100),
vehicle_type nvarchar(100),
vehicle_num int
);
create table available_orders(
id int constraint pk_available_order primary key(id) IDENTITY(1,1),
path_from nvarchar(100),
path_to nvarchar(100),
departure_time datetime,
cost int,
shipper_id int constraint fk_shipper_id foreign key(shipper_id) references shippers(id)
);

create table user_orders(
id int constraint pk_orders_id primary key(id) IDENTITY(1,1), 
available_orders_id int constraint  fk_available_orders_id foreign key(available_orders_id) references available_orders(id),
client_id  int constraint  fk_client_id foreign key(client_id) references booking_user(id),
order_time datetime 
);