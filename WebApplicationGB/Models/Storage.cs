namespace WebApplicationGB.Models
{
    public class Storage
    {
        // свзяь склада с продуктами?
        public int Id { get; set; }
        public int Count { get; set; }
        // FK
        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
