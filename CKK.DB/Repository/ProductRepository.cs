using CKK.DB.Interfaces;
using CKK.DB.UOW;
using CKK.Logic.Models;
using Dapper;

namespace CKK.DB.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public ProductRepository(IConnectionFactory Conn)
        {
            _connectionFactory = Conn;
            _connectionFactory = new DatabaseConnectionFactory();
        }
        public int Add(Product entity)
        {
            var sql = "Insert into Products (Price,Quantity,Name) VALUES (@Price,@Quantity,@Name)";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public int Delete(int id)
        {
            var sql = "UPDATE Products SET Name = NULL, Price = 0, Quantity = 0 WHERE Id = @Id";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();

                int affectedRows = connection.Execute(sql, new { Id = id });

                return affectedRows;
            }
        }

        public List<Product> GetAll()
        {
            var sql = "SELECT * FROM Products";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                List<Product> results = new List<Product>();

                results = connection.Query<Product>(sql).ToList();

                return results;
            }
        }
        public async Task<List<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                List<Product> results = await Task.Run(() => connection.Query<Product>(sql).ToList());
                
                return results;
            }
        }

        public Product GetById(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Product>(sql, new { Id = id });
                return result;
            }
        }

        public List<Product> GetByName(string name)
        {
            var sql = "SELECT * FROM Products WHERE Name = @Name";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                List<Product> results = new List<Product>();
                results = connection.Query<Product>(sql, new { Name = name }).ToList();
                return results;
            }
        }

        public int Update(Product entity)
        {
            var sql = "UPDATE Products SET Price = @Price, Quantity = @Quantity, Name = @Name WHERE Id = @Id";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                int affectedRows = connection.Execute(sql, entity);
                return affectedRows;
            }
        }
    }
}
