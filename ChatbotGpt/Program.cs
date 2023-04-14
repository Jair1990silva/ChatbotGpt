using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ChatGPTExample
{
    public static class ChatGPT
    {
        private static readonly HttpClient client = new HttpClient();
        private const string openaiApiKey = "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";
        private const string chatGptEndpoint = "https://api.openai.com/v1/engines/davinci/completions";

        public static async Task<string> GetCompletionAsync(string prompt)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openaiApiKey);

            var requestBody = new JObject
            {
                { "prompt", prompt },
                { "max_tokens", 150 },
                { "temperature", 0.9 }
            };

            var response = await client.PostAsync(chatGptEndpoint, new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseBody);

            return responseJson["choices"]?[0]?["text"]?.ToString();
        }
    }

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("Enter your prompt: ");
            var prompt = Console.ReadLine();

            var completion = await ChatGPT.GetCompletionAsync(prompt);
            Console.WriteLine(completion);
        }
    }
}