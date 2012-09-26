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
            string[] names = {"Juan", "Antonio", "Pedro", "Maria", "Jordi", "Mario"};
            string[] apellidos = {"Gomez","Perez"};
            
            for(int i=1; i < 100; i++)
            {
                string apellido = apellidos[r.Next(0, 1)];
                //creamos un objeto persona
                var p = new Person { Age = r.Next(25, 55), Name = names[r.Next(0,4)] + " " + apellido, Childs = new List<Child>() };
                p.Childs.Add(new Child { Name =  names[r.Next(0,4)] + " " + apellido, Age = r.Next(1, 12) });
                p.Childs.Add(new Child { Name =  names[r.Next(0,4)] + " " + apellido, Age = r.Next(1, 12) });

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
            
            Console.WriteLine();
            Console.WriteLine("Personas con age >= 30 y <= 40 ordenado por age");
            persons = collection.Find(Query.And(Query.GTE("Age", 30),Query.LTE("Age",40))).SetSortOrder("Age");

            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con age = 30 y con child age = 5 ordenado por age");
            persons = collection.Find(Query.And(Query.EQ("Age", 30), Query.EQ("Childs.Age", 5))).SetSortOrder("Age");

            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con Childs age = 5, solo devolvemos el campo childs");
            persons = collection.Find(Query.EQ("Childs.Age", 5)).SetFields("Childs");

            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            
            collection.Drop();
        }
    }
}
