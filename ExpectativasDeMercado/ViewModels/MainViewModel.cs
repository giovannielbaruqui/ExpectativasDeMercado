using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly BacenApiService _bacenApiService;
    private string _selectedIndicador;
    private DateTime _startDate;
    private DateTime _endDate;
    private ObservableCollection<object> _expectations; // Alterado para tipo genérico
    private bool _isLoading;

    public MainViewModel(BacenApiService bacenApiService)
    {
        _bacenApiService = bacenApiService ?? throw new ArgumentNullException(nameof(bacenApiService));
        Expectations = new ObservableCollection<object>(); // Alterado para tipo genérico
        Indicadores = new ObservableCollection<string> { "IPCA", "IGP-M", "Selic" };
        LoadDataCommand = new RelayCommandAsync(async () => await LoadDataAsync(), () => !IsLoading);
        ClearDataCommand = new RelayCommandAsync(async () => await ClearData());
        ExportDataCommand = new RelayCommandAsync(async () => await ExportDataAsync());
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        OnPropertyChanged(nameof(DadosCarregados));
    }

    public ObservableCollection<string> Indicadores { get; }

    public string SelectedIndicador
    {
        get { return _selectedIndicador; }
        set { _selectedIndicador = value; OnPropertyChanged(); }
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
        Expectations.Clear();

        if (SelectedIndicador == "Selic")
        {
            var data = await _bacenApiService.GetSelicExpectations(StartDate, EndDate);
            foreach (var item in data)
            {
                Expectations.Add(item);
            }
        }
        else
        {
            var data = await _bacenApiService.GetExpectations(SelectedIndicador, StartDate, EndDate);
            foreach (var item in data)
            {
                Expectations.Add(item);
            }
        }

        OnPropertyChanged(nameof(DadosCarregados));
        IsLoading = false;
    }

    private async Task ClearData()
    {
        SelectedIndicador = null;
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        Expectations.Clear();
        OnPropertyChanged(nameof(DadosCarregados));
    }

    private async Task ExportDataAsync()
    {
        var csvLines = new List<string>();

        // Add headers based on the type of data
        if (SelectedIndicador == "Selic")
        {
            csvLines.Add("Indicador,Data,Reuniao,Media,Mediana,DesvioPadrao,Minimo,Maximo,NumeroRespondentes,BaseCalculo");
            foreach (SelicExpectation item in Expectations)
            {
                csvLines.Add($"{item.Indicador},{item.Data},{item.Reuniao},{item.Media},{item.Mediana},{item.DesvioPadrao},{item.Minimo},{item.Maximo},{item.NumeroRespondentes},{item.BaseCalculo}");
            }
        }
        else
        {
            csvLines.Add("Indicador,Data,DataReferencia,Media,Mediana,DesvioPadrao,Minimo,Maximo,NumeroRespondentes,BaseCalculo");
            foreach (MarketExpectation item in Expectations)
            {
                csvLines.Add($"{item.Indicador},{item.Data},{item.DataReferencia},{item.Media},{item.Mediana},{item.DesvioPadrao},{item.Minimo},{item.Maximo},{item.NumeroRespondentes},{item.BaseCalculo}");
            }
        }

        // Prompt user to save the file
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
}
