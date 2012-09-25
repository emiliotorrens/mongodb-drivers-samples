using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;

namespace mongodb_samples
{
    [TestFixture]
    public class Simple
    {
        [Test]
        public void Test()
        {
            //creamos la conexion y seleccionamos la base de datos
            var server = MongoServer.Create("mongodb://localhost:27017");
            var database = server.GetDatabase("testdb");
            var collection = database.GetCollection<Person>("persons");

            //creamos un objeto persona
            var p = new Person {Age = 25, Name = "person Name", Chidls = new List<Child>()};
            p.Chidls.Add(new Child { Name = "child 1", Age= 12 });
            p.Chidls.Add(new Child { Name = "child 2", Age = 16 });

            //lo guardamos
            collection.Insert(p);

            p = collection.FindOne();
            Console.WriteLine(p.ToJson());

            //le modificamos el nombre
            collection.Update(Query.EQ("_id", p.Id), Update.Set("Name", "Name Modified"));

            p = collection.FindOne();
            Console.WriteLine(p.ToJson());

            //le modificamos el nombre
            collection.Update(Query.EQ("_id", p.Id), Update.PushWrapped("Chidls", new Child { Name = "child 3", Age = 3 }));

            //le anyadimos un hijo
            p = collection.FindOne();
            Console.WriteLine(p.ToJson());

            collection.Drop();


        }
    }
}
