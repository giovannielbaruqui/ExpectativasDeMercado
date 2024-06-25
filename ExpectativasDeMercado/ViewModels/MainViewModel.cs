using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ExpectativasDeMercado.Data;
using Microsoft.EntityFrameworkCore;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly BacenApiService _bacenApiService;
    private readonly MarketDbContext _dbContext;
    private string _selectedIndicador;
    private string _selectedApiType;
    private DateTime _startDate;
    private DateTime _endDate;
    private ObservableCollection<object> _expectations; // Alterado para tipo genérico
    private bool _isLoading;

    public MainViewModel(BacenApiService bacenApiService, MarketDbContext dbContext)
    {
        _bacenApiService = bacenApiService ?? throw new ArgumentNullException(nameof(bacenApiService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Expectations = new ObservableCollection<object>(); // Alterado para tipo genérico
        ApiTypes = new ObservableCollection<string> { "Expectativa Mercado Mensais", "Expectativa Mercado" };
        LoadDataCommand = new RelayCommandAsync(async () => await LoadDataAsync(), () => !IsLoading);
        ClearDataCommand = new RelayCommandAsync(async () => await ClearData());
        ExportDataCommand = new RelayCommandAsync(async () => await ExportDataAsync());
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        OnPropertyChanged(nameof(DadosCarregados));
    }

    public ObservableCollection<string> ApiTypes { get; }

    public ObservableCollection<string> Indicadores { get; private set; } = new ObservableCollection<string>();

    public string SelectedIndicador
    {
        get { return _selectedIndicador; }
        set { _selectedIndicador = value; OnPropertyChanged(); }
    }

    public string SelectedApiType
    {
        get { return _selectedApiType; }
        set
        {
            _selectedApiType = value;
            OnPropertyChanged();
            UpdateIndicadores(); // Atualiza os indicadores quando o tipo de API é alterado
        }
    }

    public DateTime StartDate
    {
        get { return _startDate; }
        set { _startDate = value; OnPropertyChanged(); }
    }

    public DateTime EndDate
    {
        get { return _endDate; }
        set { _endDate = value; OnPropertyChanged(); }
    }

    public ObservableCollection<object> Expectations
    {
        get { return _expectations; }
        set { _expectations = value; OnPropertyChanged(); OnPropertyChanged(nameof(DadosCarregados)); }
    }

    public int DadosCarregados
    {
        get { return Expectations.Count; }
    }

    public bool IsLoading
    {
        get { return _isLoading; }
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public RelayCommandAsync LoadDataCommand { get; }
    public RelayCommandAsync ClearDataCommand { get; }
    public RelayCommandAsync ExportDataCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            if (SelectedApiType == "Expectativa Mercado")
            {
                var data = await _bacenApiService.GetSelicExpectations(StartDate, EndDate);
                Expectations.Clear(); // Limpa a coleção antes de adicionar novos itens
                foreach (var item in data)
                {
                    Expectations.Add(item); // Adiciona o item na coleção
                }
            }
            else if (SelectedApiType == "Expectativa Mercado Mensais")
            {
                var data = await _bacenApiService.GetExpectations(SelectedIndicador, StartDate, EndDate);
                Expectations.Clear(); // Limpa a coleção antes de adicionar novos itens
                foreach (var item in data)
                {
                    Expectations.Add(item); // Adiciona o item na coleção
                }
            }

            await SaveChangesAsync(); // Salva as mudanças no banco de dados

            OnPropertyChanged(nameof(DadosCarregados));
        }
        catch (Exception ex)
        {
            // Lidar com exceções aqui, como logar ou mostrar um erro para o usuário
            Console.WriteLine($"Erro ao carregar dados: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SaveChangesAsync()
    {
        // Itera sobre os itens em Expectations e anexa ao contexto apropriado
        foreach (var item in Expectations)
        {
            if (item is SelicExpectation selicExpectation)
            {
                _dbContext.SelicExpectations.Attach(selicExpectation);
            }
            else if (item is MarketExpectation marketExpectation)
            {
                _dbContext.MarketExpectations.Attach(marketExpectation);
            }
        }

        // Salva as mudanças no banco de dados
        await _dbContext.SaveChangesAsync();
    }

    private async Task ClearData()
    {
        SelectedIndicador = null;
        SelectedApiType = null;
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        Expectations.Clear(); // Limpa a coleção
        OnPropertyChanged(nameof(DadosCarregados));
    }

    private async Task ExportDataAsync()
    {
        var csvLines = new List<string>();

        // Cria linhas CSV baseado no tipo de dado selecionado
        if (SelectedApiType == "Expectativa Mercado")
        {
            csvLines.Add("Indicador,Data,Reuniao,Media,Mediana,DesvioPadrao,Minimo,Maximo,NumeroRespondentes,BaseCalculo");
            foreach (SelicExpectation item in Expectations)
            {
                csvLines.Add($"{item.Indicador},{item.Data},{item.Reuniao},{item.Media},{item.Mediana},{item.DesvioPadrao},{item.Minimo},{item.Maximo},{item.NumeroRespondentes},{item.BaseCalculo}");
            }
        }
        else if (SelectedApiType == "Expectativa Mercado Mensais")
        {
            csvLines.Add("Indicador,Data,DataReferencia,Media,Mediana,DesvioPadrao,Minimo,Maximo,NumeroRespondentes,BaseCalculo");
            foreach (MarketExpectation item in Expectations)
            {
                csvLines.Add($"{item.Indicador},{item.Data},{item.DataReferencia},{item.Media},{item.Mediana},{item.DesvioPadrao},{item.Minimo},{item.Maximo},{item.NumeroRespondentes},{item.BaseCalculo}");
            }
        }

        // Solicita ao usuário para salvar o arquivo
        var saveFileDialog = new Microsoft.Win32.SaveFileDialog
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
