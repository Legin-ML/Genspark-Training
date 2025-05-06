-- Print all title names

SELECT title from titles;

--  Print all the titles that have been published by 1389

SELECT title from titles where pub_id = 1389;

-- Print the books that have price in range of 10 to 15

SELECT title from titles where price >= 10 AND price <= 15;

-- Print those books that have no price

SELECT title from titles where price is NULL or price = 0;

-- Print the book names that starts with 'The'

SELECT title from titles where title LIKE 'The%';

-- Print the book names that do not have 'v' in their name

SELECT title from titles where title NOT LIKE '%v%';

-- print the books sorted by the royalty

SELECT title from titles ORDER BY royalty ASC;

-- print the books sorted by publisher in descending then by types in ascending then by price in descending

SELECT t.title from titles as t JOIN publishers as p ON t.pub_id = p.pub_id ORDER BY p.pub_name DESC, type ASC,price DESC;

-- Print the average price of books in every type

SELECT AVG(price), type FROM titles GROUP BY type;

-- print all the types in unique

SELECT DISTINCT type from titles;

-- Print the first 2 costliest books

SELECT TOP 2 title from titles ORDER BY price DESC;

-- Print books that are of type business and have price less than 20 which also have advance greater than 7000

SELECT title from titles WHERE type='business' and price < 20 and advance > 7000;

-- Select those publisher id and number of books which have price between 15 to 25 and have 'It' in its name. Print only those which have count greater than 2. Also sort the result in ascending order of count

SELECT t.pub_id, COUNT(t.title) as book_count from titles as t where t.price between 15 and 25 and t.title LIKE '%It%' GROUP BY t.pub_id HAVING COUNT(t.title) > 2 ORDER BY book_count ASC;

-- Print the Authors who are from 'CA'

-- SELECT CONCAT(a.au_fname, ' ', a.au_lname) AS full_name
-- FROM pubs.dbo.authors a
-- WHERE a.state = 'CA';

SELECT DISTINCT t.title, a.state from titles as t JOIN titleauthor as ta ON t.title_id = ta.title_id JOIN  authors as a  ON ta.au_id = a.au_id where a.state = 'CA';

-- Print the count of authors from each state

SELECT state, COUNT(au_id) from authors GROUP BY state;