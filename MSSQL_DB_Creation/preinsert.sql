USE BookingDB;

GO
CREATE TRIGGER order_user_insert ON user_orders AFTER INSERT 
AS
INSERT INTO order_user (id_user, id_order) VALUES(NEW.)

GO
INSERT INTO shippers (full_name, vehicle_type, vehicle_num)
values('TIHIY OMUT', 'Mercedes-Benz', 12)

INSERT INTO available_orders (path_from, path_to, departure_time, cost, shipper_id)
values('Минск','Гродно','2021-07-06 13:20:00', 150, 1)

INSERT INTO available_orders (path_from, path_to, departure_time, cost, shipper_id)
values('Минск','Гродно','2021-07-06 15:20:00', 150, 1)

INSERT INTO available_orders (path_from, path_to, departure_time, cost, shipper_id)
values('Минск','Гродно','2021-07-06 20:00:00', 150, 1)

