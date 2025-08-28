using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BandAI.Pages;


//THIS CODE IS HOT GARBAGE



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

    /*
     * Knowledgebase stores possible user inputs for hardcoded question answering.
     * consider using an api in the future
     */

    private static readonly Record[] KnowledgeBase = {
        new Record {
            Input = "HELLO",
            Responses = new[] {
                "Hello, I am a Band Agent tailored to answering frequently asked questions about band and music. For help, enter \"Info\" for more information about me."
            }
        },
        new Record {
            Input = "WARM UP",
            Responses = new[] {
                "Recommended warmups are remingtons, scales, long tones, arpeggios, etc."
            }
        },
        new Record {
            Input = "WARMUP",
            Responses = new[] {
                "Recommended warmups are remingtons, scales, long tones, arpeggios, etc."
            }
        },
        new Record {
            Input = "PEP MUSIC",
            Responses = new[] {
                "https://drive.google.com/drive/folders/1MQuwaHkBiE5paFE7sfW3GfF-IQnD8Dk3?usp=drive_link"
            }
        },
        new Record {
            Input = "CLEAN",
            Responses = new[] {
                "https://www.yamaha.com/en/musical_instrument_guide/"
            }
        },
        new Record {
            Input = "INSTRUMENT",
            Responses = new[] {
                "https://www.yamaha.com/en/musical_instrument_guide/"
            }
        },
        new Record {
            Input = "TIPS FOR PEP",
            Responses = new[] {
                "Wear a sweatshirt to football games, its COLD. Conserve air. Dont' blast, there are 200-300 of you"
            }
        }
        //Future inputs to be added
    };

    public void OnPost() { //Main method
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
            result = "This Chatbot uses keywords to answer questions about band. Please do not ask complicated questions. They will NOT be answered. Use as little words as possible";
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

    /*
     * Takes user input and references the knowledgebase
     * for matching inputs. If inputs match, the corresponding
     * response is given out to the user.
     */
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

    private static bool IsPunc(char c) { //I have no idea what this does
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
