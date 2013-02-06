using System;
using System.Collections.Generic;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace mongodb_samples
{
    public class Person : ISupportInitialize
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public List<Child> Childs { get; set; }
        public List<string> Tags { get; set; }

        public bool ShouldSerializeTags()
        {
            return (Tags != null && Tags.Count != 0);
        }

        [BsonExtraElements]
        public BsonDocument ExtraFields { get; set; }

        [BsonIgnore]
        public int ChidlsCount { get; private set; }


        public void BeginInit()
        {
        }

        public void EndInit()
        {   
            this.ChidlsCount = this.Childs == null ? 0 : this.Childs.Count;
        }
    }
}