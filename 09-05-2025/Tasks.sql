-- Cursor-Based Questions (5)
-- Write a cursor that loops through all films and prints titles longer than 120 minutes.

DO $$ 
DECLARE
    film_title VARCHAR;
    film_length INT;
    film_cursor CURSOR FOR
        SELECT title, length FROM film;
BEGIN
    OPEN film_cursor;
    LOOP
        FETCH film_cursor INTO film_title, film_length;
        EXIT WHEN NOT FOUND;
        
        IF film_length > 120 THEN
            RAISE NOTICE 'Title: %, Length: %', film_title, film_length;
        END IF;
    END LOOP;
    CLOSE film_cursor;
END $$;


-- Create a cursor that iterates through all customers and counts how many rentals each made.

DO $$ 
DECLARE
    v_customer_id INT;
    rental_count INT;
    customer_cursor CURSOR FOR
        SELECT customer_id FROM customer;
BEGIN
    OPEN customer_cursor;
    LOOP
        FETCH customer_cursor INTO v_customer_id;
        EXIT WHEN NOT FOUND;
        
        SELECT COUNT(*) INTO rental_count
        FROM rental
        WHERE customer_id = v_customer_id;
        
        RAISE NOTICE 'Customer ID: %, Rentals: %', v_customer_id, rental_count;
    END LOOP;
    CLOSE customer_cursor;
END $$;

-- Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.

DO $$ 
DECLARE
    param_film_id INT;
    param_rental_count INT;
    film_cursor CURSOR FOR
        SELECT f.film_id 
        FROM film f;
BEGIN
    OPEN film_cursor;
    LOOP
        FETCH film_cursor INTO param_film_id;
        EXIT WHEN NOT FOUND;

        SELECT COUNT(*) INTO param_rental_count
        FROM rental r
        JOIN inventory i ON r.inventory_id = i.inventory_id
        WHERE i.film_id = param_film_id;

        IF param_rental_count < 5 THEN
            UPDATE film
            SET rental_rate = rental_rate + 1
            WHERE film_id = param_film_id;
        END IF;
    END LOOP;
    CLOSE film_cursor;
END $$;


-- Create a function using a cursor that collects titles of all films from a particular category.

CREATE OR REPLACE FUNCTION get_films_by_category(param_category_name VARCHAR)
RETURNS TABLE(film_title VARCHAR) AS $$
DECLARE
    param_film_title VARCHAR;
    film_cursor CURSOR FOR
        SELECT f.title
        FROM film f
        JOIN film_category fc ON f.film_id = fc.film_id
        JOIN category c ON fc.category_id = c.category_id
        WHERE c.name = param_category_name;
BEGIN
    OPEN film_cursor;
    LOOP
        FETCH film_cursor INTO param_film_title;
        EXIT WHEN NOT FOUND;
        film_title := param_film_title;  
        RETURN NEXT; 
    END LOOP;
    CLOSE film_cursor;
    RETURN; 
END;
$$ LANGUAGE plpgsql;

SELECT get_films_by_category('Action')

-- Loop through all stores and count how many distinct films are available in each store using a cursor.

DO $$ 
DECLARE
    param_store_id INT;
    param_distinct_film_count INT;
    store_cursor CURSOR FOR
        SELECT store_id FROM store;
BEGIN
    OPEN store_cursor;
    LOOP
        FETCH store_cursor INTO param_store_id;
        EXIT WHEN NOT FOUND;
        
        SELECT COUNT(DISTINCT film_id) INTO param_distinct_film_count
        FROM inventory
        WHERE store_id = param_store_id;
        
        RAISE NOTICE 'Store ID: %, Distinct Films: %', param_store_id, param_distinct_film_count;
    END LOOP;
    CLOSE store_cursor;
END $$;


-- Trigger-Based Questions (5)
-- Write a trigger that logs whenever a new customer is inserted.

CREATE TABLE customer_log (
    log_id SERIAL PRIMARY KEY,
    action VARCHAR(50),
    customer_id INT,
    action_date TIMESTAMP
);


CREATE OR REPLACE FUNCTION log_new_customer()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO customer_log (action, customer_id, action_date)
    VALUES ('INSERT', NEW.customer_id, NOW());
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER new_customer_insert
AFTER INSERT ON customer
FOR EACH ROW
EXECUTE FUNCTION log_new_customer();

INSERT INTO customer (store_id, first_name, last_name, email, address_id, active)
VALUES (1, 'Alice', 'Smith', 'alice.smith@example.com', 1,  1);

SELECT * FROM customer_log
WHERE action = 'INSERT' AND customer_id = (SELECT customer_id FROM customer WHERE email = 'alice.smith@example.com');


-- Create a trigger that prevents inserting a payment of amount 0.

CREATE OR REPLACE FUNCTION prevent_zero_payment()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.amount = 0 THEN
        RAISE EXCEPTION 'Payment amount cannot be zero';
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

-- Set up a trigger to automatically set last_update on the film table before update.

CREATE OR REPLACE FUNCTION update_film_last_update()
RETURNS TRIGGER AS $$
BEGIN
    NEW.last_update := NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_film_last_update_trigger
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION update_film_last_update();

UPDATE film
SET title = 'New Film Title'
WHERE film_id = 1;

SELECT film_id, last_update FROM film WHERE film_id = 1;


-- Create a trigger to log changes in the inventory table (insert/delete).

CREATE OR REPLACE FUNCTION log_inventory_change()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO inventory_log (action, action_date, film_id, store_id)
    VALUES (TG_OP, NOW(), NEW.film_id, NEW.store_id);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER inventory_change_trigger
AFTER INSERT OR DELETE ON inventory
FOR EACH ROW
EXECUTE FUNCTION log_inventory_change();




-- Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.

CREATE OR REPLACE FUNCTION prevent_rental_for_owed_customer()
RETURNS TRIGGER AS $$
DECLARE
    param_total_owed DECIMAL;
BEGIN
    SELECT SUM(amount) INTO param_total_owed
    FROM payment
    WHERE customer_id = NEW.customer_id;

    IF param_total_owed > 50 THEN
        RAISE EXCEPTION 'Customer owes more than $50 and cannot rent a film';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER prevent_rental_for_owed_customer_trigger
BEFORE INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION prevent_rental_for_owed_customer();


-- Transaction-Based Questions (5)
-- Write a transaction that inserts a customer and an initial rental in one atomic operation.

BEGIN;

INSERT INTO customer (store_id, first_name, last_name, email, address_id, active)
VALUES (1, 'John', 'Doe', 'john.doe@example.com', 1, 1);

INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
VALUES (
    NOW(),
    1,
    (SELECT customer_id FROM customer WHERE email = 'john.doe@example.com'),
	1
);

COMMIT;

ROLLBACK;

-- Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.

BEGIN;

UPDATE film
SET rental_rate = rental_rate + 1
WHERE film_id = 1;

INSERT INTO inventory (store_id, film_id)
VALUES (NULL, 1); 

COMMIT;

ROLLBACK;
-- Create a transaction that transfers an inventory item from one store to another.

BEGIN;

UPDATE inventory
SET store_id = 2
WHERE store_id = 1 AND film_id = 1;


COMMIT;

-- Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.
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
-- Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

BEGIN;

DELETE FROM payment
WHERE customer_id = 1;

DELETE FROM rental
WHERE customer_id = 1;

DELETE FROM customer
WHERE customer_id = 1;

COMMIT;

ROLLBACK;