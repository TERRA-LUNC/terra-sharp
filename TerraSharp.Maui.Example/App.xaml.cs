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
        }
    }
}