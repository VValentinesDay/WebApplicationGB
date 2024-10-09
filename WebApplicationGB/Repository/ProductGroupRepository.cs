using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplicationGB.Abstractions;
using WebApplicationGB.Data;
using WebApplicationGB.DTO;
using WebApplicationGB.Models;

namespace WebApplicationGB.Repository
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductGroupRepository(Context context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public int AddProductGroup(ProductGroupDTO productGroupDTO)
        {
                // 0 и -1 не могут быть ID
                if (_context.Products.Any(p => p.Name == productGroupDTO.Name)) 
                throw new Exception("Группа продуктов с таким именем уже существует");
                // сообщение потом обрабатывается в контроллере
                var entity = _mapper.Map<ProductGroup>(productGroupDTO);
                _context.ProductGroup.Add(entity);
                _context.SaveChanges(); // после создания объекта приянто возвращать ID. SaveChanges даёт доступ к ID
                return entity.Id;
            
        }

        public IEnumerable<ProductGroupDTO> GetProductsGroup()
        {
            if (_cache.TryGetValue("productGroup", out List<ProductGroupDTO> cacheList)) return cacheList;

            var listDTO = _context.ProductGroup.Select(_mapper.Map<ProductGroupDTO>);
            _cache.Set("productGroup", listDTO, TimeSpan.FromMinutes(30));
            return listDTO.ToList();
        }

        public string DeleteProductGroup(string name)
        {

            if (!_context.ProductGroup.Any(p => p.Name == name)) 
                throw new Exception("Такой группы продуктов не существует");

            _context.ProductGroup.Where(p => p.Name == name).ExecuteDelete();
            _cache.Remove("productGroup");
            return "Группа продуктов '" + name + "' - удалена";
        }








    }
}
