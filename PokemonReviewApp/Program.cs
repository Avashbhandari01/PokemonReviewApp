using DataAccess.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using Services.Interfaces;
using Services.Repository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewerService, ReviewerService>();
builder.Services.AddScoped<IPokemonCategoryService, PokemonCategoryService>();
builder.Services.AddScoped<IPokemonOwnerService, PokemonOwnerService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{ 
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopeFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
