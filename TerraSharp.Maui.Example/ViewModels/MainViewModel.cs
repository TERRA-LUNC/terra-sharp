using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra.Microsoft.Client.Core;
using TerraSharp.Maui.Example.Models;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using TerraSharp.Maui.Example.ViewModels.Models;

namespace TerraSharp.Maui.Example.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Log> Logs { get; set; } = new ObservableCollection<Log>();
        public ObservableCollection<Coin[]> Coins { get; set; } = new ObservableCollection<Coin[]>();

        public IEnumerable<ISeries> Series { get; set; } = new GaugeBuilder()
            .WithLabelsPosition(PolarLabelsPosition.Start)
            .WithLabelFormatter(point => point.PrimaryValue + " " + point.Context.Series.Name)
            .WithLabelsSize(20)
            .WithInnerRadius(20)
            .WithOffsetRadius(8)
            .WithBackgroundInnerRadius(20)
            //.AddValue(100, "*")
            .AddValue(39.5, "USTC")
            .AddValue(923.78, "LUNC")
            .BuildSeries();

        public string _ApplicationName;

        public MainViewModel()
        {
        }
        public string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
                OnNotifyPropertyChanged(nameof(ApplicationName));
            }
        }

        [RelayCommand]
        void Delete(Log s)
        {
            if (Logs.Contains(s))
            {
                Logs.Remove(s);
            }
        }
    }



    //private void LoadSettings()
    //{
    //    //https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/secure-storage?view=net-maui-7.0&tabs=windows
    //    Task.Run(async () =>
    //    {
    //        applicationName = await SecureStorage.Default.GetAsync(Settings.ApplicationName);
    //    });

    //}
}
