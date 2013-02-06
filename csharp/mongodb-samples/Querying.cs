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
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();
            var database = server.GetDatabase("testdb");
            var collection = database.GetCollection<Person>("persons");
            collection.Drop();

            //Creamos datos
            var r = new Random();
            string[] names = {"Juan", "Antonio", "Pedro", "Maria", "Jordi", "Mario"};
            string[] apellidos = {"Gomez","Perez"};
            
            var personsToInsert = new List<Person>();
            for(int i=1; i < 100; i++)
            {
                string apellido = apellidos[r.Next(0, 1)];
                //creamos un objeto persona
                var p = new Person { Age = r.Next(25, 55), Name = names[r.Next(0,4)] + " " + apellido, Childs = new List<Child>() };
                p.Childs.Add(new Child { Name =  names[r.Next(0,4)] + " " + apellido, Age = r.Next(1, 12) });
                p.Childs.Add(new Child { Name =  names[r.Next(0,4)] + " " + apellido, Age = r.Next(1, 12) });                
                personsToInsert.Add(p);
            }

            //Insertamos en Bach
            collection.InsertBatch(personsToInsert);


            Console.WriteLine("All person");

            var persons = collection.FindAll();
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con age = 25 sin key");
            
            persons = collection.Find(Query.EQ("Age", 25));
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con age = 25 con key");

            collection.EnsureIndex("Age");
            persons = collection.Find(Query.EQ("Age", 25));
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }


            Console.WriteLine();
            Console.WriteLine("Personas con age = 25 o = 26 que se llamen Juan");

            persons = collection.Find(Query.And(Query.Or(Query.EQ("Age", 25), Query.EQ("Age", 26)),Query.Matches("Name",new BsonRegularExpression("Juan"))));
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }
            
            Console.WriteLine();
            Console.WriteLine("Personas con age >= 30 y <= 40 ordenado por age");
            
            persons = collection.Find(Query.And(Query.GTE("Age", 30),Query.LTE("Age",40))).SetSortOrder("Age");
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con age = 30 y con child age = 5 ordenado por age");
            persons = collection.Find(Query.And(Query.EQ("Age", 30), Query.EQ("Childs.Age", 5))).SetSortOrder("Age");
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("Personas con Childs age = 5, solo devolvemos el campo childs");
            persons = collection.Find(Query.EQ("Childs.Age", 5)).SetFields("Childs");
            Console.WriteLine("Explain: " + persons.Explain().ToJson());
            foreach (Person person in persons)
            {
                Console.WriteLine(person.ToJson());
            }
            
        }
    }
}
