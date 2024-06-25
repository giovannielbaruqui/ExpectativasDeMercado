using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class MainViewModel
{
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
}
