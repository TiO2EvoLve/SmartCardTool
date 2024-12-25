namespace WindowUI.Pages;

public class Page2ViewModel : BindableBase
{
    private string _input;
    public string Input
    {
        get => _input;
        set => SetProperty(ref _input, value);
    }
}