using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ITNotionWPF.Model;
using ITNotionWPF.Model.Storage;
using ITNotionWPF.Model.User;
using ITNotionWPF.ModelView;

namespace ITNotionWPF.View;

public partial class RegisterPage : Page
{
    public RegisterPage()
    {
        InitializeComponent();
        DataContext = new RegisterModelView();
    }
    
}