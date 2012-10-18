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
                var p = new Person { Age = r.Next(25, 55), Name = names[r.Next(0, 4)] + " " + apellido, Childs = new List<Child>(), Tags = new List<string>() {"A","B"}};
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
            Console.WriteLine("Cuantos hay y las edades unicas con edad >=40 y con hijos edad >= 4");

            string nameFilter = "Juan Gomez";
            
            operations = new[]{
                new BsonDocument{
                    {
                        "$match" , new BsonDocument
                                       {                                           
                                           {"Age" , new BsonDocument{ {"$gte" , 40} } },
                                           {"Childs.Age" , new BsonDocument{ {"$gte" , 4} } }
                                       }
                    } 
                }, 
                new BsonDocument {
                {
                    "$group", new BsonDocument{ { "_id" , "$Name" } , {"Total" , new BsonDocument{ {"$sum",1} } },
                              new BsonDocument("UniqueAges" , new BsonDocument( "$addToSet" , "$Age" )) }
                }               
               },               
                new BsonDocument("$sort" , new BsonDocument( "Total" , -1 )),    
                new BsonDocument{
                    {
                        "$project", new BsonDocument{ {"_id",1}, {"Total",1},{"UniqueAges",1} }
                    }            
                },
                new BsonDocument("$match" , new BsonDocument( "Total" , new BsonDocument{ {"$gte" , 10} } )),
                //new BsonDocument("$unwind" , "$UniqueAges")
            };

            result = collection.Aggregate(operations);

            foreach (var document in result.ResultDocuments)
            {
                Console.WriteLine(document.ToJson());
            }

        }
    }
}
