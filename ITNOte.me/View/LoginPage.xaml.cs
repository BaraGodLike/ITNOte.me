using System.Net.Http;
using System.Windows.Controls;
using ITNOte.me.ModelView;

namespace ITNOte.me.View;

public partial class LoginPage : Page
{
    
    public LoginPage()
    {
        InitializeComponent();
        DataContext = new LoginModelView(new ApiService(new HttpClient { BaseAddress = new Uri("http://localhost:5019/api/") }));
    }
}