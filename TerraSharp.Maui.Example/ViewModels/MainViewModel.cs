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

namespace TerraSharp.Maui.Example.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Log> Logs { get; set; } = new ObservableCollection<Log>();
        public ObservableCollection<Coin[]> Coins { get; set; } = new ObservableCollection<Coin[]>();
        public ObservableValue ObservableValueLunc { get; set; } = new ObservableValue { Value = 0 };
        public ObservableValue ObservableValueStaked { get; set; } = new ObservableValue { Value = 0 };

        public ObservableValue ObservableValueTotal { get; set; } = new ObservableValue { Value = 0 };

        public IEnumerable<ISeries> Series { get; set; } 

        public string _ApplicationName;
        public string _CurrentAddress;
        public string _CurrentEnvironnement;
        public string _Url;
        public MainViewModel()
        {
            //ObservableValueG = new ObservableValue { Value = 50 };
            Series = new GaugeBuilder()
                .WithLabelsSize(20)
                .WithLabelsPosition(PolarLabelsPosition.Start)
                .WithLabelFormatter(point => $"{point.PrimaryValue} {point.Context.Series.Name}")
                .WithInnerRadius(20)
                .WithOffsetRadius(8)
                .WithBackgroundInnerRadius(20)
                .WithLabelsPosition(PolarLabelsPosition.Start)
                .AddValue(ObservableValueLunc, "Lunc")
                .AddValue(ObservableValueStaked, "Staked")
                .AddValue(ObservableValueTotal, "Total")
                .BuildSeries();
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

        public string CurrentAddress
        {
            get
            {
                return _CurrentAddress;
            }
            set
            {
                _CurrentAddress = value;
                OnNotifyPropertyChanged(nameof(CurrentAddress));
            }
        }

        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
                OnNotifyPropertyChanged(nameof(Url));
            }
        }

        public string CurrentEnvironnement
        {
            get
            {
                return _CurrentEnvironnement;
            }
            set
            {
                _CurrentEnvironnement = value;
                OnNotifyPropertyChanged(nameof(CurrentEnvironnement));
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
