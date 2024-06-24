using System;

public class MarketExpectation
{
    public string Indicador { get; set; }
    public DateTime Data { get; set; }
    public DateTime DataReferencia { get; set; }
    public decimal Media { get; set; }
    public decimal Mediana { get; set; }
    public decimal DesvioPadrao { get; set; }
    public decimal Minimo { get; set; }
    public decimal Maximo { get; set; }
    public int NumeroRespondentes { get; set; }
    public int BaseCalculo { get; set; }
}