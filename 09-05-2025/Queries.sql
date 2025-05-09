-- SELECT Queries
-- List all films with their length and rental rate, sorted by length descending.
-- Columns: title, length, rental_rate

select title, length, rental_rate from film ORDER BY length DESC 

-- Find the top 5 customers who have rented the most films.
-- Hint: Use the rental and customer tables.

select cus.customer_id, cus.first_name || ' ' || cus.last_name as CustomerName, COUNT(ren.rental_id) as Count
from customer as cus join rental as ren on ren.customer_id = cus.customer_id
GROUP BY cus.customer_id
ORDER BY Count DESC
LIMIT 5

-- Display all films that have never been rented.
-- Hint: Use LEFT JOIN between film and inventory → rental.

SELECT DISTINCT f.title FROM film as f left outer join inventory as i on i.film_id = f.film_id
left outer join rental as r on r.inventory_id = i.inventory_id
where r.rental_id is NULL

-- JOIN Queries
-- List all actors who appeared in the film ‘Academy Dinosaur’.
-- Tables: film, film_actor, actor

SELECT a.first_name || ' ' || a.last_name as "Actor Names" from film as f join film_actor fa on f.film_id = fa.film_id
join actor as a on a.actor_id = fa.actor_id WHERE f.title = 'Academy Dinosaur'

-- List each customer along with the total number of rentals they made and the total amount paid.
-- Tables: customer, rental, payment

select cus.customer_id, cus.first_name || ' ' || cus.last_name as CustomerName, SUM(pay.amount) as Count
from customer as cus join rental as ren on ren.customer_id = cus.customer_id
join payment as pay on pay.rental_id = ren.rental_id
GROUP BY cus.customer_id


-- CTE-Based Queries
-- Using a CTE, show the top 3 rented movies by number of rentals.
-- Columns: title, rental_count

WITH count_movies AS (
SELECT f.title, COUNT(r.rental_id) as rental_count FROM film as f JOIN inventory as i on f.film_id = i.film_id JOIN
rental as r on i.inventory_id = r.inventory_id GROUP BY f.title 
)

SELECT * FROM count_movies ORDER BY rental_count DESC LIMIT 3

-- Find customers who have rented more than the average number of films.
-- Use a CTE to compute the average rentals per customer, then filter.

WITH AverageRentals AS (
	SELECT r.customer_id, COUNT(*) as total_rentals
	FROM rental as r GROUP BY r.customer_id
),
Average AS (
	SELECT CAST(AVG(total_rentals) AS INT) AS average_rentals
	FROM AverageRentals
)

SELECT ar.customer_id, ar.total_rentals AS "Total Rented"
FROM AverageRentals as ar, Average as a
WHERE ar.total_rentals > a.average_rentals


--  Function Questions
-- Write a function that returns the total number of rentals for a given customer ID.
-- Function: get_total_rentals(customer_id INT)
DROP FUNCTION IF EXISTS get_total_rentals
CREATE FUNCTION get_total_rentals(customer_id_param INT)
RETURNS INTEGER AS $$
BEGIN
	RETURN COUNT(r.rental_id) FROM rental as r WHERE r.customer_id = customer_id_param;
END;
$$ LANGUAGE plpgsql

SELECT get_total_rentals(2)

-- Stored Procedure Questions
-- Write a stored procedure that updates the rental rate of a film by film ID and new rate.
-- Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)

DROP FUNCTION IF EXISTS update_rental_rate
CREATE FUNCTION update_rental_rate (param_film_id INT, param_new_rate NUMERIC)
RETURNS VOID AS $$
BEGIN
	UPDATE film SET rental_rate = param_new_rate WHERE film_id = param_film_id;
END;
$$ LANGUAGE plpgsql

SELECT update_rental_rate(12, 99)

-- Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
-- Procedure: get_overdue_rentals() that selects relevant columns.

DROP FUNCTION IF EXISTS get_overdue_rentals;
CREATE FUNCTION get_overdue_rentals()
RETURNS TABLE (customer_id smallint, rental_date timestamp without time zone) AS $$
BEGIN
	RETURN QUERY
	SELECT r.customer_id, r.rental_date FROM rental as r
	WHERE r.return_date IS NULL AND r.rental_date < CURRENT_DATE - INTERVAL '7 days';
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_overdue_rentals()