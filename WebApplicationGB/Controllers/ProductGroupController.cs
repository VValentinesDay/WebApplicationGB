using Microsoft.AspNetCore.Mvc;
using WebApplicationGB.Abstractions;
using WebApplicationGB.DTO;

namespace WebApplicationGB.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupRepository _productGroupRepository;
        public ProductGroupController(IProductGroupRepository productGroupRepository) 
        {
            _productGroupRepository = productGroupRepository;
        }


        [HttpGet("getProductGroup")]
        public ActionResult<IEnumerable<ProductGroupDTO>> GetProductGroup() 
        { 
            return Ok( _productGroupRepository.GetProductsGroup().ToList());
        }

        [HttpPost("AddGroup")]
        public ActionResult AddProductGroup(ProductGroupDTO productGroup) 
        { 
            _productGroupRepository.AddProductGroup(productGroup);
            return Ok(productGroup.Name);
        }

        [HttpDelete("DeleteGroup")]
        public string DeleteProductGroup(string name)
        {
            return _productGroupRepository.DeleteProductGroup(name);
        }
    }
}
