using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

using Receipts.ReadModel.CrossCutting.Config;

namespace Receipts.ReadModel.CrossCutting.Extensions.Mongo
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, MongoSettings mongoSettings)
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
            #pragma warning disable CS0618
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
            #pragma warning restore CS0618
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            services.AddSingleton(sp =>
            {
                var mongoClient = sp.GetService<IMongoClient>()!;
                var db = mongoClient.GetDatabase(mongoSettings.Database);
                return db;
            });

            return services;
        }
    }
}
