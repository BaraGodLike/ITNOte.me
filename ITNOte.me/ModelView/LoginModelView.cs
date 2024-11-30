using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows;
using ITNOte.me.Model;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using ITNOte.me.View;

namespace ITNOte.me.ModelView;

public class LoginModelView : INotifyPropertyChanged
{
    private ApiService _apiService;
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


    private async Task<bool> IsValidNickname()
    {
        return Nickname != null && await _apiService.GetAsync<UserDto?>($"users/{Nickname}") is not null;
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
                    var register = new RegisterPage
                    {
                        DataContext = new RegisterModelView(_apiService)
                    };
                    ((MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!)
                        .MainFrame.Navigate(register);
                }
            );
        }
    }


    private async Task<string?> LoginWithApi()
    {
        return await _apiService.PostAndGetToken("users/login", new UserDto
        {
            Name = Nickname!,
            Password = Password!
        });
    }
    

    private DelayCommand? _login;

    public LoginModelView(ApiService apiService)
    {
        _apiService = apiService;
    }

    public DelayCommand LoginUser
    {
        get
        {
            return _login ??= new DelayCommand(async obj =>
                {
                    if (!await IsValidNickname())
                    {
                        IncorrectNickname();
                        return;
                    }

                    var token = await LoginWithApi();
                    
                    if (token is null)
                    {
                        IncorrectPassword();
                        return;
                    } 
                    var userDto = await _apiService.GetAsync<UserDto>($"users/{Nickname}");
                    if (userDto is null)
                    {
                        MessageBox.Show("Oops, you're null");
                        return;
                    }
                    
                    var user = new User
                    {
                        Name = userDto.Name,
                        General_Folder_Id = userDto.General_Folder_Id,
                        GeneralFolder = new Folder(Nickname) {
                            Id = userDto.General_Folder_Id,
                            Children = await Storage.RepoStorage.GetAllChildren(userDto.General_Folder_Id)
                            }
                    };
                    var redactor = new RedactorPage
                    {
                        DataContext = new RedactorModelView(user, new ApiService(new HttpClient
                        {
                            BaseAddress = new Uri("http://localhost:5019/api/"), 
                            DefaultRequestHeaders =
                            {
                                Authorization = new AuthenticationHeaderValue("Bearer", token)
                            }
                        }))
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
