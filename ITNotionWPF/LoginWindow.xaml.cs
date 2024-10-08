using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ITNotionWPF.Storage;
using ITNotionWPF.User;

namespace ITNotionWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{

    private readonly Storage.Storage _storage = new(new LocalRepository());
    
    public LoginWindow()
    {
        InitializeComponent();
    }

    private string? GetName()
    {
        return NicknameBox.Text;
    }

    private string? GetPassword()
    {
        return PasswordBox.Password;
    }

    private bool IsValidNickname(string? nick)
    {
        return nick != null && _storage.HasNicknameInStorage(nick);
    }

    private async Task<bool> IsValidPassword(string nick, string? password)
    {
        return password != null && 
               Storage.Storage.HashPassword(password).
                   Equals(await _storage.GetPasswordUser(nick));
    }

    private void IncorrectNickname()
    {
        MessageBox.Show("Incorrect Nickname");
        NicknameBox.Text = "";
    }

    private void IncorrectPassword()
    {
        MessageBox.Show("Incorrect Password");
        PasswordBox.Password = "";
    }
    
    private async void LoginUser(object sender, RoutedEventArgs routedEventArgs)
    {
        var nick = GetName();
        if (!IsValidNickname(nick))
        {
            IncorrectNickname();
            return;
        }
        
        var password = GetPassword();
        if (!await IsValidPassword(nick!, password))
        {
            IncorrectPassword();
            return;
        }

        await Log.LogInformation(new UserDto(new User.User(nick!, password)), "login");
    }
}