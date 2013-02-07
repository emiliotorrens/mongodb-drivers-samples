using System;
using System.Collections.Generic;
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
            var client = new MongoClient("mongodb://localhost:27017");
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase("testdb");
            MongoCollection<Person> collection = database.GetCollection<Person>("persons");
            collection.RemoveAll();

            //creamos un objeto persona
            var p = new Person {Age = 25, Name = "person Name", Cars=1,Childs = new List<Child>()};
            p.Childs.Add(new Child {Name = "child 1", Age = 12});
            p.Childs.Add(new Child {Name = "child 2", Age = 16});
           
            //lo guardamos
            collection.Save(p);

            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());

            //modificamos el objeto y lo enviamos entero
            p.Name = "person Name 2";
            p.Age = 35;
            p.Tags = new List<string>() {"A","B"};
            p.Childs[0].Age = 5;

            collection.Save(p);

            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());

            //le modificamos el nombre y la edad
            collection.Update(Query.EQ("_id", p.Id), Update.Set("Name", "Name Modified").Inc("Age", 1));

            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());

            //le anyadimos un hijo a los que tengan 37 años
            collection.Update(Query.EQ("Age", 36), Update.PushWrapped("Childs", new Child {Name = "child 3", Age = 3}),
                              UpdateFlags.Multi);

            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());
        }
    }
}