using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petrus.Interfaces.Models;
using Petrus.Interfaces.Props;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Petrus.Tests
{


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
                data  = "ooo",
                key = "baaa"
            });

            Assert.IsNotNull(response.Data);
        }

    }
}