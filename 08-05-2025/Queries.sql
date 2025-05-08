  select * from products where 
  try_cast(json_value(details,'$.spec.cpu') as nvarchar(20)) ='i7'

  create proc proc_FilterProducts(@pcpu varchar(20), @pcount int out)
  as
  begin
      set @pcount = (select count(*) from products where 
	  try_cast(json_value(details,'$.spec.cpu') as nvarchar(20)) =@pcpu)
  end

 begin
  declare @cnt int
 exec proc_FilterProducts 'i7', @cnt out
  print concat('The number of computers is ',@cnt)
  end
