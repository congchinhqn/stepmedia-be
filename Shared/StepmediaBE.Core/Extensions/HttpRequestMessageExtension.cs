using System.Net.Http;
using System.Threading;

namespace Metatrade.Core.Extensions
{
    public static class HttpRequestMessageExtension
    {
        public static void AddLanguage(this HttpRequestMessage message)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            message.Headers.Add("Accept-Language", culture.Name);
        }
    }
}