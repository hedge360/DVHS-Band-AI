using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BandAI.Pages;

public class IndexModel : PageModel {
    [BindProperty]
    public string? UserInput { get; set; }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger) {
        _logger = logger;
    }

    public void OnGet() {
    }

    private const int MAX_RESP = 30;
    private const string delim = "?!.;,";
    private static readonly Random random = new Random();

    private class Record {
        public string Input { get; set; } = string.Empty;
        public string[] Responses { get; set; } = Array.Empty<string>();
    }

    private static readonly Record[] KnowledgeBase = {
        new Record {
            Input = "HELLO",
            Responses = new[] {
                "Placeholder response"
            }
        }
    };

    public void OnPost() {
        string sResponse = "";
        string result = "";

        if (string.IsNullOrEmpty(UserInput))
        {
            ViewData["Result"] = "Please enter a message";
            return;
        }

        string processedInput = UserInput;
        PreprocessInput(ref processedInput);
        var responses = FindMatch(processedInput);

        if (processedInput == "INFO") {
            result = "This Chatbot uses keywords to answer questions about band.\nPlease do not ask complicated questions. They will NOT be answered";
        } else if (responses.Count == 0) {
            result = "I do not understand what you are saying, or the question hasn't been asked before. Please Try Again.";
        } else {
            int nSelection = random.Next(responses.Count);
            sResponse = responses[nSelection];
            result = sResponse;
        }
        ViewData["Result"] = result;
    }

    private static void PreprocessInput(ref string str) {
        CleanString(ref str);
        UpperCase(ref str);
    }

    private static List<string> FindMatch(string input) {
        var listReturn = new List<string>();
        foreach (var record in KnowledgeBase) {
            if (input.Contains(record.Input)) {
                listReturn = record.Responses.Take(MAX_RESP).ToList();
                break;
            }
        }
        return listReturn;
    }

    private static void UpperCase(ref string str) {
        str = str.ToUpper();
    }

    private static bool IsPunc(char c) {
        return delim.IndexOf(c) != -1;
    }

    private static void CleanString(ref string str) {
        string temp = "";
        char prevChar = '\0';

        foreach (char c in str) {
            if (c == ' ' && prevChar == ' ') {
                continue;
            } else if (!IsPunc(c)) {
                temp += c;
            }
            prevChar = c;
        }
        str = temp;
    }
}
