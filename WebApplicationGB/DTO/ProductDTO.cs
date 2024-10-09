namespace WebApplicationGB.DTO
{
    public class ProductDTO
    {
        //DTO(data transport object) - объект который обеспечивает коммникацию front-end с back-end
        // указанные поля будут выдаваться пользователю

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }




    }
}
