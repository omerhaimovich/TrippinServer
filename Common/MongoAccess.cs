using Common.Interfaces;
using Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MongoAccess
    {
        private static MongoAccess m_objInstance;
        private IMongoDatabase m_objDatabase;
        private Dictionary<string, object> m_dicCollections;


        public static IMongoCollection<T> Access<T>() where T : ICollectional, new()
        {
            if (m_objInstance == null)
                m_objInstance = new MongoAccess();

            T obj = new T();

            if (!m_objInstance.m_dicCollections.ContainsKey(obj.GetCollectionName()))
            {
                m_objInstance.m_dicCollections.Add(obj.GetCollectionName(), m_objInstance.m_objDatabase.GetCollection<T>(obj.GetCollectionName()));
            }

            return m_objInstance.m_dicCollections[obj.GetCollectionName()] as IMongoCollection<T>;
        }

        private MongoAccess()
        {

            MongoUrl uri = new MongoUrl(ConfigurationSettings.AppSettings["MongoPath"]);

            MongoClient mongoClient = new MongoClient(uri);
            m_objDatabase = mongoClient.GetDatabase("Trippin");
            m_dicCollections = new Dictionary<string, object>();
        }

    }
}
