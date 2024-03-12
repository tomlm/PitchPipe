using Avalonia.Controls;
using PitchPipe.ViewModels;

namespace PitchPipe.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Note_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var button = (Button)sender!;
        var note = button.DataContext.ToString();
        ((MainViewModel)this.DataContext!).PlayNote(note);
    }

    private void ToggleSharp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ((MainViewModel)this.DataContext!).ToggleNotes();
    }

    private void ButtonSpinner_Spin(object? sender, Avalonia.Controls.SpinEventArgs e)
    {
        var buttonSpinner = (ButtonSpinner)sender!;
        var viewModel = (MainViewModel)this.DataContext!;
        viewModel.Octave += e.Direction == SpinDirection.Increase ? 1 : -1;
    }
}
