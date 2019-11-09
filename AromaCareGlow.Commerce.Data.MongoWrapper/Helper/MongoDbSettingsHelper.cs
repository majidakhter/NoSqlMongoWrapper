
using System.Collections.Generic;

using System.Configuration;
using MongoDB.Driver;
using AromaCareGlow.Commerce.Data.MongoWrapper.Configuration;
using System.Net.Sockets;
using System;

namespace AromaCareGlow.Commerce.Data.MongoWrapper.Helpers
{
    public class MongoDbSettingsHelper
    {
        public MongoClient client { get; set; }
        public string DefaultDatabase { get; set; }
        public string DefaultCollection { get; set; }
        public MongoCollections MongoCollections { get; set; }

        public MongoDbSettingsHelper()
        {
            Init();
        }

        public MongoDbSettingsHelper(string connectionString, string defaultDatabase)
        {
            Init(connectionString, defaultDatabase);
        }

        private void Init()
        {
            MongoDbConfiguration configuration = ConfigurationManager.GetSection(MongoDbConfiguration.PreferredSectionName) as MongoDbConfiguration;
            // client = new MongoClient(configuration.ConnectionString);     
            client = CreateClient(configuration.ConnectionString);
            DefaultDatabase = configuration.DefaultDatabase;
            MongoCollections = configuration.MongoCollections;
        }

        private void Init(string connectionString, string defaultDatabase)
        {
            // client = new MongoClient(connectionString);
            client = CreateClient(connectionString);
            DefaultDatabase = defaultDatabase;
        }
        private MongoClient CreateClient(string connectionString)
        {
            void SocketConfigurator(Socket s) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.SocketTimeout = TimeSpan.FromSeconds(60);
            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(60);
            settings.ClusterConfigurator = builder =>
                builder.ConfigureTcp(tcp => tcp.With(socketConfigurator: (Action<Socket>)SocketConfigurator));
            var mongoclient = new MongoDB.Driver.MongoClient(settings);
            return mongoclient;


        }

    }
}
