using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Models.DTOs;

namespace ShopMigrationAPI.Services
{
    public class OrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRespository;

        public OrderService(IRepository<Order> orderRepository, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRespository = productRepository;       
        }
        
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        public Order GetOrderById(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {id} not found.");
            }
            return order;
        }

        public IEnumerable<Order> GetOrdersPaged(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be greater than zero.");
            
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero.");

            var allOrders = _orderRepository.GetAll().ToList();


            return allOrders
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize);                 
        }
        
        public void CreateOrder(Order order)
        {
            _orderRepository.Add(order);
            _orderRepository.Save();
        }
        
        public void UpdateOrder(Order order)
        {
            var existingOrder = _orderRepository.GetById(order.Orderid);
            if (existingOrder == null)
            {
                throw new InvalidOperationException($"Order with ID {order.Orderid} not found.");
            }

            _orderRepository.Update(order);
            _orderRepository.Save();
        }
        
        public void DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {id} not found.");
            }

            _orderRepository.Delete(id);
            _orderRepository.Save();
        }
        
        public void ProcessOrder(OrderRequestDTO dto)
        {
            var order = new Order
            {
                Customername = dto.CustomerName,
                Customerphone = dto.CustomerPhone,
                Customeremail = dto.CustomerEmail,
                Customeraddress = dto.CustomerAddress,
                Orderdate = DateOnly.FromDateTime(DateTime.Now),
                Paymenttype = "Cash",
                Status = "Processing"
            };

            _orderRepository.Add(order);
            _orderRepository.Save();

            foreach (var item in dto.Items)
            {
                var product = _productRespository.GetById(item.Product.Productid);
                if (product == null)
                    throw new Exception($"Product with ID {item.Product.Productid} not found.");

                var orderDetail = new Orderdetail
                {
                    Orderid = order.Orderid,
                    Productid = product.Productid,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                /*_orderDetailRepo.Add(orderDetail);
                _orderDetailRepo.Save();*/
            }
        }
    }
}
