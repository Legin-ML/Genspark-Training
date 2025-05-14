/*
Objective:
Create a stored procedure that inserts rental data on the primary server, and verify that changes replicate to the standby server. Add a logging mechanism to track each operation.

Tasks to Complete:
Set up streaming replication (if not already done):

Primary on port 5432

Standby on port 5433

Create a table on the primary:


CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    rental_time TIMESTAMP,
    customer_id INT,
    film_id INT,
    amount NUMERIC,
    logged_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
Ensure this table is replicated.

Write a stored procedure to:

Insert a new rental log entry

Accept customer_id, film_id, amount as inputs

Wrap logic in a transaction with error handling (BEGIN...EXCEPTION...END)


CREATE OR REPLACE PROCEDURE sp_add_rental_log(
    p_customer_id INT,
    p_film_id INT,
    p_amount NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (rental_time, customer_id, film_id, amount)
    VALUES (CURRENT_TIMESTAMP, p_customer_id, p_film_id, p_amount);
EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error occurred: %', SQLERRM;
END;
$$;
Call the procedure on the primary:


CALL sp_add_rental_log(1, 100, 4.99);
On the standby (port 5433):

Confirm that the new record appears in rental_log

Run:SELECT * FROM rental_log ORDER BY log_id DESC LIMIT 1;

Add a trigger to log any UPDATE to rental_log
*/

CREATE TABLE rental_log (
    log_id SERIAL PRIMARY KEY,
    rental_time TIMESTAMP,
    customer_id INT,
    film_id INT,
    rental_id INT,
    logged_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE PROCEDURE sp_add_rental_log(
    p_customer_id INT,
    p_film_id INT,
    p_rental_id NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (rental_time, customer_id, film_id, rental_id)
    VALUES (CURRENT_TIMESTAMP, p_customer_id, p_film_id, p_rental_id);
EXCEPTION WHEN OTHERS THEN
    RAISE NOTICE 'Error occurred: %', SQLERRM;
END;
$$;

CALL sp_add_rental_log(1, 100, 4);
	

CREATE OR REPLACE PROCEDURE sp_insert_into_log(
    p_rental_time TIMESTAMP WITHOUT TIME ZONE,
    p_customer_id SMALLINT,
    p_film_id SMALLINT,
    p_rental_id INTEGER,
	p_logged_on TIMESTAMP WITH TIME ZONE
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log (
        rental_time,
        customer_id,
        film_id,
        rental_id,
        logged_on
    )
    VALUES (
        p_rental_time, 
        p_customer_id::INT,
        p_film_id::INT,
        p_rental_id,
		p_logged_on
    );
END;
$$;

CREATE OR REPLACE FUNCTION trigger_log_update()
RETURNS TRIGGER AS $$
DECLARE
v_film_id SMALLINT;
BEGIN

	SELECT film_id INTO v_film_id
	FROM inventory WHERE inventory_id = NEW.inventory_id;

    CALL sp_insert_into_log(
        NEW.rental_date,       
        NEW.customer_id,       
        v_film_id,           
        NEW.rental_id,           
		CURRENT_TIMESTAMP
    );
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER rental_update_trigger
AFTER UPDATE ON rental
FOR EACH ROW
EXECUTE FUNCTION trigger_log_update();

UPDATE rental
SET staff_id = 2
WHERE rental_id = 1;







