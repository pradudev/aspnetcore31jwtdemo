using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Helpers
{
    public static class JsonHelper
    {
        public static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(
                    obj,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        NullValueHandling = NullValueHandling.Ignore
                    });
        }
    }
}