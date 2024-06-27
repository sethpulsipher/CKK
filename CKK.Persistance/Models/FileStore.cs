using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKK.Logic.Interfaces;
using CKK.Persistance.Interfaces;
using CKK.Logic.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;
using CKK.Logic.Exceptions;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CKK.Persistance.Models
{
    public class FileStore : IStore, ISavable, ILoadable
    {
        private FileStream? path;
        public List<StoreItem> _items = new List<StoreItem>();
        public List<StoreItem> loadedItems = new List<StoreItem>();
        public readonly string filePath = @"C:\Users\Owner\Documents\Persistance\StoreItem.json";
        private int IdCounter;

        public FileStore()
        {
            CreatePath(); 
        }
        private void CreatePath()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "Persistance");
        }
        public List<StoreItem> GetAllProductsByName(string name)
        {
            string Name = name;

            var FindByName =
                from i in loadedItems
                where Name == i.Product.Name
                select i;

            var list = new List<StoreItem>();

            foreach (var item in FindByName)
            {
                var iitem = new StoreItem(item.Product, item.Quantity);
                list.Add(iitem);
            }
            return list;

        }
        public List<StoreItem> GetProductsByQuantity()
        {
            List<StoreItem> listedOrder = _items.OrderBy(i => i.Quantity).ToList();
            return listedOrder;
        }
        public List<StoreItem> GetProductByPrice()
        {
            List<StoreItem> listedPrice = _items.OrderBy(i => i.Product.Price).ToList();
            return listedPrice;
        }
        public void Load()
        {
            using(path = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {   
                loadedItems.Clear();
                using StreamReader sr = new StreamReader(path);
                var jsonText = sr.ReadToEnd();
                StoreItem[] items = JsonConvert.DeserializeObject<StoreItem[]>(jsonText);
                loadedItems = items.ToList();
                Array.Clear(items);
            }
        }
        public void Save()
        {
            //using (path = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            //{
                foreach(var item in _items)
                {
                    var jsonString = JsonConvert.SerializeObject(item);
                    File.WriteAllText(filePath, jsonString);
                }
                _items.Clear();
            //}
        }
        public StoreItem AddStoreItem(Product prod, int quantity)
        {
            if (quantity > 0)
            {
                int num = _items.Count;
                if (num > 0)
                {
                    var CheckItems =
                        from i in _items
                        let me = new StoreItem(prod, quantity)
                        where i.Product == me.Product
                        select i;
                    if (CheckItems.Any())
                    {
                        foreach (var item in CheckItems)
                        {
                            item.Quantity += quantity;
                            return item;
                        }
                    }
                    else if (!CheckItems.Any())
                    {
                        if (num > 0 && prod.Id == 0)
                        {
                            Random rnd = new Random();
                            int rndNum = rnd.Next(1, 999);
                            prod.Id = rndNum;
                            _items.Add(new StoreItem(prod, quantity));
                            return _items[num];
                        }
                        else
                        {
                            _items.Add(new StoreItem(prod, quantity));
                            return _items[num];
                        }
                    }
                }
                else if (prod.Id == 0)
                {
                    Random rnd = new Random();
                    int rndNum = rnd.Next(1, 999);
                    prod.Id = rndNum;
                    _items.Add(new StoreItem(prod, quantity));
                    return _items[0];
                }
                else
                    _items.Add(new StoreItem(prod, quantity));
                    return _items[0];
            }
            else
                throw new InventoryItemStockTooLowException();
        }
        public StoreItem RemoveStoreItem(int id, int quantity)
        {
            if (quantity > 0)
            {
                if (_items!.Contains(FindStoreItemById(id)))
                {
                    int num = FindStoreItemById(id).Quantity - quantity;
                    if (num > 0)
                    {
                        FindStoreItemById(id).Quantity = num;
                        return FindStoreItemById(id);
                    }
                    else
                    {
                        FindStoreItemById(id).Quantity = 0;
                        return FindStoreItemById(id);
                    }
                }
                else
                    throw new ProductDoesNotExistException();
            }
            else
                throw new ArgumentOutOfRangeException();
        }
        public void DeleteStoreItem(int id)
        {
            _items!.Remove(FindStoreItemById(id));
        }

        public List<StoreItem> GetStoreItems()
        {
            return _items!;
        }

        public StoreItem FindStoreItemById(int id)
        {
            if (id > 0)
            {
                var FindByID =
                     from i in _items!
                     where i.Product.Id == id
                     select i;

                if (FindByID.Any())
                {
                    return FindByID.First();
                }
                else if (!FindByID.Any())
                {
                    return null!;
                }
                else
                    return null!;
            }
            else
                throw new InvalidIdException();
        }
    }
}
