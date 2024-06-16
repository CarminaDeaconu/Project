using Microsoft.Maui.Controls;
using Project.Views;

namespace Project
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }
    }
}
