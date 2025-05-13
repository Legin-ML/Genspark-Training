-- Cursors 
-- Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.

CREATE TABLE summary (
	customer_id INT PRIMARY KEY,
	customer_name VARCHAR(100),
	rental_count INT
)

DO $$
DECLARE 
record RECORD;
cur_customer_rentals CURSOR FOR
SELECT c.customer_id, c.first_name || ' ' || c.last_name as "customer_name", COUNT(r.customer_id) as rental_count
FROM customer as c JOIN rental as r on c.customer_id = r.customer_id GROUP BY c.customer_id;

BEGIN
	OPEN cur_customer_rentals;

	LOOP
		FETCH cur_customer_rentals INTO record;
		EXIT WHEN NOT FOUND;

		INSERT INTO summary (customer_id, customer_name, rental_count) VALUES (record.customer_id, record.customer_name, record.rental_count);

	END LOOP;

	CLOSE cur_customer_rentals;
END;
$$;

SELECT * from summary;


-- Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.

DO $$
DECLARE 
record RECORD;
cur_comedy CURSOR FOR
SELECT f.title as "title"FROM film as f
JOIN film_category as fc ON fc.film_id = f.film_id
JOIN category as c ON c.category_id = fc.category_id
JOIN inventory as i ON i.film_id = f.film_id
JOIN rental as r ON i.inventory_id = r.inventory_id
WHERE c."name" = 'Comedy'
GROUP BY f.title
HAVING COUNT(r.rental_id) > 10;

BEGIN
	OPEN cur_comedy;

	LOOP
		FETCH cur_comedy INTO record;
		EXIT WHEN NOT FOUND;

		
		RAISE NOTICE 'Film Title: %', record.title;
	END LOOP;

	CLOSE cur_comedy;
END;
$$;

 
-- Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.

CREATE TABLE report (
	store_id INT PRIMARY KEY,
	film_count INT
)

DO $$
DECLARE 
record RECORD;
cur_unique_count CURSOR FOR
SELECT DISTINCT store_id from store;

BEGIN
	OPEN cur_unique_count;

	LOOP
		FETCH cur_unique_count INTO record;
		EXIT WHEN NOT FOUND;

		INSERT INTO report (store_id, film_count) VALUES (record.store_id, (SELECT COUNT(DISTINCT film_id) from inventory WHERE inventory.store_id = record.store_id));

	END LOOP;

	CLOSE cur_unique_count;
END;
$$;

SELECT * from report;
 
-- Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.

DROP TABLE IF EXISTS inactive_customers;

CREATE TABLE inactive_customers (
    customer_id INT PRIMARY KEY,
    full_name TEXT
);

DO $$
DECLARE
    cust_rec RECORD;
    cur_inactive CURSOR FOR
        SELECT customer_id, first_name || ' ' || last_name AS full_name
        FROM customer
        WHERE customer_id NOT IN (
            SELECT DISTINCT customer_id
            FROM rental
            WHERE rental_date >= NOW() - INTERVAL '6 months'
        );
BEGIN
    OPEN cur_inactive;
    
    LOOP
        FETCH cur_inactive INTO cust_rec;
        EXIT WHEN NOT FOUND;
        
        INSERT INTO inactive_customers (customer_id, full_name)
        VALUES (cust_rec.customer_id, cust_rec.full_name);
    END LOOP;

    CLOSE cur_inactive;
END $$;

SELECT * FROM inactive_customers;

-- --------------------------------------------------------------------------
 
-- Transactions 
-- Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.

BEGIN;

-- Insert new customer
INSERT INTO customer (first_name, last_name, email, store_id, address_id) 
VALUES ('John', 'Doe', 'john.doe@example.com', 1, 1);

-- Add rental
INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id) 
VALUES (NOW(), 1, CURRVAL('customer_customer_id_seq'), 1);

-- Log payment
INSERT INTO payment (amount, payment_date, rental_id, customer_id, staff_id) 
VALUES (6.99, NOW(), CURRVAL('rental_rental_id_seq'), CURRVAL('customer_customer_id_seq'), 1);

COMMIT;

ROLLBACK;
 
-- Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.

BEGIN;

UPDATE rental SET rental_date = NOW() WHERE rental_id = 'WRONG'; 

COMMIT;

ROLLBACK; --other commits can only progress if rolled back
 
-- Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.

SELECT * FROM payment WHERE payment_id IN (17503, 17504);
BEGIN;

UPDATE payment
SET amount = 100
WHERE payment_id = 17503;

SAVEPOINT sp_before_update;


UPDATE payment
SET amount = 50
WHERE payment_id = 17504;

ROLLBACK TO SAVEPOINT sp_before_update;

COMMIT;

SELECT * FROM payment WHERE payment_id IN (17503, 17504);
 
-- Perform a transaction that transfers inventory from one store to another (delete + insert) safely.

BEGIN;

UPDATE inventory
SET store_id = 2
WHERE store_id = 1 AND film_id = 1;

COMMIT;

ROLLBACK;

 
-- Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.

BEGIN;

DELETE FROM payment
WHERE customer_id = 1;

DELETE FROM rental
WHERE customer_id = 1;

DELETE FROM customer
WHERE customer_id = 1;

COMMIT;

ROLLBACK;
-- ----------------------------------------------------------------------------
 
-- Triggers
-- Create a trigger to prevent inserting payments of zero or negative amount.

DROP TRIGGER prevent_zero_payment_trigger ON payment

CREATE OR REPLACE FUNCTION prevent_zero_payment()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.amount <= 0 THEN
        RAISE EXCEPTION 'Payment amount cannot be zero or negative';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER prevent_zero_payment_trigger
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_zero_payment();

INSERT INTO payment (customer_id, staff_id, rental_id, amount)
VALUES (1, 1, 1, 0);
 
-- Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.

CREATE OR REPLACE FUNCTION update_film_last_update()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.title IS DISTINCT FROM OLD.title OR NEW.rental_rate IS DISTINCT FROM OLD.rental_rate THEN
    NEW.last_update = NOW();
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER film_update_trigger
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION update_film_last_update();


UPDATE film
SET title = 'New Film Title'
WHERE film_id = 1;

SELECT film_id, last_update FROM film WHERE film_id = 1;
 
-- Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.
DROP TABLE IF EXISTS rental_log;

CREATE TABLE rental_log (
    inventory_id INT PRIMARY KEY,
    film_title TEXT,
    log_date TIMESTAMP WITHOUT TIME ZONE
);

CREATE OR REPLACE FUNCTION log_frequent_rentals()
RETURNS TRIGGER AS $$
DECLARE
    rental_count INT;
    title TEXT;
BEGIN
    SELECT COUNT(*) INTO rental_count
    FROM rental r
    WHERE r.inventory_id = NEW.inventory_id
      AND r.rental_date >= NOW() - INTERVAL '7 days';

    IF rental_count > 3 THEN
        SELECT f.title INTO title
        FROM inventory i
        JOIN film f ON i.film_id = f.film_id
        WHERE i.inventory_id = NEW.inventory_id;

        INSERT INTO rental_log (inventory_id, film_title, log_date)
        VALUES (NEW.inventory_id, title, NOW());
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;



CREATE TRIGGER frequent_rental_log
AFTER INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION log_frequent_rentals();

INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (
    NOW(),     
    1,          
    6,           
    NULL,        
    1,           
    NOW()        
);

select * from rental_log
