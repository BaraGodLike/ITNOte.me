using System.Windows;
using System.Windows.Controls;
using ITNOte.me.Model.Notes;
using ITNOte.me.ModelView;

namespace ITNOte.me.View;

public partial class RedactorPage : Page
{
    public RedactorPage()
    {
        InitializeComponent();
    }
    
    private async void TreeViewItem_Selected(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is AbstractSource selectedSource)
        {
            await ((RedactorModelView)DataContext).LoadNoteContent(selectedSource);
        }
    }

}