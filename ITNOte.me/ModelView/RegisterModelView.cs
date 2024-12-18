﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using ITNOte.me.Model;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using ITNOte.me.View;

namespace ITNOte.me.ModelView;

public partial class RegisterModelView : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
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

    public async Task<bool> IsCorrectName()
    {
        if (Nickname == null || Nickname.Length < 2 || 
            !NicknameRegex().IsMatch(Nickname) || BannedNames.bannedNames.Contains(Nickname)) return false;
        var user = await _apiService.GetAsync<UserDto?>($"users/{Nickname}");
        return user is null;
    }

    public bool IsCorrectPassword()
    {
        return Password != null && PasswordRegex().IsMatch(Password) && Password.Length >= 4;
    }
    

    private DelayCommand? _register;
    public DelayCommand RegisterUser
    {
        get
        {
            return _register ??= new DelayCommand(async obj =>
                {
                    if (!await IsCorrectName())
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
                        await _apiService.PostAsync($"users/register?name={Nickname}&password={Password}", new User());
                        ((MainWindow) Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!).
                            MainFrame.Navigate(new Uri("View/LoginPage.xaml", UriKind.Relative));
                        await Log.LogInformation(new User(Nickname, null), "register");
                        return;
                    }

                    MessageBox.Show("The passwords don't match");
                    PasswordRepeat = "";
                }
            );
        }
    }

    
    
    private DelayCommand? _toLogin;

    public RegisterModelView(ApiService apiService)
    {
        _apiService = apiService;
    }

    public DelayCommand BackToLogin
    {
        get
        {
            return _toLogin ??= new DelayCommand(async obj =>
                {
                    ((MainWindow) Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)!).
                        MainFrame.Navigate(new Uri("View/LoginPage.xaml", UriKind.Relative));
                }
            );
        }
    }
    

    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    private static partial Regex PasswordRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    private static partial Regex NicknameRegex();

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