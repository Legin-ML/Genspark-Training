--print the publisher deatils of the publisher who has never published

SELECT * FROM publishers where pub_id not in (SELECT DISTINCT pub_id FROM titles)

-- Select author_id for all the books, Print the author_id and book name

SELECT title, au_id from titles join titleauthor on titles.title_id = titleauthor.title_id

-- Print the publisher's name, book name and the order date of the books

SELECT t.title as Book, p.pub_name as Publisher, s.ord_date as OrdDate from publishers as p join  titles as t on t.pub_id = p.pub_id join sales as s
on s.title_id = t.title_id

-- Print the publisher's name and the first book sale date of all publishers

SELECT p.pub_name, MIN(s.ord_date) AS First_Book_Sale_Date
FROM publishers p
left outer JOIN titles t ON p.pub_id = t.pub_id
left outer JOIN sales s ON t.title_id = s.title_id
GROUP BY p.pub_name
ORDER BY 2 DESC

-- print the bookname and the store address of the sale

SELECT title, stor_address from titles left outer join sales on sales.title_id = titles.title_id left outer join stores on stores.stor_id = sales.stor_id

----------------------------------------------------

create procedure proc_HelloWorld
as
begin
PRINT 'Hello World!'
end
go
exec proc_HelloWorld

create table Products
(id int identity(1,1) constraint pk_productId primary key,
name nvarchar(100) not null,
details nvarchar(max))
Go
create or alter proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end
go
proc_InsertProduct 'Laptop','{"brand":"ASUS","spec":{"ram":"16GB","cpu":"Ryzen 9"}}'

select * from products

select JSON_QUERY(details, '$.spec') Product_Specification from products

create proc proc_UpdateProductSpec(@pid int,@newvalue varchar(20))
as
begin
   update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @pid
end
go
proc_UpdateProductSpec 1, '32GB'

select id, name, JSON_VALUE(details, '$.brand') Brand_Name
from Products

  create table Posts
  (id int primary key,
  title nvarchar(100),
  user_id int,
  body nvarchar(max))
Go

  select * from Posts
  go
  create proc proc_BulInsertPosts(@jsondata nvarchar(max))
  as
  begin
		insert into Posts(user_id,id,title,body)
	  select userId,id,title,body from openjson(@jsondata)
	  with (userId int,id int, title nvarchar(100), body varchar(max))
  end
  go
  delete from Posts

  proc_BulInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }]'

  create or alter proc proc_GetPosts(@id int)
  as
  begin
  select title, body from Posts where user_id = @id
  end
  go

  exec proc_GetPosts 1