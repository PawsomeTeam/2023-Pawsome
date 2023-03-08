using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OpenAI_API.Completions;
using OpenAI_API;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net.Http.Headers;
using System.Threading.Channels;

namespace PawsomeProject.Client.Pages;

public partial class ChatbotBase : ComponentBase
{
    [Inject] HttpClient httpClient { get; set; }
    [Inject] NavigationManager _navigationManager { get; set; }
    [Inject] IJSRuntime _jsRuntime { get; set; }

    public string message { get; set; }
    public string generatedText { get; set; } = "Hi there! I'm Pawla, how can I help? I will try my best to answer your questions😊";
    private readonly HttpClient _httpClient = new HttpClient();




    protected async Task GetResponseFromGPT3()
    {
        var persona = "veterinarian named Pawla";
        var introMessage = "This is your Pawsome Assistant named Pawla, I'm here to help you learn more about pets and animals. You can ask me any question related to pets or chat with me about anything else. I will try my best to answer your questions and make you happy.😊";
        var prompt = $"{persona}: {introMessage}\nYou: {message}\n{persona}: ";
        generatedText = "Give me a sec, Im thinking 😁";
        
        // SecretClientOptions options = new SecretClientOptions()
        // {
        //     Retry =
        //     {
        //         Delay= TimeSpan.FromSeconds(2),
        //         MaxDelay = TimeSpan.FromSeconds(16),
        //         MaxRetries = 5,
        //         Mode = RetryMode.Exponential
        //     }
        // };
        // var client = new SecretClient(new Uri("https://pawsome.vault.azure.net/"), new DefaultAzureCredential(), options);

        // KeyVaultSecret secret = client.GetSecret("OpenAiKeyString");

        // string secretValue = secret.Value;
        try
        {

            string apiKey = "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";

            string answer = string.Empty;
            var openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = prompt;
            completion.Model = OpenAI_API.Models.Model.DavinciText;
            completion.MaxTokens = 4000;
            completion.BestOf = 1;
            var result = await openai.Completions.CreateCompletionAsync(completion);

            Console.WriteLine(result);
            if (result != null)
            {
                foreach (var item in result.Completions)
                {
                    GPT3Response.Response = item.Text;
                }
                generatedText = GPT3Response.Response;
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }



    public static class GPT3Response
    {
        public static string Response { get; set; }
    }

}