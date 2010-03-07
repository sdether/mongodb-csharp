using System.Configuration;

namespace MongoDB.Driver {
    public static class MongoFactory {

        public static string Host;
        public static int Port;
        static MongoFactory() {
            Host = ConfigurationManager.AppSettings["mongo.host"] ?? Connection.DEFAULTHOST;
            if(!int.TryParse(ConfigurationManager.AppSettings["mongo.port"], out Port)) {
                Port = Connection.DEFAULTPORT;
            }
        }

        public static Mongo CreateMongo() {
            return new Mongo(Host, Port);
        }

        public static Connection CreateConnection() {
            return new Connection(Host, Port);
        }
    }
}
