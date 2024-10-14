using System.Windows.Controls;
using ITNotionWPF.ModelView;

namespace ITNotionWPF.View;

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