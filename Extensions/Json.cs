using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

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
                                var path = entry.Key + "|" + property[0];

                                int record = output
                                    .Where(kvp => kvp.Key.Contains(path)).Count();

                                if (record > 0)
                                {
                                    path = record + @"|" + path;
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

                    var item = SupKey + " " + entry.Key;
                    var property = item.ToString().Split(':');
                    var path = entry.Key + "/" + property[0];

                    int record = output
                        .Where(kvp => kvp.Key.Contains(SupKey + " " + entry.Key)).Count();

                    output.Add(SupKey + " " + entry.Key + ":" + (record + 1).ToString(), entry.Value == null ? "" : entry.Value.ToString());

                }
            }

        }

        /// <summary>
        /// Convert Json Object to to Datable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static DataTable ConvertJsonToDatatable(this string json)
        {
            var jsonLinq = JObject.Parse(json);

            var linqArray = jsonLinq.Descendants().Where(x => x is JArray).First();
            var jsonArray = new JArray();
            foreach (JObject row in linqArray.Children<JObject>())
            {
                var createRow = new JObject();

                foreach (JProperty column in row.Properties())
                {
                    if (column.Value is JValue)
                    {
                        createRow.Add(column.Name, column.Value);
                        jsonArray.Add(createRow);
                    }
                    else if (column.Value is JArray)
                    {
                        string output = string.Empty;
                        int i = 1;
                        foreach (JObject value in column.Value as JArray)
                        {
                            foreach (var val in value.Properties())
                            {
                                createRow.Add(val.Name + i, val.Value);
                                jsonArray.Add(createRow);
                            }
                            i++;
                        }
                    }
                    else if (column.Value is IList)
                    {
                        foreach (JObject value in column as IEnumerable)
                        {
                            foreach (var val in value.Properties())
                            {
                                createRow.Add(val.Name, val.Value);
                                jsonArray.Add(createRow);
                            }
                        }
                    }
                }
            }
            return JsonConvert.DeserializeObject<DataTable>(jsonArray.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compressedJson"></param>
        /// <returns></returns>
        public static string Unzip(this string compressedJson)
        {
            using (var inputStream = new MemoryStream(Convert.FromBase64String(compressedJson)))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gZipStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static byte[] Compress(this string json)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(json);

            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                return outputStream.ToArray();
            }
        }
    }
}

