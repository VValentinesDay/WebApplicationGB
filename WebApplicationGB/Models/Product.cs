namespace WebApplicationGB.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price {  get; set; }
        public string Description { get; set; }
        // FK для БД
        public int? ProductGroupID { get; set; }
        public virtual List<Storage> Storages { get; set; }
        // виртуальные свойства
        // у группы м.б. много продуктов, у одного продукта м.б. только одна группа
       
        public virtual ProductGroup? ProductGroup { get; set; }
        // такое свойство с типом другого класса из папки моделей, формируется заранее
        // для создания в будующем связи типа один ко многим
    }
}
