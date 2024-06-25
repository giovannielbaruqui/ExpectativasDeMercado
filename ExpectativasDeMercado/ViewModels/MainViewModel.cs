using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO; // Adicionar esta linha
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly BacenApiService _bacenApiService;
    private string _selectedIndicador;
    private string _selectedApiType;
    private DateTime _startDate;
    private DateTime _endDate;
    private ObservableCollection<object> _expectations;
    private bool _isLoading;

    public MainViewModel(BacenApiService bacenApiService)
    {
        _bacenApiService = bacenApiService ?? throw new ArgumentNullException(nameof(bacenApiService));
        Expectations = new ObservableCollection<object>();
        ApiTypes = new ObservableCollection<string> { "Expectativa Mercado Mensais", "Expectativa Mercado" };
        LoadDataCommand = new RelayCommandAsync(async () => await LoadDataAsync(), () => !IsLoading && (SelectedIndicador != null || SelectedApiType == "Expectativa Mercado"));
        ClearDataCommand = new RelayCommandAsync(async () => await ClearData(), () => Expectations.Any());
        ExportDataCommand = new RelayCommandAsync(async () => await ExportDataAsync(), () => Expectations.Any());
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        OnPropertyChanged(nameof(DadosCarregados));
    }

    public ObservableCollection<string> ApiTypes { get; }

    public ObservableCollection<string> Indicadores { get; private set; } = new ObservableCollection<string>();

    public string SelectedIndicador
    {
        get { return _selectedIndicador; }
        set
        {
            _selectedIndicador = value;
            OnPropertyChanged();
            LoadDataCommand.RaiseCanExecuteChanged();
        }
    }

    public string SelectedApiType
    {
        get { return _selectedApiType; }
        set
        {
            _selectedApiType = value;
            OnPropertyChanged();
            UpdateIndicadores();
            LoadDataCommand.RaiseCanExecuteChanged();
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
        Expectations.Clear();

        if (SelectedApiType == "Expectativa Mercado")
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

        ClearDataCommand.RaiseCanExecuteChanged();
        ExportDataCommand.RaiseCanExecuteChanged();
    }

    private async Task ClearData()
    {
        SelectedIndicador = null;
        Expectations.Clear();
        OnPropertyChanged(nameof(DadosCarregados));

        ClearDataCommand.RaiseCanExecuteChanged();
        ExportDataCommand.RaiseCanExecuteChanged();
    }

    private async Task ExportDataAsync()
    {
        var csvLines = new List<string>();

        if (SelectedApiType == "Expectativa Mercado")
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
