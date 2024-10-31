using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ITNOte.me.Model;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using ITNOte.me.View;

namespace ITNOte.me.ModelView;

public class LoginModelView : INotifyPropertyChanged
{
    public IStorage _storage { get; init; } = Model.Storage.Storage.RepoStorage;
    private string? _nickname = "";
    private string? _password = "";

    public string? Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged();
        }
    }

    public string? Password
    {
        get => _password;
        set {
            _password = value;
            OnPropertyChanged();
        }
    }


    private bool IsValidNickname()
    {
        return Nickname != null && _storage.HasNicknameInStorage(Nickname);
    }

    private async Task<bool> IsValidPassword()
    {
        return Password != null &&
               Storage.Hasher.VerifyHashedPassword(Password,
                   (await _storage.GetUserFromStorage<User>(Nickname!))!.Password);
    }

    private void IncorrectNickname()
    {
        MessageBox.Show("Incorrect Nickname");
        Nickname = "";
    }

    private void IncorrectPassword()
    {
        MessageBox.Show("Incorrect Password");
        Password = "";
    }
    
    private DelayCommand? _goReg;
    public DelayCommand GoToRegister
    {
        get
        {
            return _goReg ??= new DelayCommand(async obj =>
                {
                    ((MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!)
                        .MainFrame.Navigate(new Uri("View/RegisterPage.xaml", UriKind.Relative));
                }
            );
        }
    }

    private DelayCommand? _login;
    public DelayCommand LoginUser
    {
        get
        {
            return _login ??= new DelayCommand(async obj =>
                {
                    if (!IsValidNickname())
                    {
                        IncorrectNickname();
                        return;
                    }
                    if (!await IsValidPassword())
                    {
                        IncorrectPassword();
                        return;
                    }

                    var user = await _storage.GetUserFromStorage<User>(Nickname!);
                    var redactor = new RedactorPage
                    {
                        DataContext = new RedactorModelView(user!)
                    };

                    ((MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!)
                        .MainFrame.Navigate(redactor);
                    await Log.LogInformation(user, "login");
                }
            );
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
