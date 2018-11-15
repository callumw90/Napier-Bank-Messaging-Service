using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NapierBank.Model
{
    /// <summary>
    /// 
    /// Serializes objects to JSON
    /// 
    /// </summary>
    class JSONSerializer
    {

        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };


        public static string Serializer<Message>(Message m)
        {
            string json = JsonConvert.SerializeObject(m, settings);
            return json;
        }

        //not implemented yet, for next version
        public static Message Deserializer<Message>(string json)
        {
            Message temp = JsonConvert.DeserializeObject<Message>(json, settings);
            return temp;
        }

    }
}
