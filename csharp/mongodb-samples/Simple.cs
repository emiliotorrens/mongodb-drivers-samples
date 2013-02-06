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
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();
            var database = server.GetDatabase("testdb");
            var collection = database.GetCollection<Person>("persons");
            collection.RemoveAll();

            //creamos un objeto persona
            var p = new Person {Age = 25, Name = "person Name", Childs = new List<Child>()};
            p.Childs.Add(new Child { Name = "child 1", Age= 12 });
            p.Childs.Add(new Child { Name = "child 2", Age = 16 });

            //lo guardamos
            collection.Insert(p);

            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());            
			
			//modificamos el objeto y lo enviamos entero
			p.Name = "person Name 2";
			p.Age = 35;
			p.Childs[0].Age = 5;
			
			collection.Save(p);
			
			p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());            
			
            //le modificamos el nombre
            collection.Update(Query.EQ("_id", p.Id), Update.Set("Name", "Name Modified"));

            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());            

            //le anyadimos un hijo
            collection.Update(Query.EQ("_id", p.Id), Update.PushWrapped("Childs", new Child { Name = "child 3", Age = 3 }));
            
            p = collection.FindOne();
            Console.WriteLine();
            Console.WriteLine(p.ToJson());            
            

        }
    }
}
