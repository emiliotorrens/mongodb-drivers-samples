using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace mongodb_samples
{
    public class Person
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public List<Child> Childs { get; set; }
    }
}