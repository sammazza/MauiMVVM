using Android.Graphics.Drawables;
using MauiMVVM.ViewModels;

namespace MauiMVVM.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        this.BindingContext = new LoginPageViewModel();

   
    }

    private void btnLogin_Clicked(object sender, EventArgs e)
    {

    }
}
