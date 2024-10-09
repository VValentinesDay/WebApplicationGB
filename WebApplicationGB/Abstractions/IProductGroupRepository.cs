using WebApplicationGB.DTO;
using WebApplicationGB.Models;

namespace WebApplicationGB.Abstractions
{
    public interface IProductGroupRepository
    {
        IEnumerable<ProductGroupDTO> GetProductsGroup();
        int AddProductGroup(ProductGroupDTO productGroupDTO);
        string DeleteProductGroup(string name);
    }
}
