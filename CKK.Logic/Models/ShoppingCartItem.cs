using CKK.Logic.Exceptions;

namespace CKK.Logic.Models
{
    [Serializable]
    public class ShoppingCartItem
    {
        public Product Product { get; set; }
        public int ShoppingCartId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        private int quantity { get; set; }
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (value >= 0)
                {
                    quantity = value;
                }
                else
                {
                    try
                    {
                        value = 0;
                    }
                    catch
                    {
                        throw new InventoryItemStockTooLowException();
                    }
                }
            }
        }
        public decimal GetTotal()
        {
            return Product.Price * Quantity;
        }
    }
}
