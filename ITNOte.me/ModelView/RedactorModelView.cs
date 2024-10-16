using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;

namespace ITNOte.me.ModelView;

public class RedactorModelView : INotifyPropertyChanged
{
    private User User { get; }
    public ObservableCollection<AbstractSource> Folders { get; }
    
    private string _currentNote;
    public string CurrentNote
    {
        get => _currentNote;
        set => SetField(ref _currentNote, value);
    }

    public string LogOutUsername => $"logout from {User.Name}";

    private bool _isNoteSelected;
    public bool IsNoteSelected
    {
        get => _isNoteSelected;
        set => SetField(ref _isNoteSelected, value);
    }

    private string _nameOfNewSource = string.Empty;

    public string NameOfNewSource
    {
        get => _nameOfNewSource;
        set
        {
            _nameOfNewSource = value;
            OnPropertyChanged();
        }
    }
    
    public RedactorModelView(User user)
    {
        User = user;
        Folders = user.GeneralFolder.Children;
        CurrentNote = string.Empty;
        IsNoteSelected = true;
    }
    
    public async Task LoadNoteContent(AbstractSource selectedFile)
    {
        if (selectedFile is Note note)
        {
            CurrentNote = await note.GetText();
            IsNoteSelected = true;
        }
        else
        {
            CurrentNote = string.Empty;
            IsNoteSelected = false;
        }
    }
    
    private DelayCommand? _toLogin;
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

    private DelayCommand? _newFolder;

    public DelayCommand NewFolder
    {
        get
        {
            Console.Write("Hello World");
            return _newFolder ??= new DelayCommand(async obj =>
            {
                if (NameOfNewSource.Equals("") || NameOfNewSource.Contains(' '))
                {
                    MessageBox.Show("Source can't be null or named with SPACE");
                    return;
                }
                Console.Write("Создал папку");
                Folders.Add(new Folder(NameOfNewSource, User.GeneralFolder));
                NameOfNewSource = string.Empty;
            });
        }
    }
    
    private DelayCommand? _newNote;

    public DelayCommand NewNote
    {
        get
        {
            Console.Write(_newNote is null);
            return _newNote ??= new DelayCommand(async obj =>
            {
                if (NameOfNewSource.Equals("") || NameOfNewSource.Contains(' '))
                {
                    MessageBox.Show("Source can't be null or named with SPACE");
                    return;
                }
                Console.Write("Создал файл");
                var note = new Note(NameOfNewSource, User.GeneralFolder);
                Folders.Add(note);
                await LoadNoteContent(note);
                NameOfNewSource = string.Empty;
            });
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