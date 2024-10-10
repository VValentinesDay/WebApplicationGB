using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using WebApplicationGB.Abstractions;
using WebApplicationGB.Data;
using WebApplicationGB.DTO;
using WebApplicationGB.Models;

namespace WebApplicationGB.Repository
{
    public class ProductRepository : IProductRepository
    // работает с базой данных, теперь работа с БД осуществляется
    // через репозиторий
    {
        Context context = new();
        private readonly IMapper _mapper; // _ - readonle private
        private readonly IMemoryCache _cache;


        // контекст передаётся через конструктор.
        // Т.о. контекст будет использоваться лишь по необходимости
        public ProductRepository(Context context, IMapper mapper, IMemoryCache _cache)
        {
            this.context = context;
            this._mapper = mapper;
            this._cache = _cache;
            // контекст будет удаляться из процессов, если его подключить
            // к сревисам, поэтому можно будет обойтись без using
        }

        public int AddProduct(ProductDTO productDTO)
        {
            // здесь using используется чтобы закрывать соединение
            {
                // 0 и -1 не могут быть ID
                if (context.Products.Any(p => p.Name == productDTO.Name)) throw new Exception("Продукт с таким именем уже существует");
                // сообщение потом обрабатывается в контроллере
                var entity = _mapper.Map<Product>(productDTO);
                context.Products.Add(entity);
                _cache.Remove("products"); // удаление по ключу
                context.SaveChanges(); // после создания объекта приянто возвращать ID. SaveChanges даёт доступ к ID
                return entity.Id;
            }
        }


        public IEnumerable<ProductDTO> GetProducts()
        {
            // попытка получить данные из кэша
            if (_cache.TryGetValue("products", out List<ProductDTO> listDTOcache)) return listDTOcache;

            // сама реализация
            var listDTO = context.Products.Select(_mapper.Map<ProductDTO>).ToList();
            _cache.Set("products", listDTO, TimeSpan.FromMinutes(30)); // set : ключ - значение - время

            return listDTO;
        }

        public string DeleteProduct(string name)
        {


            if (Int32.TryParse(name, out int id))
            {
                if (!context.Products.Any(p => p.Id == id))
                    throw new Exception("Такого продукта не существует");
                context.Products.Remove(context.Products.Find(id));
                context.SaveChanges();

                _cache.Remove("products");
                return "Продукт c ID '" + name + "' - удалён";
            }
            else
            {
                if (!context.Products.Any(p => p.Name == name))
                    throw new Exception("Такого продукта не существует");
                context.Products.Where(p => p.Name == name).ExecuteDelete();

                _cache.Remove("products");
                return "Продукт c наименованием '" + name + "' - удалён";
            }



        }

        public string ChangePrice(string name, decimal price)
        {
            if (!context.Products.Any(p => p.Name == name))
                throw new Exception("Такого продукта не существует");
            // вряд ли есть необходимсоть в аutomapper, т.к. пользователь меняет данные в БД и ничего не получает
            context.Products.Where(p => p.Name == name).ExecuteUpdate(p => p.SetProperty(p => p.Price, p => price));
            _cache.Remove("products");
            return ($"Цена продукта {name} теперь составляет: {price}");
        }

        public string GetCacheStat()
        {
            var content = _cache.GetCurrentStatistics();

            string result = "TotalHits: " + content.TotalHits.ToString() +
                            ", TotalMisses: " + content.TotalMisses.ToString() + 
                            ", CurrentEntryCount: " + content.CurrentEntryCount .ToString();
            return result;
        }
    }
}
