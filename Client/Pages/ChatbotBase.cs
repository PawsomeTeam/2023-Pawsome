using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OpenAI_API.Completions;
using OpenAI_API;
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
    public string generatedText { get; set; } = "Answer will be displayed here.";
    private readonly HttpClient _httpClient = new HttpClient();

    protected async Task GetResponseFromGPT3()
    {
        generatedText = "Brain Storming...";
        try
        { 
            string apiKey = "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";
            string answer = string.Empty;
            var openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = message;
            completion.Model = OpenAI_API.Models.Model.DavinciText; //OpenAI_API.Model.DavinciText;
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