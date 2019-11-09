using System;
using System.Configuration;

namespace AromaCareGlow.Commerce.Data.MongoWrapper.Configuration
{
    public class MongoDbConfiguration: ConfigurationSection
    {
        public const string PreferredSectionName = "mongodb";

        [ConfigurationProperty("connectionString", IsDefaultCollection = false, IsRequired = true)]
        public string ConnectionString => (string)base["connectionString"];

        [ConfigurationProperty("defaultDatabase", IsDefaultCollection = false, IsRequired = true)]
        public string DefaultDatabase => (string)base["defaultDatabase"];          

        [ConfigurationProperty("mongoCollections", IsDefaultCollection = false, IsRequired = false)]
        public MongoCollections MongoCollections
        {
            get
            {
                return (MongoCollections)base["mongoCollections"];
            }
        }
    }

    [ConfigurationCollection(typeof(MongoCollection), AddItemName = "mongoCollection")]
    public class MongoCollections : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MongoCollection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((MongoCollection)element).Name;
        }
    }

    public class MongoCollection : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }
    }

}
