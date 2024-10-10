using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.FileProviders;
using WebApplicationGB.Abstractions;
using WebApplicationGB.Automapper;
using WebApplicationGB.Data;
using WebApplicationGB.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache(x=> x.TrackStatistics=true);

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Для связи интерфейсов с классами
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{ 
    container.RegisterType<ProductRepository>().As<IProductRepository>();
    container.RegisterType<ProductGroupRepository>().As<IProductGroupRepository>();

    // Регистрация контекста для репозиториев
    container.Register(_ => new Context(builder.Configuration.GetConnectionString("db"))).
                                        InstancePerDependency();

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Работа со статичными файлами
var staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
Directory.CreateDirectory(staticFilePath);

app.UseStaticFiles( new StaticFileOptions 
{
    FileProvider = new PhysicalFileProvider(staticFilePath),
    RequestPath = "/static"
}
   );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
