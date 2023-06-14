using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la conexión a MongoDB
var mongoConnectionString = "mongodb://localhost:27017";
var mongoDatabaseName = "mydb";

// Crear una instancia del cliente de MongoDB
var mongoClient = new MongoClient(mongoConnectionString);

// Obtener una referencia a la base de datos
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

// Registrar el cliente y la base de datos en el contenedor de dependencias
builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var collections = mongoDatabase.ListCollectionNames().ToList();
Console.WriteLine("Colecciones en la base de datos:");
foreach (var collection in collections)
{
    Console.WriteLine(collection);
}


app.Run();
