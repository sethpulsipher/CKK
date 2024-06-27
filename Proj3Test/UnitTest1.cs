using CKK.DB.Interfaces;
using CKK.DB.UOW;
using CKK.Logic.Models;

namespace CKK.Logic
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IConnectionFactory data = new DatabaseConnectionFactory();
            UnitOfWork services = new UnitOfWork(data);

            Product book = new Product { Id = 1, Name = "Bible", Price = 10, Quantity = 100 } ;
            using(var connection = data.GetConnection)
            {
                var sql = "Insert into Products (Id, Name, Price, Quantity) values (@Id, @Name, @Price, @Quantity)";
                connection.Open();
                
            }
        }
    }
}