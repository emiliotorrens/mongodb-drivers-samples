using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using NUnit.Framework;

namespace mongodb_samples
{
    [TestFixture]
    public class ReplicaSets
    {
        [Test]
        public void Test()
        {            
             //creamos la conexion y seleccionamos la base de datos
            var client = new MongoClient("mongodb://192.168.1.218/?replicaSet=devSet;connect=automatic");
            var server = client.GetServer();
            server.Connect();
            Assert.AreEqual(3,server.Instances.Count());            

            //BreakPoint para hacer que caiga el primario (218)
            int i = 0;

            var database = server.GetDatabase("testdb");
            var collection = database.GetCollection<Person>("persons");
            collection.RemoveAll();

        }
    }
}
