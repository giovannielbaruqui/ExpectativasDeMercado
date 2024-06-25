using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public partial class MainViewModel : INotifyPropertyChanged
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
}
