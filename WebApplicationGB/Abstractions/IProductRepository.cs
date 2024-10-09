using Microsoft.AspNetCore.Mvc;
using WebApplicationGB.DTO;
using WebApplicationGB.Models;

namespace WebApplicationGB.Abstractions
{
    public interface IProductRepository
    {
        IEnumerable<ProductDTO> GetProducts();
        int AddProduct(ProductDTO productDTO);
        string DeleteProduct(string name);
        string ChangePrice(string name, decimal price);
    } 
}
