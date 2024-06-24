using Newtonsoft.Json;

public class SelicExpectation
{
    [JsonProperty("Indicador")]
    public string Indicador { get; set; }

    [JsonProperty("Data")]
    public string Data { get; set; }

    [JsonProperty("Reuniao")]
    public string Reuniao { get; set; }

    [JsonProperty("Media")]
    public double Media { get; set; }

    [JsonProperty("Mediana")]
    public double Mediana { get; set; }

    [JsonProperty("DesvioPadrao")]
    public double DesvioPadrao { get; set; }

    [JsonProperty("Minimo")]
    public double Minimo { get; set; }

    [JsonProperty("Maximo")]
    public double Maximo { get; set; }

    [JsonProperty("numeroRespondentes")]
    public int NumeroRespondentes { get; set; }

    [JsonProperty("baseCalculo")]
    public int BaseCalculo { get; set; }
}
