using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace mongodb_samples
{
    [TestFixture]
    public class Aggregation
    {
        [Test]
        public void Test()
        {
            //creamos la conexion y seleccionamos la base de datos
            var server = MongoServer.Create("mongodb://localhost:27017");
            var database = server.GetDatabase("testdb");
            var collection = database.GetCollection<Person>("persons");
            collection.RemoveAll();

            var r = new Random();
            string[] names = { "Juan", "Antonio", "Pedro", "Maria", "Jordi", "Mario" };
            string[] apellidos = { "Gomez", "Perez" };

            for (int i = 1; i < 100; i++)
            {
                string apellido = apellidos[r.Next(0, 1)];
                //creamos un objeto persona
                var p = new Person { Age = r.Next(25, 55), Name = names[r.Next(0, 4)] + " " + apellido, Childs = new List<Child>() };
                p.Childs.Add(new Child { Name = names[r.Next(0, 4)] + " " + apellido, Age = r.Next(1, 12) });
                p.Childs.Add(new Child { Name = names[r.Next(0, 4)] + " " + apellido, Age = r.Next(1, 12) });

                //lo guardamos
                collection.Insert(p);
            }

            Console.WriteLine("Cuantas personas de cada Nombre hay");

            var operations = new[]{
                new BsonDocument {
                {
                    "$group", new BsonDocument{ { "_id" , "$Name" } , {"Total" , new BsonDocument{ {"$sum",1} } } }
                }
               }
            };

            var result = collection.Aggregate(operations);

            foreach (var document in result.ResultDocuments)
            {
                Console.WriteLine(document.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Años Sumados de todas las personas x nombre");

            operations = new[]{
                new BsonDocument {
                {
                    "$group", new BsonDocument{ { "_id" , "$Name" } , {"Total" , new BsonDocument{ {"$sum","$Age"} } } }
                }
               }
            };

            result = collection.Aggregate(operations);

            foreach (var document in result.ResultDocuments)
            {
                Console.WriteLine(document.ToJson());
            }


            Console.WriteLine();
            Console.WriteLine("Cuandos hay y las edades unicas de los que se llaman Juan Gomez");

            operations = new[]{
                new BsonDocument("$match" , new BsonDocument( "Name" , "Juan Gomez" )), 
                new BsonDocument {
                {
                    "$group", new BsonDocument{ { "_id" , "$Name" } , {"Total" , new BsonDocument{ {"$sum",1} } },
                              new BsonDocument("DistincAges" , new BsonDocument( "$addToSet" , "$Age" )) }
                }               
               }               
            };

            result = collection.Aggregate(operations);

            foreach (var document in result.ResultDocuments)
            {
                Console.WriteLine(document.ToJson());
            }

        }
    }
}
