namespace Tests
{
    using System.Collections.Generic;
    using Extensions;
    using Xunit;


    public class ModelA
    {
        public List<SubClass> subClass { get; set; }
    }
    public class SubClass
    {
        public List<int> SimpleTypeProperty { get; set; }
    }

    public class ModelB
    {
        public List<int> SimpleTypeProperty { get; set; }
    }

    public class ModelC
    {
        public List<int> SimpleTypeProperty { get; set; }
    }

    public class ModelD
    {
        public List<SubClassB> TypeProperty { get; set; }
    }
    public class SubClassB
    {
        public List<SubClassC> AlternativeProductIds { get; set; }
    }
    public class SubClassC
    {
        public List<int> AlternativeProductIds { get; set; }
    }


    public class CSVTests
    {
        [Fact]
        public void Check_With_1_Layer_Model()
        {
            var model = new ModelB()
            {
                SimpleTypeProperty = new List<int>() { 1, 2 }
            };

            List<ModelB> people = new List<ModelB>();

            people.Add(model);
            people.Add(model);

            Assert.Equal("SimpleTypeProperty,\r\n1,2,1,2,", people.ToCSV(","));
        }

        [Fact]
        public void Check_With_2_Layer_Model()
        {
            var model = new ModelA()
            {
                subClass = new List<SubClass>()
                         {
                               new SubClass()
                               {
                                   SimpleTypeProperty = new List<int>(){ 1,2 }
                               }
                         }
            };

            List<ModelA> people = new List<ModelA>();

            people.Add(model);
            people.Add(model);


            string a = people.ToCSV(",");
            Assert.Equal("SimpleTypeProperty,\r\n1,2,1,2,", people.ToCSV(","));
        }

        [Fact]
        public void Check_With_3_Layer_Model()
        {
            var model = new ModelA()
            {
                subClass = new List<SubClass>()
                         {
                               new SubClass()
                               {
                                   SimpleTypeProperty = new List<int>(){ 1,2 }
                               }
                         }
            };

            List<ModelA> people = new List<ModelA>();

            people.Add(model);
            people.Add(model);

            Assert.Equal("SimpleTypeProperty,\r\n1,2,1,2,", people.ToCSV(","));
        }
    }
}

