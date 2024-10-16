using System.Windows.Controls;
using ITNOte.me.ModelView;

namespace ITNOte.me.View;

public partial class LoginPage : Page
{
    
    public LoginPage()
    {
        InitializeComponent();
        DataContext = new LoginModelView();
    }
}