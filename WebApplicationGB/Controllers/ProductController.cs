using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using WebApplicationGB.Abstractions;
using WebApplicationGB.Data;
using WebApplicationGB.DTO;
using WebApplicationGB.Models;
using WebApplicationGB.Repository;

namespace WebApplicationGB.Controllers
{
    [ApiController]        // этот контроллер не будет возвращать view
    [Route("[Controller]")] // Переброска на этот контроллер будет происходить по имени контроллера
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpPost]
        public ActionResult<int> AddProduct(ProductDTO productDTO)
        {
            try
            {
                var product = _productRepository.AddProduct(productDTO);
                return Ok(product);
            }
            catch (Exception exception)
            {
                return StatusCode(409);
            }
        }

        // Получение всех продуктов
        [HttpGet("get_product")]
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            return Ok(_productRepository.GetProducts());
        }


        [HttpDelete("delete_product")]
        public ActionResult<string> DeleteProduct(string name)
        {
            return _productRepository.DeleteProduct(name);
        }

        [HttpPatch("change_price")]
        public ActionResult<string> ChangePrice(string name, decimal price)
        {
            using (Context context = new Context())
            {

                _productRepository.ChangePrice(name, price);
                return Ok($"Цена продукта {name} теперь составляет: {price}");
     
            }
        }


        [HttpGet(template: "GetCsvProducts")]
        public FileContentResult GetCsvProducts()
        {
            var productList = _productRepository.GetProducts().ToList();
            StringBuilder listForCvs = new StringBuilder();

            foreach (var product in productList)
            {
                listForCvs.AppendLine("ID: " + product.Id + ", Наименование: " + product.Name + ", Цена: " +
                product.Price + ", Описание: " + product.Description + "\n");
            }
            return File(new System.Text.UTF8Encoding().GetBytes(listForCvs.ToString()), "text/csv", "report.csv");
        }
    }
}
