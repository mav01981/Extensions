using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class JsonExtension
    {
        public static string ObjectToJson<T>(this T @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static string ObjectToJson<T>(this List<T> @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static Dictionary<string, string> JsonToDictionary(this string value, string suppressKey = "")
        {
            try
            {
                JToken.Parse(value);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            Dictionary<string, string> output = new Dictionary<string, string>();
            Dictionary<string, object> dictionaryA = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
            JsonToDictionary(dictionaryA, suppressKey, output);
            return output;
        }

        public static List<string> CompareJsonObjects(this string jobjectA, string jObjectB)
        {
            try
            {
                JToken.Parse(jobjectA);
                JToken.Parse(jObjectB);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> dictionaryA = JsonConvert.DeserializeObject<Dictionary<string, object>>(jobjectA);
            Dictionary<string, object> dictionaryB = JsonConvert.DeserializeObject<Dictionary<string, object>>(jObjectB);

            Dictionary<string, string> outputA = new Dictionary<string, string>();
            Dictionary<string, string> outputB = new Dictionary<string, string>();
            JsonToDictionary(dictionaryA, "", outputA);
            JsonToDictionary(dictionaryB, "", outputB);

            var collection1Diff = outputA
                .Where(entry => outputB[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            var collection2Diff = outputB
                .Where(entry => outputA[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            var differences = new List<Tuple<string, string, string, string>>();
            List<string> output = new List<string>();

            foreach (var item in collection1Diff)
            {
                output.Add(item.Key + ":" + item.Value);
            }
            foreach (var item in collection2Diff)
            {
                output.Add(item.Key + ":" + item.Value);
            }

            return output;
        }

        static void JsonToDictionary(Dictionary<string, object> dic, string SupKey, Dictionary<string, string> output)
        {
            foreach (KeyValuePair<string, object> entry in dic)
            {
                if (entry.Value is Dictionary<string, object>)
                    JsonToDictionary((Dictionary<string, object>)entry.Value, entry.Key, output);
                else
                    if (entry.Value is ICollection)
                {
                    foreach (var item in (ICollection)entry.Value)
                    {
                        if (item is Dictionary<string, object>)
                            JsonToDictionary((Dictionary<string, object>)item, SupKey + " : " + entry.Key, output);
                        else
                        {
                            if (item is JProperty)
                            {
                                var property = item.ToString().Split(':');
                                var path = entry.Key + "/" + property[0];

                                int record = output
                                    .Where(kvp => kvp.Key.Contains(path)).Count();

                                if (record > 0)
                                {
                                    path = record + @"/" + path;
                                }

                                output.Add(path, property[1]);

                            }
                            else
                            {
                                string line = item.ToString().Replace("{", "").Replace("}", "");
                                var @object = JsonConvert
                                    .DeserializeObject<Dictionary<string, object>>(item.ToString());

                                JsonToDictionary((Dictionary<string, object>)@object, SupKey + " : " + entry.Key, output);
                            }

                        }
                    }
                }
                else
                {
                    output.Add(entry.Key, entry.Value.ToString());
                }
            }
        }

    }
}
