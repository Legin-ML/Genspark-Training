CREATE TABLE EMP (
    emp_no INT PRIMARY KEY,
    emp_name VARCHAR(50) NOT NULL,
    salary MONEY NOT NULL,
    dept_name VARCHAR(50) NULL,
    boss_no INT NULL,
)

CREATE TABLE DEPARTMENT (
    dept_name VARCHAR(50) PRIMARY KEY,
    floor INT NOT NULL,
    phone VARCHAR(13) NOT NULL,
    mgnr_no INT NOT NULL,
)

CREATE TABLE SALES (
    sales_no INT PRIMARY KEY,
    saleqty INT,
    item_name VARCHAR(50),
    dept_name VARCHAR(50)

)

CREATE TABLE ITEM (
    item_name VARCHAR(50) PRIMARY KEY,
    item_type VARCHAR(50),
    item_color VARCHAR(50)
)


INSERT INTO EMP VALUES (1, 'Alice', 75000, 'Management', NULL);
INSERT INTO EMP VALUES (2, 'Ned', 45000, 'Marketing', 1);
INSERT INTO EMP VALUES (3, 'Andrew', 25000, 'Marketing', 2);
INSERT INTO EMP VALUES (4, 'Clare', 22000, 'Marketing', 2);
INSERT INTO EMP VALUES (5, 'Todd', 38000, 'Accounting', 1);
INSERT INTO EMP VALUES (6, 'Nancy', 22000, 'Accounting', 5);
INSERT INTO EMP VALUES (7, 'Brier', 43000, 'Purchasing', 1);
INSERT INTO EMP VALUES (8, 'Sarah', 56000, 'Purchasing', 7);
INSERT INTO EMP VALUES (9, 'Sophia', 35000, 'Personnel', 1);
INSERT INTO EMP VALUES (10, 'Sanjay', 15000, 'Navigation', 3);
INSERT INTO EMP VALUES (11, 'Rita', 15000, 'Books', 4);
INSERT INTO EMP VALUES (12, 'Gigi', 16000, 'Clothes', 4);
INSERT INTO EMP VALUES (13, 'Maggie', 11000, 'Clothes', 4);
INSERT INTO EMP VALUES (14, 'Paul', 15000, 'Equipment', 3);
INSERT INTO EMP VALUES (15, 'James', 15000, 'Equipment', 3);
INSERT INTO EMP VALUES (16, 'Pat', 15000, 'Furniture', 3);
INSERT INTO EMP VALUES (17, 'Mark', 15000, 'Recreation', 3);

INSERT INTO DEPARTMENT VALUES ('Management', 5, 34, 1);
INSERT INTO DEPARTMENT VALUES ('Books', 1, 81, 4);
INSERT INTO DEPARTMENT VALUES ('Clothes', 2, 24, 4);
INSERT INTO DEPARTMENT VALUES ('Equipment', 3, 57, 3);
INSERT INTO DEPARTMENT VALUES ('Furniture', 4, 14, 3);
INSERT INTO DEPARTMENT VALUES ('Navigation', 1, 41, 3);
INSERT INTO DEPARTMENT VALUES ('Recreation', 2, 29, 4);
INSERT INTO DEPARTMENT VALUES ('Accounting', 5, 35, 5);
INSERT INTO DEPARTMENT VALUES ('Purchasing', 5, 36, 7);
INSERT INTO DEPARTMENT VALUES ('Personnel', 5, 37, 9);
INSERT INTO DEPARTMENT VALUES ('Marketing', 5, 38, 2);

INSERT INTO SALES VALUES (101, 2, 'Boots-snake proof', 'Clothes');
INSERT INTO SALES VALUES (102, 1, 'Pith Helmet', 'Clothes');
INSERT INTO SALES VALUES (103, 1, 'Sextant', 'Navigation');
INSERT INTO SALES VALUES (104, 3, 'Hat-polar Explorer', 'Clothes');
INSERT INTO SALES VALUES (105, 5, 'Pith Helmet', 'Equipment');
INSERT INTO SALES VALUES (106, 2, 'Pocket Knife-Nile', 'Clothes');
INSERT INTO SALES VALUES (107, 3, 'Pocket Knife-Nile', 'Recreation');
INSERT INTO SALES VALUES (108, 1, 'Compass', 'Navigation');
INSERT INTO SALES VALUES (109, 2, 'Geo positioning system', 'Navigation');
INSERT INTO SALES VALUES (110, 5, 'Map Measure', 'Navigation');
INSERT INTO SALES VALUES (111, 1, 'Geo positioning system', 'Books');
INSERT INTO SALES VALUES (112, 1, 'Sextant', 'Books');
INSERT INTO SALES VALUES (113, 3, 'Pocket Knife-Nile', 'Books');
INSERT INTO SALES VALUES (114, 1, 'Pocket Knife-Nile', 'Navigation');
INSERT INTO SALES VALUES (115, 1, 'Pocket Knife-Nile', 'Equipment');
INSERT INTO SALES VALUES (116, 1, 'Sextant', 'Clothes');
INSERT INTO SALES VALUES (117, 1, 'Sextant', 'Equipment');
INSERT INTO SALES VALUES (118, 1, 'Sextant', 'Recreation');
INSERT INTO SALES VALUES (119, 1, 'Sextant', 'Furniture');
INSERT INTO SALES VALUES (120, 1, 'Pocket Knife-Nile', 'Furniture');
INSERT INTO SALES VALUES (121, 1, 'Exploring in 10 easy lessons', 'Books');
INSERT INTO SALES VALUES (122, 1, 'How to win foreign friends', 'Furniture');
INSERT INTO SALES VALUES (123, 1, 'Compass', 'Furniture');
INSERT INTO SALES VALUES (124, 1, 'Pith Helmet', 'Furniture');
INSERT INTO SALES VALUES (125, 1, 'Elephant Polo stick', 'Recreation');
INSERT INTO SALES VALUES (126, 1, 'Camel Saddle', 'Recreation');

INSERT INTO ITEM VALUES ('Pocket Knife-Nile', 'E', 'Brown');
INSERT INTO ITEM VALUES ('Pocket Knife-Avon', 'E', 'Brown');
INSERT INTO ITEM VALUES ('Compass', 'N', NULL);
INSERT INTO ITEM VALUES ('Geo positioning system', 'N', NULL);
INSERT INTO ITEM VALUES ('Elephant Polo stick', 'R', 'Bamboo');
INSERT INTO ITEM VALUES ('Camel Saddle', 'R', 'Brown');
INSERT INTO ITEM VALUES ('Sextant', 'N', NULL);
INSERT INTO ITEM VALUES ('Map Measure', 'N', NULL);
INSERT INTO ITEM VALUES ('Boots-snake proof', 'C', 'Green');
INSERT INTO ITEM VALUES ('Pith Helmet', 'C', 'Khaki');
INSERT INTO ITEM VALUES ('Hat-polar Explorer', 'C', 'White');
INSERT INTO ITEM VALUES ('Exploring in 10 Easy Lessons', 'B', NULL);
INSERT INTO ITEM VALUES ('Hammock', 'F', 'Khaki');
INSERT INTO ITEM VALUES ('How to win Foreign Friends', 'B', NULL);
INSERT INTO ITEM VALUES ('Map case', 'E', 'Brown');
INSERT INTO ITEM VALUES ('Safari Chair', 'F', 'Khaki');
INSERT INTO ITEM VALUES ('Safari cooking kit', 'F', 'Khaki');
INSERT INTO ITEM VALUES ('Stetson', 'C', 'Black');
INSERT INTO ITEM VALUES ('Tent - 2 person', 'F', 'Khaki');
INSERT INTO ITEM VALUES ('Tent -8 person', 'F', NULL);

ALTER TABLE EMP ADD CONSTRAINT fk_dept_name FOREIGN KEY (dept_name) REFERENCES DEPARTMENT(dept_name) ON DELETE SET NULL;
ALTER TABLE EMP ADD CONSTRAINT fk_boss_no FOREIGN KEY (boss_no) REFERENCES EMP(emp_no) ON DELETE SET NULL;

ALTER TABLE DEPARTMENT ADD CONSTRAINT fk_mgnr_no FOREIGN KEY (mgnr_no) REFERENCES EMP(emp_no) ON DELETE NO ACTION;
ALTER TABLE SALES ADD CONSTRAINT fk_item_name FOREIGN KEY (item_name) REFERENCES ITEM(item_name) ON DELETE NO ACTION;
ALTER TABLE SALES ADD CONSTRAINT fk_dept_name_sales FOREIGN KEY (dept_name) REFERENCES DEPARTMENT(dept_name) ON DELETE NO ACTION;
