using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Petrus.Interfaces.Models;
using Petrus.Interfaces.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Petrus.Tests
{
    public static class Helpers
    {
        public static Int64 GetTime()
        {
            var time = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1));
            return (Int64)(time.TotalMilliseconds + 0.5);
        }

        public static string ParseXML(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                var doc = XElement.Parse(xml);
                var node_cdata = doc.DescendantNodes().OfType<XCData>().ToList();

                foreach (var node in node_cdata)
                {
                    node.Parent.Add(node.Value);
                    node.Remove();
                }

                return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.None, false);
            }

            return string.Empty;
        }


    }


    [TestClass()]
    public class PetrusTests
    {

        [TestMethod()]
        public async Task GetTestPokemon()
        {
            PetrusResult result = await Petrus.Get("https://pokeapi.co/api/v2/pokemon/pikachu");
            Assert.IsNotNull(result.Data.abilities, "abilities cannot be null");
        }

        [TestMethod()]
        public async Task GetTest1()
        {
            var result = await Petrus.Get("https://google.com/search", new PetrusOptions
            {
                Params = new List<(string, string)>()
                {
                    ("q","opa")
                }
            });

            Assert.IsNotNull(result.Data);
        }

        [TestMethod()]
        public async Task PostDynamicObject()
        {
            var response = await Petrus.Post("https://google.com", new
            {
                q = "pedro"
            });

            Assert.IsNotNull(response.Data);
        }

        [TestMethod()]
        public async Task GetTestApi()
        {
            var response = await Petrus.Get("https://api.publicapis.org/entries");

            Assert.IsNotNull(response.Data);
        }

        [TestMethod()]
        public async Task PostTest()
        {
            var response = await Petrus.Post("https://classify-web.herokuapp.com/api/encrypt", new
            {
                data = "ooo",
                key = "baaa"
            });

            Assert.IsNotNull(response.Data);
        }

        [TestMethod()]
        public async Task GetInitialData()
        {
            var response = await Petrus.Get(
                string
                .Format("http://{0}/moneyBook/getInitData?_dc={1}",
                    "10.0.0.111:8888", Helpers.GetTime()),
                new PetrusOptions { ForceJson = true });

            Assert.IsNotNull(response.Data);
        }

    }
}