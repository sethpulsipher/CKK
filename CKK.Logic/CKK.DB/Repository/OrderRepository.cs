using CKK.DB.Interfaces;
using CKK.DB.UOW;
using CKK.Logic.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKK.DB.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public OrderRepository(IConnectionFactory Conn)
        {
            _connectionFactory = Conn;
            _connectionFactory = new DatabaseConnectionFactory();
        }
        public int Add(Order entity)
        {
            var sql = "Insert into Orders (OrderId,OrderNumber,CustomerId,ShoppingCartId) VALUES (@OrderId,@OrderNumber,@CustomerId,@ShoppingCartId)";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                int result = connection.Execute(sql, entity);
                return result;
            }
        }

        public int Delete(int orderId)
        {
            var sql = "DELETE FROM Orders WHERE OrderId = @orderId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();

                int affectedRows = connection.Execute(sql, new { OrderId = orderId });

                return affectedRows;
            }
        }

        public List<Order> GetAll()
        {
            var sql = "SELECT * FROM Orders";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                List<Order> results = new List<Order>();

                results = connection.Query<Order>(sql).ToList();

                return results;
            }
        }

        public Order GetById(int orderId)
        {
            var sql = "SELECT * FROM Orders WHERE OrderId = @orderId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Order>(sql, new { OrderId = orderId });
                return result;
            }
        }

        public Order GetOrderByCustomerId(int customerId)
        {
            var sql = "SELECT * FROM Orders WHERE CustomerId = @customerId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Order>(sql, new { CustomerId = customerId });
                return result;
            }
        }

        public int Update(Order entity)
        {
            var sql = "UPDATE Orders SET OrderId = @OrderId, OrderNumber = @OrderNumber, CustomerId= @CustomerId, ShoppingCartId = @ShoppingCartId WHERE ShoppingCartId = @ShoppingCartId AND CustomerId = @CustomerId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                int affectedRows = connection.Execute(sql, entity);
                return affectedRows;
            }
        }
    }
}
