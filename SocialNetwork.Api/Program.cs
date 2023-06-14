using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
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

// Configurar la tubería de solicitudes HTTP.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Verificar si las colecciones ya existen
var usersCollectionExists = mongoDatabase.ListCollectionNames().ToList().Contains("Users");
var postsCollectionExists = mongoDatabase.ListCollectionNames().ToList().Contains("Posts");

if (usersCollectionExists && postsCollectionExists)
{
    Console.WriteLine("Las colecciones 'Users' y 'Posts' ya existen en la base de datos. No se crearán nuevamente.");
}
else
{
    // Crear la colección "Users" y agregar el documento
    var usersCollection = mongoDatabase.GetCollection<BsonDocument>("Users");

    var userDocument = new BsonDocument
    {
        { "_id", new ObjectId("64893f8fd3035777883e02ba") },
        { "Email", "adguangel@hotmail.com" },
        { "Name", "Angel Guevara" },
        { "Username", "guevaraucleshn" },
        {
            "Posts", new BsonArray
            {
                new BsonDocument
                {
                    { "_id", new ObjectId("64893a0fd3035777883e02b0") },
                    { "Content", "Hola Buenas" },
                    { "UserId", "64893f8fd3035777883e02ba" },

                }
            }
        }
    };
    var userDocument2 = new BsonDocument
    {
        { "_id", new ObjectId("64893f8fd3035777883e02bb") },
        { "Email", "otrousuario@gmail.com" },
        { "Name", "Otro Usuario" },
        { "Username", "otrousuario" },
        {
            "Posts", new BsonArray
            {
                new BsonDocument
                {

                }
            }
        }
    };

    usersCollection.InsertOne(userDocument);
    usersCollection.InsertOne(userDocument2);

    Console.WriteLine("Colección 'Users' creada y documento agregado.");

    // Crear la colección "Posts" y agregar el documento
    var postsCollection = mongoDatabase.GetCollection<BsonDocument>("Posts");

    var postDocument = new BsonDocument
    {
        { "_id", new ObjectId("64893a0fd3035777883e02b0") },
        { "Content", "Hola Buenas" },
        { "UserId", "64893f8fd3035777883e02ba" },
    };

    postsCollection.InsertOne(postDocument);

    Console.WriteLine("Colección 'Posts' creada y documento agregado.");
}

app.Run();
