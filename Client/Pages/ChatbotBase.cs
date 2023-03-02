﻿using System.Text.Json.Serialization;
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
    public string generatedText { get; set; } = "This is Pawesome Assistant. I'm here to help you learn more about pets and animals. You can ask me any question related to pets or chat with me about anything else. I will try my best to answer your questions.😊";
    private readonly HttpClient _httpClient = new HttpClient();



    protected async Task GetResponseFromGPT3()
    {
        var persona = "veterinarian";
        var introMessage = "This is Pawesome Assistant. I'm here to help you learn more about pets and animals. You can ask me any question related to pets or chat with me about anything else. I will try my best to answer your questions and make you happy.😊";
        var prompt = $"{persona}: {introMessage}\nYou: {message}\n{persona}: ";
        generatedText = "Fething response...";
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