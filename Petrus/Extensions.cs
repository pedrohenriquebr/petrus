using System.Linq;

namespace PetrusPackage.Extensions
{
    public static class StringExtensions
    {

        public static string AppendToURL(this string baseURL, params string[] segments)
        {
            {
                return string.Join("/", new[] { baseURL.TrimEnd('/') }
                    .Concat(segments.Select(s => s.Trim('/'))));
            }
        }
    }
}
