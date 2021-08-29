CREATE DATABASE order_db;
CREATE USER order_db_usr IDENTIFIED BY 'order_db_usr_pass';
GRANT SELECT, INSERT, UPDATE, DELETE ON order_db.* TO 'order_db_usr'@'%';

CREATE DATABASE shipping_db;
CREATE USER shipping_db_usr IDENTIFIED BY 'shipping_db_usr_pass';
GRANT SELECT, INSERT, UPDATE, DELETE ON shipping_db.* TO 'shipping_db_usr'@'%';

CREATE DATABASE stock_db;
CREATE USER stock_db_usr IDENTIFIED BY 'stock_db_usr_pass';
GRANT SELECT, INSERT, UPDATE, DELETE ON stock_db.* TO 'stock_db_usr'@'%';