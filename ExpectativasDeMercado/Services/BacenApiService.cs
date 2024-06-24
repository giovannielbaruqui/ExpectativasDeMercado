using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class BacenApiService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<List<object>> GetExpectations(string indicador, DateTime startDate, DateTime endDate)
    {
        string baseUrl = "https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais";
        string filter = $"Indicador eq '{indicador}' and Data ge '{startDate:yyyy-MM-dd}' and Data le '{endDate:yyyy-MM-dd}'";
        string url = $"{baseUrl}?$filter={Uri.EscapeDataString(filter)}&$format=json";

        Console.WriteLine("Generated URL for Market Expectations: " + url); // Linha para depuração

        var response = await client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response Content for Market Expectations: " + content); // Linha para depuração

        response.EnsureSuccessStatusCode(); // Levanta uma exceção se a resposta não for bem-sucedida

        var result = JsonConvert.DeserializeObject<RootObjectMarket>(content);

        return result?.Value.Cast<object>().ToList() ?? new List<object>();
    }

    public async Task<List<object>> GetSelicExpectations(DateTime startDate, DateTime endDate)
    {
        string baseUrl = "https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativasMercadoSelic";
        string filter = $"Data ge '{startDate:yyyy-MM-dd}' and Data le '{endDate:yyyy-MM-dd}'";
        string url = $"{baseUrl}?$filter={Uri.EscapeDataString(filter)}&$format=json";

        Console.WriteLine("Generated URL for Selic: " + url); // Linha para depuração

        var response = await client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response Content for Selic: " + content); // Linha para depuração

        response.EnsureSuccessStatusCode(); // Levanta uma exceção se a resposta não for bem-sucedida

        var result = JsonConvert.DeserializeObject<RootObjectSelic>(content);

        return result?.Value.Cast<object>().ToList() ?? new List<object>();
    }

    private class RootObjectMarket
    {
        [JsonProperty("value")]
        public List<MarketExpectation> Value { get; set; } = new List<MarketExpectation>();
    }

    private class RootObjectSelic
    {
        [JsonProperty("value")]
        public List<SelicExpectation> Value { get; set; } = new List<SelicExpectation>();
    }
}
