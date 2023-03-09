using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using TerraSharp.Maui.Example.ViewModels.Models;

namespace TerraSharp.Maui.Example
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                mergedDictionaries.Add(new LightTheme());
                mergedDictionaries.Add(new SharedStyles());
            }

            MainPage = new AppShell();

            LiveCharts.Configure(config =>
               config
                   // registers SkiaSharp as the library backend
                   // REQUIRED unless you build your own
                   .AddSkiaSharp()

                   // adds the default supported types
                   // OPTIONAL but highly recommend
                   .AddDefaultMappers()

                   // select a theme, default is Light
                   // OPTIONAL
                   //.AddDarkTheme()
                   .AddLightTheme()

                   // finally register your own mappers
                   // you can learn more about mappers at:
                   // ToDo add website link...
                   //.HasMap<WalletTotal>((asset, point) =>
                   //{
                   //    point.PrimaryValue = (double)asset.Amount;
                   //    point.SecondaryValue = point.Context.Entity.EntityIndex;
                   //})
               // .HasMap<Foo>( .... )
               // .HasMap<Bar>( .... )
               );
        }
    }
}