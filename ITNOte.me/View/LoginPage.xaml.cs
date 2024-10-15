using System.Windows.Controls;
using ITNOte.me.ModelView;

namespace ITNOte.me.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginPage : Page
{
    
    public LoginPage()
    {
        InitializeComponent();
        DataContext = new LoginModelView();
    }
}