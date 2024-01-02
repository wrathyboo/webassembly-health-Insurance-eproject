using Newtonsoft.Json;

namespace HealthInsurance.Server.Helpers
{
    public static class Json
    {
        /// <summary>
        /// This method will help us handling ReferanceHandler from stuck in loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T SingularObjectDeserialize <T>(T list) //Return an object
        {
            var ClientSerialize = JsonConvert.SerializeObject(list,
         Formatting.Indented,
         new JsonSerializerSettings()
         {
             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
         }
          );
            return JsonConvert.DeserializeObject<T>(ClientSerialize)!;
        }

        public static List<T> ObjectDeserialize<T>(List<T> list) //Return a list of object
        {
            var ClientSerialize = JsonConvert.SerializeObject(list,
         Formatting.Indented,
         new JsonSerializerSettings()
         {
             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
         }
          );
            return JsonConvert.DeserializeObject<List<T>>(ClientSerialize)!;
        }
    }


}
