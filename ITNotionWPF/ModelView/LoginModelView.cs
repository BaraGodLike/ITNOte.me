using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ITNotionWPF.Model;
using ITNotionWPF.Model.Storage;
using ITNotionWPF.Model.User;

namespace ITNotionWPF.ModelView;

public class LoginModelView : INotifyPropertyChanged
{
    private string _nickname = "";
    private string _password = "";

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


    private bool IsValidNickname()
    {
        return Nickname != null && Storage.RepoStorage.HasNicknameInStorage(Nickname);
    }

    private async Task<bool> IsValidPassword()
    {
        return Password != null && 
               Storage.HashPassword(Password).
                   Equals((await Storage.RepoStorage.GetUserFromStorage<UserDto>(Nickname))!.User.Password);
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
    
    private DelayCommand _goReg;
    public DelayCommand GoToRegister
    {
        get
        {
            return _goReg ??= new DelayCommand(async obj =>
                {
                    ((MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!)
                        .MainFrame
                        .NavigationService.Navigate(new Uri("View/RegisterPage.xaml", UriKind.Relative));
                }
            );
        }
    }

    private DelayCommand _login;
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

                    MessageBox.Show($"Добро пожаловать, {Nickname}!");
                    await Log.LogInformation(new UserDto(new User(Nickname, Password)), "login");
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
