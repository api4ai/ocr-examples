using System;
using System.Net.Http;
using System.Text.Json.Nodes;

using MimeTypes;
using RestSharp;

#pragma warning disable CS0162


/*
 * Use 'normal' mode if you have an API Key from the API4AI Developer Portal. This is the method that users should normally prefer.
 *
 * Use 'rapidapi' if you want to try api4ai via RapidAPI marketplace.
 * For more details visit:
 *   https://rapidapi.com/api4ai-api4ai-default/api/ocr43/details
 */

const String MODE = "normal";


/*
 * Your API4AI key. Fill this variable with the proper value if you have one.
 */
const String API4AI_KEY = "";


/*
 * Your RapidAPI key. Fill this variable with the proper value if you want
 * to try api4ai via RapidAPI marketplace.
 */
const String RAPIDAPI_KEY = "";

String url;
Dictionary<String, String> headers = new Dictionary<String, String>();

switch (MODE) {
    case "normal":
        url = "https://api4ai.cloud/ocr/v1/results";
        headers.Add("X-API-KEY", API4AI_KEY);
        break;
    case "rapidapi":
        url = "https://ocr43.p.rapidapi.com/v1/results";
        headers.Add("X-RapidAPI-Key", RAPIDAPI_KEY);
        break;
    default:
        Console.WriteLine($"[e] Unsupported mode: {MODE}");
        return 1;
}

// Prepare request.
String image = args.Length > 0 ? args[0] : "https://static.api4.ai/samples/ocr-1.png";
var client = new RestClient(new RestClientOptions(url) { ThrowOnAnyError = true });
var request = new RestRequest();
if (image.Contains("://")) {
    request.AddParameter("url", image);
} else {
    request.AddFile("image", image, MimeTypeMap.GetMimeType(Path.GetExtension(image)));
}
request.AddHeaders(headers);

// Perform request.
var jsonResponse = (await client.ExecutePostAsync(request)).Content!;

// Print raw response.
Console.WriteLine($"[i] Raw response:\n{jsonResponse}\n");

// Parse response and print recognized text.
JsonNode docRoot = JsonNode.Parse(jsonResponse) ?? throw new InvalidOperationException("Failed to parse response JSON.");
JsonArray results = docRoot["results"]?.AsArray() ?? throw new InvalidOperationException("Response JSON does not contain a 'results' array.");
foreach (JsonNode? resultNode in results) {
    JsonObject result = resultNode?.AsObject() ?? throw new InvalidOperationException("Response result is not an object.");
    String text = result["entities"]?[0]?["objects"]?[0]?["entities"]?[0]?["text"]?.GetValue<String>()
        ?? throw new InvalidOperationException("Response result does not contain recognized text.");
    String page_tip = result["page"] is JsonNode page ? $" on page {page.GetValue<int>()}" : "";
    Console.WriteLine($"[i] Recognized text{page_tip}:\n{text}\n");
}

return 0;
