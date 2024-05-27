using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Todo.Services
{
    public class Gravatar
    {
        private readonly HttpClient _httpClient;

        public Gravatar(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
      
        public static string GetHash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(emailAddress.Trim().ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToLowerInvariant();
            }
        }

        public async Task<GravatarProfile> GetGravatarProfileAsync(string email)
        {
            try
            {
                var hash = GetHash(email);
                var url = $"https://www.gravatar.com/{hash}.json";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "ToDoListApplication/1.0 (urlUnknown)");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return GravatarProfile.FromJson(jsonResponse);
                }
                else
                {
                    // TODO: Some Loger could be added to othe application 
                    Console.WriteLine($"Error fetching Gravatar profile: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException reqEx)
            {
                Console.WriteLine($"HTTP request error: {reqEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return null;
            }
        }
    }


    public class GravatarProfile
    {
        public List<GravatarEntry> Entry { get; set; }

        public static GravatarProfile FromJson(string json) =>
            JsonSerializer.Deserialize<GravatarProfile>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        public class GravatarEntry
        {
            public string Hash { get; set; }
            public string RequestHash { get; set; }
            public string ProfileUrl { get; set; }
            public string PreferredUsername { get; set; }
            public string ThumbnailUrl { get; set; }
            public string LastProfileEdit { get; set; }
            public string DisplayName { get; set; }
            public string Pronouns { get; set; }
            public string AboutMe { get; set; }
            public string CurrentLocation { get; set; }
        }

    }
}