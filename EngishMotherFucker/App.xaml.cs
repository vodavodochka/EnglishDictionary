using EngishMotherFucker.Database;
namespace EngishMotherFucker
{
    public partial class App : Application
    {
        public static WordDatabase Database { get; private set; }
        public App()
        {
            InitializeComponent();

            Database = new WordDatabase();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Database.Dispose();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new MainPage()));
        }
    }
}