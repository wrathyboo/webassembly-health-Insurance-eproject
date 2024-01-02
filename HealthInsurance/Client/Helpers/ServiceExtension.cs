using HealthInsurance.Shared;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HealthInsurance.Client.Helpers
{
    public static class ServiceExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization, string method)
        {
            ApplicationUser s = new ApplicationUser();
            HttpRequestMessage request;
            switch (method.ToUpper())
            {
                case "POST":
                    request = new HttpRequestMessage(HttpMethod.Post, url);
                    break;
                case "PUT":
                    request = new HttpRequestMessage(HttpMethod.Put, url);
                    break;
                case "DELETE":
                    request = new HttpRequestMessage(HttpMethod.Delete, url);
                    break;
                default:
                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    break;
            }

            request.Headers.Authorization = authorization;

            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = authorization;

            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static async Task<T> PutJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = authorization;

            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static async Task<T> DeleteJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = authorization;

            var response = await httpClient.SendAsync(request);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Deserialize<T>(responseBytes, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
