using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplicationGB.Models;

namespace WebApplicationGB.Data
{
    public class Context : DbContext
    {
        // Объявление ДБ-сетов (будующие таблицы в БД). Соответсвенно моделям описываются сущности
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductGroup> ProductGroup { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }

        // переопределение методов
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.

        //    UseSqlServer("Data source = master; Initial Catalog = WebAppGB;" +
        //    " Trusted_Connection = True; TrustServerCertificate = True").
        //    UseLazyLoadingProxies().LogTo(Console.WriteLine);
        //// здесь Console.WriteLine используется как делегат
        /// <summary>

        // string connectionString = "server=WebAppServer;user=root;password=22022202";
        string connectionString = "Server=localhost; Port=3306; Database=test; Uid=root; Pwd=22022202;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.
            UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).UseLazyLoadingProxies().LogTo(Console.WriteLine);
        // здесь Console.WriteLine используется как делегат

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.HasKey(pg => pg.Id).HasName("product_group_pk");
                // наименование таблицы
                entity.ToTable("Category");
                // Property(pg => pg.Name) - задаёт колоке имя из класса.
                // Следющий метод,задаёт другое имя, при необходимости
                entity.Property(pg => pg.Name).
                HasColumnName("name").
                HasMaxLength(255);

                entity.Property(d=>d.Description).HasColumnName("description").HasMaxLength(500);

            }
                );

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(pg => pg.Id).HasName("product_pk");

                // entity.ToTable("product"); изначальное имя сущности и так будет "Product"

                entity.Property(pg => pg.Name).
                HasColumnName("name").
                HasMaxLength(255);

                // связи
                entity.HasOne(p => p.ProductGroup).WithMany(p => p.Products).HasForeignKey(p => p.ProductGroupID);
                // т.е. ProductGroup - один, а Products - много
            }
        );

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(pg => pg.Id).
                HasName("storage_pk");

                entity.HasOne(p => p.Product).
                WithMany(p => p.Storages).
                HasForeignKey(p => p.ProductID);
            }
);
            // после завершения создания сущностей создаётся миграция 
            // dotnet ef migrations add InitialCreate
            // возможны ошибки. Пришлось доустановить ef design и пересобрать приложение 

            

            // ипользуется после миграции для связи с sql сервером 
            // dotnet ef database update

            // dotnet tool install --global dotnet-ef
            // dotnet --ef

        }
    }
}
