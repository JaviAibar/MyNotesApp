using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BlazorNotesApp.WeatherModule
{
    public partial class TemperatureView
    {
        [Inject]
        IHttpClientFactory ClientFactory { get; set; }
        public float? Temperature { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.open-meteo.com/v1/forecast?latitude=48.5227&longitude=9.0522&hourly=temperature_2m");
            request.Headers.Add("Accept", "text/json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                JsonNode temperatures = await JsonSerializer.DeserializeAsync<JsonNode>(responseStream);
                DateTime clock = DateTime.UtcNow.AddHours(2);
                JsonNode hourly = temperatures["hourly"];
                JsonNode time = hourly["time"];
                JsonNode temperature_2m = hourly["temperature_2m"];

                string currentTimeString = clock.ToString("s").Substring(0, 14) + "00";

                JsonNode currentTime = JsonValue.Create(currentTimeString);
                await Console.Out.WriteLineAsync(temperature_2m[clock.Hour].ToJsonString());
                Temperature = float.Parse(temperature_2m[clock.Hour].ToJsonString(), CultureInfo.InvariantCulture);
            }

        }
    }
}