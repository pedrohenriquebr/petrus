using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PetrusPackage;

namespace PetrusExamples
{
    class Program
    {

        async static Task AsyncMain()
        {

            var api = Petrus.Create(new ()
            {
                BaseURL = "https://pokeapi.co/api/v2/"
            });
            var result  = await api.Get("pokemon/pikachu");
            Pokemon pikachu = (result.Data as JObject).ToObject<Pokemon>();
            Console.WriteLine(result.Response.RequestMessage);
            Console.WriteLine("Name: " + pikachu.Name);
            Console.WriteLine("Height: " + pikachu.Height);
            Console.WriteLine("Weight: " + pikachu.Weight);

        }
        static  void Main(string[] args)
        {

            Task.Run(AsyncMain).Wait();
        }
    }
}
