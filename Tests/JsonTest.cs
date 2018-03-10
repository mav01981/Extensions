namespace Tests
{
    using System.Collections.Generic;
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
    }
}
