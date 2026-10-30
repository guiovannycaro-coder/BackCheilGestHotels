
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.application.services;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Persistence;
using BackCheilGestHotels.infra.repositories;


var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión DefaultConnection.");

builder.Services.AddSingleton(
    new DatabaseConnection(connectionString));

// Repository
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersSeguridadRepository, UsersSeguridadRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

// Service
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersSeguridadService, UsersSeguridadServices>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();
builder.Services.AddScoped<ICitiesService, CitiesService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();

// Controllers for REST API
builder.Services.AddControllers();

// CORS: allow Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();




// Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "BackCheilGestHotels API v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();