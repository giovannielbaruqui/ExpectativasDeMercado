using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class MainViewModel
{
    private async Task ExportDataAsync()
    {
        var csvLines = new List<string>();

        if (SelectedApiType == "Expectativa Mercado")
        {
            csvLines.Add("Indicador;Data;Reuniao;Media;Mediana;DesvioPadrao;Minimo;Maximo;NumeroRespondentes;BaseCalculo");
            foreach (SelicExpectation item in Expectations)
            {
                csvLines.Add($"{item.Indicador};{item.Data};{item.Reuniao};{item.Media};{item.Mediana};{item.DesvioPadrao};{item.Minimo};{item.Maximo};{item.NumeroRespondentes};{item.BaseCalculo}");
            }
        }
        else
        {
            csvLines.Add("Indicador;Data;DataReferencia;Media;Mediana;DesvioPadrao;Minimo;Maximo;NumeroRespondentes;BaseCalculo");
            foreach (MarketExpectation item in Expectations)
            {
                csvLines.Add($"{item.Indicador};{item.Data};{item.DataReferencia};{item.Media};{item.Mediana};{item.DesvioPadrao};{item.Minimo};{item.Maximo};{item.NumeroRespondentes};{item.BaseCalculo}");
            }
        }

        var saveFileDialog = new SaveFileDialog
        {
            FileName = "ExpectativasDeMercado",
            DefaultExt = ".csv",
            Filter = "CSV files (*.csv)|*.csv"
        };

        bool? result = saveFileDialog.ShowDialog();

        if (result == true)
        {
            string filePath = saveFileDialog.FileName;
            await File.WriteAllLinesAsync(filePath, csvLines);
        }
    }

    private void UpdateIndicadores()
    {
        Indicadores.Clear();

        if (SelectedApiType == "Expectativa Mercado Mensais")
        {
            Indicadores.Add("IPCA");
            Indicadores.Add("IGP-M");
        }
        else if (SelectedApiType == "Expectativa Mercado")
        {
            Indicadores.Add("Selic");
        }

        SelectedIndicador = Indicadores.FirstOrDefault();
    }
}
