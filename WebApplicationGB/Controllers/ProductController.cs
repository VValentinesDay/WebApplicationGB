using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationGB.Data;
using WebApplicationGB.Models;

namespace WebApplicationGB.Controllers
{
    [ApiController]        // этот контроллер не будет возвращать view
    [Route("[Controller]")] // Переброска на этот контроллер будет происходить по имени контроллера
    public class ProductController : ControllerBase
    {


        // Добавление продуктов в БД

        // контроллеры возвращают либо ActionResult либоо IActionResult
        [HttpPost]
        public ActionResult<int> AddProduct(string name, string discr, int price)
        {
            // здесь using используется чтобы закрывать соединение
            using (Context context = new Context()) 
            {
                if (context.Products.Any(p => p.Name == name)) return StatusCode(409);

                var product = new Product { Name = name, Description = discr, Price = price };
                context.Products.Add(product);
                context.SaveChanges(); // после создания объекта приянто возвращать ID. SaveChanges даёт доступ к ID
                return Ok(product.Id);
            }
        }
        // Получение всех продуктов
        [HttpGet("get_product")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
           IEnumerable<Product> products;
            // здесь using используется чтобы закрывать соединение
            using (Context context = new Context())
            {
                products = context.Products.Select(p=>new Product {Name = p.Name, Description =  p.Description, Price = p.Price}).ToList();
                // пока не выбрано приведение в какому-то типу как выше ToList будет существовать только запрос. Если смотреть по тексту,
                // то IEnumerable нельзя вернуть в качестве объекта(почему так??)

                // метод неплохо сделать асинхронным чтобы не процесс е ждал выгрузки
                return Ok(products);

            }

        }
        [HttpDelete("delete_product")]
        public ActionResult<string> DeleteProduct(string name) 
        {
            using (Context context = new Context()) 
            {
                if (context.Products.Any(p => p.Name == name)) 
                {
                    context.Products.Where(p => p.Name == name).ExecuteDelete();

                    return Ok(name + " - продукт удалён");
                   }

                else return "Продукт не найден";
                
            }
        }

        [HttpPatch("change_price")]
        public ActionResult<string> ChangePrice(string name, int price) 
        {
            using (Context context = new Context()) 
            {
                if (context.Products.Any(p => p.Name == name))
                {
                    context.Products.Where(p => p.Name == name).ExecuteUpdate(p => p.SetProperty(p => p.Price, p => price));
                    return Ok($"Цена продукта {name} теперь составляет: {price}");
                }
                else {
                    return StatusCode(404);
                }
            }
        }
    }
}
