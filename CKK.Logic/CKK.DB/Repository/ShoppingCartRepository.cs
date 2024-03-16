using CKK.DB.Interfaces;
using CKK.Logic.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKK.DB.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public ShoppingCartRepository(IConnectionFactory Conn)
        {
            _connectionFactory = Conn;
        }
        public int Add(ShoppingCartItem entity)
        {
            var sql = "Insert into ShoppingCartItems (ShoppingCartId,ProductId,Quantity) VALUES (@ShoppingCartId,@ProductId, @Quantity)";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                int result = connection.Execute(sql, entity);
                return result;
            }
        }

        public ShoppingCartItem AddToCart(int ShoppingCartId, int ProductId, int quantity)
        {
            using (var conn = _connectionFactory.GetConnection)
            {
                ProductRepository _productRepository = new ProductRepository(_connectionFactory);
                var item = _productRepository.GetById(ProductId);
                var ProductItems = GetProducts(ShoppingCartId).Find(x => x.ProductId == ProductId);

                var shopitem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    ProductId = ProductId,
                    Quantity = quantity
                };

                if (item.Quantity >= quantity)
                {
                    if (ProductItems != null)
                    {
                        //Product already in cart so update quantity
                        var test = Update(shopitem);
                    }
                    else
                    {
                        //New product for the cart so add it
                        var test = Add(shopitem);
                    }
                }
                return shopitem;
            }
        }

        public int ClearCart(int shoppingCartId)
        {
			var sql = "DELETE FROM ShoppingCartItems WHERE ShoppingCartId = @ShoppingCartId";
			using (var connection = _connectionFactory.GetConnection)
			{
				connection.Open();

				var affected = connection.Execute(sql, new { ShoppingCartId = shoppingCartId });

                return affected;
			}
		}

        public List<ShoppingCartItem> GetProducts(int ShoppingCartId)
        {
            var sql = "SELECT * FROM ShoppingCartItems";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                List<ShoppingCartItem> results = new List<ShoppingCartItem>();

                results = connection.Query<ShoppingCartItem>(sql).ToList();

                return results;
            }
        }

        public decimal GetTotal(int shoppingCartId)
        {
            var sql = "SELECT SUM(p.Price * s.Quantity) FROM ShoppingCartItems s JOIN Products p ON s.ProductId = p.Id WHERE s.ShoppingCartId = @shoppingCartId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();

                decimal? total = connection.QueryFirstOrDefault<decimal?>(sql, new { ShoppingCartId = shoppingCartId });

                return total ?? 0;
            }
        }

        public void Ordered(int shoppingCartId)
        {
            var sql = "DELETE FROM ShoppingCartItems WHERE ShoppingCartId = @shoppingCartId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();

                var affectedRows = connection.Execute(sql, new { ShoppingCartId = shoppingCartId });
            }
        }

        public int Update(ShoppingCartItem entity)
        {
            var sql = "UPDATE ShoppingCartItems SET ShoppingCartId = @ShoppingCartId, ProductId = @ProductId, Quantity = @Quantity WHERE ShoppingCartId = @ShoppingCartId AND ProductId = @ProductId";
            using (var connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }
    }
}