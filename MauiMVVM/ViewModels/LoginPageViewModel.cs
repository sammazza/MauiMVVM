//using AndroidX.Lifecycle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiMVVM.ViewModels
{
    public class BoolToEyeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Visibility_off" : "Visibility";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    class LoginPageViewModel : ViewModel
    {
        private string? loginStatus = string.Empty;
        public string LoginStatus
        {
            get { return loginStatus; }
            set { loginStatus = value; }
        }

        private string? email = string.Empty;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string? password = string.Empty;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string? msgErrorEmail = string.Empty;
        public string MsgErrorEmail
        {
            get { return msgErrorEmail; }
            set { msgErrorEmail = value; }
        }

        private string? msgErrorPassword = string.Empty ;
        public string MsgErrorPassword
        {
            get { return msgErrorPassword; }
            set { msgErrorPassword = value; }
        }
        private bool fieldsOK = true;
        public bool FieldsOK
        {
            get { return fieldsOK; }
            set { fieldsOK = value; }
        }

        private bool isCancellable = true;
        public bool IsCancellable
        {
            get { return isCancellable; }
            set { isCancellable = value; }
        }

        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand PasswordChangeCommand { get; }
        public ICommand EmailChangedCommand { get; }

        private bool isPasswordHidden = true;
        public bool IsPasswordHidden
        {
            get { return isPasswordHidden; }
            set { isPasswordHidden = value; }
        }

        public LoginPageViewModel()
        {
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
            LoginCommand = new Command(Login);
            CancelCommand = new Command(ClearForm);
            EmailChangedCommand = new Command<string>(ValidateEmail);
            PasswordChangeCommand = new Command<string>(ValidatePassword);
        }

        //void OnEntryEmailChanged(string value)
        //{
        //    // Notify that the command's CanExecute state might have changed
        //    (EmailChangedCommand as Command<string>)?.NotifyCanExecuteChanged();
        //}

        private void TogglePasswordVisibility()
        {
            isPasswordHidden = !isPasswordHidden;
            OnPropertyChanged(nameof(IsPasswordHidden));
        }

        public void ValidatePassword(string text)
        {
            Regex regex = new("(?=.*[A-Z])(?=.*\\d).+"); // from regex101.com
            msgErrorPassword = string.Empty;
            if (regex.IsMatch(text))
            {
                msgErrorPassword = "Password must include a CAPITAL letter and a digit";
            }
            OnPropertyChanged(MsgErrorPassword);
        }

        public void ValidateEmail(string text)
        {
            Regex regex = new Regex(@"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");
            if (regex.IsMatch(text))
            {
                msgErrorPassword = "Invalid Email address";
            }
            OnPropertyChanged(MsgErrorEmail);
        }

        private void Login()
        {
            if ("sam@gmail.com".Equals(email) && "A123".Equals(password))
                loginStatus = "Login success";
            else
                loginStatus = "invalid credentials";
            OnPropertyChanged(nameof (LoginStatus));
        }

        private void ClearForm()
        {
            email=string.Empty;
            password = string.Empty;
            fieldsOK = false;
            isCancellable = false;

            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(FieldsOK));
            OnPropertyChanged(nameof(IsCancellable));
        }
    }

    public class EventToCommandBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior), null);

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            var eventInfo = bindable.GetType().GetEvent(EventName);
            if (eventInfo != null)
            {
                eventInfo.AddEventHandler(bindable, new EventHandler(OnEventRaised));
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            var eventInfo = bindable.GetType().GetEvent(EventName);
            if (eventInfo != null)
            {
                eventInfo.RemoveEventHandler(bindable, new EventHandler(OnEventRaised));
            }
        }

        private void OnEventRaised(object sender, EventArgs e)
        {
            if (Command == null) return;

            var parameter = CommandParameter ?? e;
            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }


    //public class TextChangedBehavior : Behavior<Entry>
    //{
    //    public static readonly BindableProperty CommandProperty =
    //        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TextChangedBehavior));

    //    public ICommand Command
    //    {
    //        get => (ICommand)GetValue(CommandProperty);
    //        set => SetValue(CommandProperty, value);
    //    }

    //    protected override void OnAttachedTo(Entry bindable)
    //    {
    //        base.OnAttachedTo(bindable);
    //        bindable.TextChanged += OnTextChanged;
    //    }

    //    protected override void OnDetachingFrom(Entry bindable)
    //    {
    //        base.OnDetachingFrom(bindable);
    //        bindable.TextChanged -= OnTextChanged;
    //    }

    //    private void OnTextChanged(object sender, TextChangedEventArgs e)
    //    {
    //        //if (Command?.CanExecute(e.NewTextValue) == true)
    //        {
    //            Command.Execute(e.NewTextValue);
    //        }
    //    }
    //}
}
