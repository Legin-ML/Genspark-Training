CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- 1. Create a stored procedure to encrypt a given text
--Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.

CREATE OR REPLACE FUNCTION fn_encrypt_text(
	 v_input TEXT
) 
RETURNS TEXT AS
$$
BEGIN
	RETURN pgp_sym_encrypt(v_input, 'SuperSecureH@5h');
END;
$$
LANGUAGE plpgsql;

SELECT fn_encrypt_text('Hello');

CREATE OR REPLACE PROCEDURE sp_encrypt_text(
    IN v_input TEXT,
    OUT v_encrypted_text BYTEA
) 
LANGUAGE plpgsql
AS $$
BEGIN
    v_encrypted_text := pgp_sym_encrypt(v_input, 'SuperSecureH@5h');
END;
$$;

DO $$
DECLARE
    v_input TEXT := 'Hello';
    v_encrypted_text BYTEA;
BEGIN

    CALL sp_encrypt_text(v_input, v_encrypted_text);
    
    RAISE NOTICE 'Encrypted Text: %', v_encrypted_text;
END;
$$;

------------------------------------------------------------------------------------------------------------------------

-- 2. Create a stored procedure to compare two encrypted texts
-- Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

CREATE OR REPLACE FUNCTION fn_compare_encrypted(
	v_input_1 BYTEA,
	v_input_2 BYTEA 
)
RETURNS BOOLEAN AS
$$
BEGIN
	IF pgp_sym_decrypt(v_input_1, 'SuperSecureH@5h') = pgp_sym_decrypt(v_input_2, 'SuperSecureH@5h') THEN
		RETURN TRUE;
	ELSE
		RETURN FALSE;
	END IF;
END;
$$
LANGUAGE plpgsql;

SELECT fn_compare_encrypted('\xc30d0407030271a8ef97ec93ff5478d23601e682b9db6b6b2cf3ef0aab786b00e0083f7330c95b265ee4b60a15e629ee3fde7720eec05e8003e2c1836cec48030a5b3c568de4e2'::bytea, '\xc30d0407030271a8ef97ec93ff5478d23601e682b9db6b6b2cf3ef0aab786b00e0083f7330c95b265ee4b60a15e629ee3fde7720eec05e8003e2c1836cec48030a5b3c568de4e2'::bytea);

CREATE OR REPLACE PROCEDURE sp_compare_encrypted(
    IN v_input_1 BYTEA,
    IN v_input_2 BYTEA,
    OUT result BOOLEAN
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF pgp_sym_decrypt(v_input_1, 'SuperSecureH@5h') = pgp_sym_decrypt(v_input_2, 'SuperSecureH@5h') THEN
        result := TRUE;
    ELSE
        result := FALSE;
    END IF;
END;
$$;


DO $$
DECLARE
    v_encrypted_text1 BYTEA;
    v_encrypted_text2 BYTEA;
    v_result BOOLEAN;
BEGIN
    CALL sp_encrypt_text('Hello', v_encrypted_text1);
    CALL sp_encrypt_text('Hello', v_encrypted_text2);
    
    CALL sp_compare_encrypted(v_encrypted_text1, v_encrypted_text2, v_result);
    
    IF v_result THEN
        RAISE NOTICE 'The encrypted texts match.';
    ELSE
        RAISE NOTICE 'The encrypted texts do not match.';
    END IF;
END;
$$;

-- 3. Create a stored procedure to partially mask a given text
-- Task: Write a procedure sp_mask_text that:
 
-- Shows only the first 2 and last 2 characters of the input string
 
-- Masks the rest with *
 
-- E.g., input: 'john.doe@example.com' → output: 'jo***************om'

CREATE OR REPLACE FUNCTION fn_mask_text(
    p_input_text TEXT         
)
RETURNS TEXT                 
LANGUAGE plpgsql
AS $$
BEGIN

    IF LENGTH(p_input_text) > 4 THEN
        RETURN SUBSTRING(p_input_text FROM 1 FOR 2) || 
               REPEAT('*', LENGTH(p_input_text) - 4) ||
               SUBSTRING(p_input_text FROM LENGTH(p_input_text) - 1 FOR 2);
    ELSE

        RETURN p_input_text;
    END IF;
END;
$$;

SELECT fn_mask_text('johndoe@gmail.com');

CREATE OR REPLACE PROCEDURE sp_mask_text(
    IN p_input_text TEXT,
    OUT p_masked_text TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF LENGTH(p_input_text) > 4 THEN
        p_masked_text := SUBSTRING(p_input_text FROM 1 FOR 2) || 
                         REPEAT('*', LENGTH(p_input_text) - 4) ||
                         SUBSTRING(p_input_text FROM LENGTH(p_input_text) - 1 FOR 2);
    ELSE
        p_masked_text := p_input_text;
    END IF;
END;
$$;


DO $$
DECLARE
    v_input_text TEXT := 'johndoe@gmail.com';
    v_masked_text TEXT;
BEGIN
    CALL sp_mask_text(v_input_text, v_masked_text);
    
    RAISE NOTICE 'Masked Text: %', v_masked_text;
END;
$$;

-- 4. Create a procedure to insert into customer with encrypted email and masked name
-- Task:
 
-- Call sp_encrypt_text for email
 
-- Call sp_mask_text for first_name
 
-- Insert masked and encrypted values into the customer table
 
-- Use any valid address_id and store_id to satisfy FK constraints.

CREATE TABLE Customers (
	customer_id SERIAL PRIMARY KEY,
	first_name TEXT,
	last_name TEXT,
	email TEXT
);

CREATE OR REPLACE PROCEDURE sp_insert_customer(
    IN p_first_name TEXT,  
    IN p_last_name TEXT,      
    IN p_email TEXT           
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_encrypted_email TEXT;
    v_masked_first_name TEXT;
BEGIN

    v_encrypted_email := fn_encrypt_text(p_email);
    v_masked_first_name := fn_mask_text(p_first_name);

    INSERT INTO Customers (first_name, last_name, email)
    VALUES (v_masked_first_name, p_last_name, v_encrypted_email);
END;
$$;

CALL sp_insert_customer('Johnny', 'Doe', 'john.doe@example.com');

SELECT * FROM Customers;

-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
-- Task:
-- Write sp_read_customer_masked() that:
 
-- Loops through all rows
 
-- Decrypts email
 
-- Displays customer_id, masked first name, and decrypted email


CREATE OR REPLACE PROCEDURE sp_read_customer_masked()
LANGUAGE plpgsql
AS $$
DECLARE
    v_customer RECORD;      
    v_decrypted_email TEXT;   
	customer_cursor CURSOR FOR SELECT * FROM Customers;
BEGIN
OPEN customer_cursor;
    
    LOOP
        FETCH customer_cursor INTO v_customer;
        EXIT WHEN NOT FOUND;
        
        v_decrypted_email := pgp_sym_decrypt(v_customer.email::bytea, 'SuperSecureH@5h');
        
        RAISE NOTICE 'Customer ID: %, Masked First Name: %, Decrypted Email: %', 
                     v_customer.customer_id, v_customer.first_name, v_decrypted_email;
    END LOOP;
    
    CLOSE customer_cursor;
END;
$$;

CALL sp_read_customer_masked();
