using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    private BacenApiService _bacenApiService;
    private string _selectedIndicador;
    private DateTime _startDate;
    private DateTime _endDate;
    private ObservableCollection<MarketExpectation> _expectations;
    private bool _isLoading;

    public MainViewModel()
    {
        _bacenApiService = new BacenApiService();
        Expectations = new ObservableCollection<MarketExpectation>();
        Indicadores = new ObservableCollection<string> { "IPCA", "IGP-M", "Selic" };
        LoadDataCommand = new RelayCommand(async () => await LoadData());
        ClearDataCommand = new RelayCommand(ClearData);
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        // Assumindo que DadosCarregados também precisa ser inicializado
        OnPropertyChanged(nameof(DadosCarregados));
    }

    public event PropertyChangedEventHandler PropertyChanged;

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

    public ObservableCollection<MarketExpectation> Expectations
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

    public ICommand LoadDataCommand { get; }
    public ICommand ClearDataCommand { get; }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private async Task LoadData()
    {
        IsLoading = true;
        var data = await _bacenApiService.GetMarketExpectations(SelectedIndicador, StartDate, EndDate);
        Expectations.Clear();
        foreach (var item in data)
        {
            Expectations.Add(item);
        }
        OnPropertyChanged(nameof(DadosCarregados));
        IsLoading = false;
    }

    private void ClearData()
    {
        SelectedIndicador = null;
        StartDate = DateTime.Now.AddMonths(-1);
        EndDate = DateTime.Now;
        Expectations.Clear();
        OnPropertyChanged(nameof(DadosCarregados));
    }
}
