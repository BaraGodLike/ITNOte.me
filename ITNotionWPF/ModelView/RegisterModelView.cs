using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using ITNotionWPF.Model;
using ITNotionWPF.Model.Storage;
using ITNotionWPF.Model.User;

namespace ITNotionWPF.ModelView;

public partial class RegisterModelView : INotifyPropertyChanged
{
    
    private string _nickname = "";
    private string _password = "";
    private string _passwordRepeat = "";

    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string PasswordRepeat
    {
        get => _passwordRepeat;
        set
        {
            _passwordRepeat = value;
            OnPropertyChanged();
        }
    }

    private bool IsCorrectName()
    {
        if (Nickname == null || Nickname.Length < 2) return false;
        return !Storage.RepoStorage.HasNicknameInStorage(Nickname);
    }

    private bool IsCorrectPassword()
    {
        return Password != null && MyRegex().IsMatch(Password) && Password.Length >= 4;
    }
    

    private DelayCommand _register;
    public DelayCommand RegisterUser
    {
        get
        {
            return _register ??= new DelayCommand(async obj =>
                {
                    if (!IsCorrectName())
                    {
                        MessageBox.Show("The nickname is occupied or null");
                        Nickname = "";
                        return;
                    }
        
                    if (!IsCorrectPassword())
                    {
                        MessageBox.Show("The password must be more than 3 characters long " +
                                        "\nand contain only Latin letters, numbers and underscore");
                        Password = "";
                        return;
                    }

                    if (PasswordRepeat.Equals(Password))
                    {
                        var userDto = new User(Nickname, Storage.HashPassword(Password));
                        await Storage.RepoStorage.SaveRegistryUser(userDto);
                        MessageBox.Show($"Welcome {Nickname}!");
                        await Log.LogInformation(userDto, "login");
                        return;
                    }

                    MessageBox.Show("The passwords don't match");
                    PasswordRepeat = "";
                }
            );
        }
    }

    
    
    private DelayCommand _toLogin;
    public DelayCommand BackToLogin
    {
        get
        {
            return _toLogin ??= new DelayCommand(async obj =>
                {
                    ((MainWindow) Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!).
                        MainFrame.NavigationService.Navigate(new Uri("View/LoginPage.xaml", UriKind.Relative));
                }
            );
        }
    }
    

    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    private static partial Regex MyRegex();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}