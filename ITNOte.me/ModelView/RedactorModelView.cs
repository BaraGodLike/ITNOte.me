using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using ITNOte.me.Model;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using MdXaml;
using WpfMath.Controls;

namespace ITNOte.me.ModelView;

public partial class RedactorModelView : INotifyPropertyChanged
{
    public IStorage _storage { get; init; } = Storage.RepoStorage; 
    private User User { get; }
    private ObservableCollection<AbstractSource> _folders;

    public ObservableCollection<AbstractSource> Folders
    {
        get => _folders;
        set
        {
            _folders = value;
            OnPropertyChanged();
        }
    }

    private string? _contentNote;
    public string? ContentNote
    {
        get => _contentNote;
        set
        {
            _contentNote = value;
            OnPropertyChanged();
        }
    }

    private Note? _curNote;

    public Note? CurNote
    {
        get => _curNote;
        set
        {
            _curNote = value;
            OnPropertyChanged();
        }
    }
    
    public string LogOutUsername => $"logout from {User.Name}";

    private bool _isNoteSelected;
    public bool IsNoteSelected
    {
        get => _isNoteSelected;
        set
        {
            _isNoteSelected = value;
            OnPropertyChanged();
        }
    }

    private string _nameOfNewSource = "Test";

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
        _folders = user.GeneralFolder.Children!;
        ContentNote = string.Empty;
        IsNoteSelected = true;
    }
    
    public async Task LoadNoteContent(AbstractSource selectedFile)
    {
        if (selectedFile is Note note)
        {
            CurNote = note;
            ContentNote = await _storage.ReadNote(note.Id, note.Name);
            
            IsNoteSelected = false;
        }
        else
        {
            IsNoteSelected = true;
        }
    }

    private async Task NewNoteContent(Note note)
    {
        CurNote = note;
        ContentNote = note.Content;
        IsNoteSelected = false;
    }
    
    private DelayCommand? _toLogin;
    public DelayCommand BackToLogin
    {
        get
        {
            return _toLogin ??= new DelayCommand(async obj =>
                {
                    await Log.LogInformation(User, "log out");
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
            return _newFolder ??= new DelayCommand(async obj =>
            {
                if (BannedNames.bannedNames.Contains(NameOfNewSource))
                {
                    MessageBox.Show("Unacceptable name");
                    NameOfNewSource = string.Empty;
                    return;
                }

                if (!NameRegex().IsMatch(NameOfNewSource))
                {
                    MessageBox.Show("Name can contains only latin letters, numbers and underscore");
                    return;
                }

                if (!IsValidNameOfSource())
                {
                    MessageBox.Show("The name is occupied");
                    NameOfNewSource = string.Empty;
                    return;
                }

                var folder = new Folder(NameOfNewSource, User.GeneralFolder);
                await _storage.SaveUser(User);
                await Log.LogInformation(User, $"created new Folder with name {NameOfNewSource}");
                NameOfNewSource = string.Empty;
            });
        }
    }

    private bool IsValidNameOfSource()
    {
        return User.GeneralFolder.Children!.All(i => !i.Name.Equals(NameOfNewSource));
    }
    
    private DelayCommand? _newNote;
    public DelayCommand NewNote
    {
        get
        {
            return _newNote ??= new DelayCommand(async obj =>
            {
                if (BannedNames.bannedNames.Contains(NameOfNewSource))
                {
                    MessageBox.Show("Unacceptable name");
                    NameOfNewSource = string.Empty;
                    return;
                }

                if (!NameRegex().IsMatch(NameOfNewSource))
                {
                    MessageBox.Show("Name can contains only latin letters, numbers and underscore");
                    return;
                }

                if (!IsValidNameOfSource())
                {
                    MessageBox.Show("The name is occupied");
                    NameOfNewSource = string.Empty;
                    return;
                }
                
                var note = new Note(NameOfNewSource, User.GeneralFolder);
                await NewNoteContent(note);
                await _storage.SaveUser(User);
                await Log.LogInformation(User, $"created new Note with name {NameOfNewSource}");
                NameOfNewSource = string.Empty;
            });
        }
    }
    
    private DelayCommand? _saveNoteCommand;

    public DelayCommand SaveNoteCommand
    {
        get
        {
            return _saveNoteCommand ??= new DelayCommand(async obj =>
            {
                if (CurNote != null)
                {
                    CurNote.Content = ContentNote!;
                    await _storage.WriteInNote(CurNote.Id, CurNote.Name, CurNote.Content!);
                    await Log.LogInformation(User, $"saved file {CurNote.Name}");
                }
            });
        }
    }
    
    
    public bool IsFolder(object obj) => obj is Folder;
    
    
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
    
    
    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    private static partial Regex NameRegex();
    
}