using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra.Microsoft.Client.Core;
using TerraSharp.Maui.Example.Models;

namespace TerraSharp.Maui.Example.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        public ObservableCollection<Log> Logs { get; set; } = new ObservableCollection<Log>();

        public ObservableCollection<Coin[]> Coins { get; set; } = new ObservableCollection<Coin[]>();
        public string _ApplicationName;
        IConnectivity connectivity;
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
