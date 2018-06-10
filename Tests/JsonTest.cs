namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Extensions;
    using Xunit;

    public class JsonTest
    {
        [Fact]
        public void Compare_Two_Json_Objects_That_Are_Differnt()
        {
            string jsonA = @"{
      'status_code': 200,
      'status_text': 'matches found',
      'data': [{
         'company': {
           'id': '1',
           'value': '201',
           'companyId': '2001',}
         },
         {
         'company': {
           'id': '2',
           'value': '20',
           'companyId': '2002',}
         },
         {
         'company': {
           'id': '3',
           'value': '30',
           'companyId': '2003',}
         },]
       }";

            string jsonB = @"{
      'status_code': 200,
      'status_text': 'matches found',
      'data': [{
         'company': {
           'id': '1',
           'value': '20',
           'companyId': '2001',}
         },
         {
         'company': {
           'id': '2',
           'value': '20',
           'companyId': '2002',}
         },
         {
         'company': {
           'id': '3',
           'value': '30',
           'companyId': '2003',}
         },]
       }";

            var differences = jsonA.CompareJsonObjects(jsonB);

            Assert.True(differences.Count == 2);
        }



        [Fact]
        public void Convert_JsonString_To_Dictionary()
        {
            string json = @"{
      'status_code': 200,
      'status_text': 'matches found',
      'data': [{
         'company': {
           'id': '1',
           'value': '20',
           'companyId': '2001',}
         },
         {
         'company': {
           'id': '2',
           'value': '20',
           'companyId': '2002',}
         },
         {
         'company': {
           'id': '3',
           'value': '30',
           'companyId': '2003',}
         },]
       }";

            Dictionary<string, string> dictionary = json.JsonToDictionary();

            Assert.True(dictionary.Count == 11);
        }

        [Fact]
        public void Convert_JsonString_To_Datable()
        {
            string json = new Car()
            {
                Ids = new int[] { 1, 2, 3 },
                Name = "Nissan Primera 1.8 Flare 5dr",
                Colour = "Red",
                Engine = new Engine()
                {
                    EngineSize = "1.8L",
                    Measure = new Type()
                    {
                        Name = "New measure Type"
                    }
                }
            }.ObjectToJson();

            var dataTable = json.ConvertJsonToDatatable();

            Assert.True(dataTable.Rows.Count == 11);
        }


        [Fact]
        public void Compress_Json_String()
        {
            string json = new Car()
            {
                Ids = new int[] { 1, 2, 3 },
                Name = "Nissan Primera 1.8 Flare 5dr",
                Colour = "Red",
                Engine = new Engine()
                {
                    EngineSize = "1.8L",
                    Measure = new Type()
                    {
                        Name = "New measure Type"
                    }
                }
            }.ObjectToJson();

            var compressedString = json.Compress();

            Assert.True((compressedString.Length * sizeof(byte)) < ASCIIEncoding.Unicode.GetByteCount(json));
        }

        [Fact]
        public void Unzip_Json_String()
        {
            string json = new Car()
            {
                Ids = new int[] { 1, 2, 3 },
                Name = "Nissan Primera 1.8 Flare 5dr",
                Colour = "Red",
                Engine = new Engine()
                {
                    EngineSize = "1.8L",
                    Measure = new Type()
                    {
                        Name = "New measure Type"
                    }
                }
            }.ObjectToJson();

            var compressedString = json.Compress();

            Assert.Equal(json, Convert.ToBase64String(compressedString).Unzip());
        }
    }
}
