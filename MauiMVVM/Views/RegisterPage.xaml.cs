using MauiMVVM.ViewModels;

namespace MauiMVVM.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        this.BindingContext = new RegisterPageViewModel();
    }
}