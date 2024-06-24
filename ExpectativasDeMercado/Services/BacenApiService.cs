using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class BacenApiService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<List<MarketExpectation>> GetMarketExpectations(string indicador, DateTime startDate, DateTime endDate)
    {
        string baseUrl = "https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais";
        string filter = $"Indicador eq '{indicador}' and Data ge '{startDate:yyyy-MM-dd}' and Data le '{endDate:yyyy-MM-dd}'";
        string url = $"{baseUrl}?$filter={Uri.EscapeDataString(filter)}&$format=json";

        Console.WriteLine("Generated URL: " + url); // Linha para depuração

        var response = await client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response Content: " + content); // Linha para depuração

        response.EnsureSuccessStatusCode(); // Levanta uma exceção se a resposta não for bem-sucedida

        var result = JsonConvert.DeserializeObject<RootObject>(content);

        return result?.Value ?? new List<MarketExpectation>();
    }

    private class RootObject
    {
        [JsonProperty("value")]
        public List<MarketExpectation> Value { get; set; } = new List<MarketExpectation>();
    }
}
