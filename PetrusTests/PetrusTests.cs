using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PetrusPackage.Interfaces.Models;
using PetrusTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetrusPackage.Tests
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
            PResult<dynamic> result = await Petrus.Get("https://pokeapi.co/api/v2/pokemon/pikachu");
            Assert.IsNotNull(result.Data.abilities, "abilities cannot be null");
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
            var response = await Petrus.Post(
                "https://classify-web.herokuapp.com/api/encrypt",
                new
                {
                    data = "ooo",
                    key = "baaa"
                });

            Assert.IsNotNull(response.Data);
        }


        [TestMethod()]
        public async Task GetAllStates_UsingGenericType()
        {
            var apiUrl = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
            var response = await Petrus.Get<State[]>(apiUrl);

            Assert.IsNotNull(response.Data);
            Assert.IsTrue(response.Data.Length > 0);

            Assert.AreEqual<string>(response.Data[0].Name, "Rondônia");
        }

    }
}