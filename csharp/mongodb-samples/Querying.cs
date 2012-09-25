using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;

namespace mongodb_samples
{
    [TestFixture]
    public class Querying
    {
        [Test]
        public void Test()
        {
            //creamos la conexion y seleccionamos la base de datos
            var server = MongoServer.Create("mongodb://localhost:27017");
            var database = server.GetDatabase("testdb");
            var collection = database.GetCollection<Person>("persons");

            var r = new Random();
            
            for(int i=1; i < 100; i++)
            {
                //creamos un objeto persona
                var p = new Person { Age = r.Next(25, 55), Name = "Person Name", Chidls = new List<Child>() };
                p.Chidls.Add(new Child { Name = "child 1", Age = r.Next(1, 12) });
                p.Chidls.Add(new Child { Name = "child 2", Age = r.Next(1, 12) });

                //lo guardamos
                collection.Insert(p);
            }


            Console.WriteLine("All person");

            var persons = collection.FindAll();
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con age = 25");

            persons = collection.Find(Query.EQ("Age", 25));

            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }
            
            collection.Drop();
        }
    }
}
