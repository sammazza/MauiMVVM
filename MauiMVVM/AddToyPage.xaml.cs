using Microsoft.Maui.Hosting;

namespace MauiMVVM;
using Models;
using System.Text.RegularExpressions;

public partial class AddToyPage : ContentPage
{
    public List<Toy> toys { get; set; } = new List<Toy>();
    private int numToys;
    public int NumToys
    {
        get
        {
            numToys = toys.Count;
            return numToys;
        }
    }
    Toy newToy;

    private string toyName;
    public string ToyName
    {
        get { return toyName; }
        set
        {
            if (toyName != value)
            {
                toyName = value;
                HandleError();
                OnPropertyChanged(nameof(ToyName));
                OnPropertyChanged(nameof(HasNameError));
            }
        }
    }
    private string errorNameMessage = string.Empty;

    public string ErrorNameMessage
    {
        get { return errorNameMessage; }
        set
        {
            if (errorNameMessage != value)
            {
                errorNameMessage = value;
                OnPropertyChanged(nameof(ErrorNameMessage));
                OnPropertyChanged(nameof(HasNameError));
                OnPropertyChanged(nameof(toyOK));
            }
        }
    }

    public bool HasNameError
    {
        get { return string.IsNullOrEmpty(toyName) || !ValidateToyName(); }

    }

    private double price;
    public double Price
    {
        get { return price; }
        set
        {
            if (price != value)
                price = Price;
            OnPropertyChanged(nameof(Price));
        }
    }
    private int selectedType;
    public int SelectedType
    {
        get { return selectedType; }
        set
        {
            if (selectedType != value)
                selectedType = SelectedType;
            OnPropertyChanged(nameof(SelectedType));
        }
    }

    public bool toyOK { get { return !HasNameError; } }

    public List<ToyType>? toyTypes { get; set; } = new List<ToyType>(); // it's one way bc the list won't change from the UI, no need for OnPropertyCHange

    public AddToyPage()
    {       
        toyTypes.Add(new ToyType() { Id = 1, Type = "Board Game" });
        toyTypes.Add(new ToyType() { Id = 2, Type = "Card Game" });
        toyTypes.Add(new ToyType() { Id = 3, Type = "Puzzle" });
        toyTypes.Add(new ToyType() { Id = 4, Type = "Strategy Game" });

        InitializeComponent();
        BindingContext = this;
    }

    ////
    private void HandleError()
    {
        ErrorNameMessage = string.Empty;
        if (!ValidateToyName())
        {
            ErrorNameMessage = "Toy name is too short or...";
        }
    }

    private bool ValidateToyName()
    {
        Regex regex = new Regex(@"[A-Z][a-zA-Z]{3,}");
        bool isOK = regex.IsMatch(ToyName);
        return isOK;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        bool isUsed = false;
        if (btn == btnUsedToy)
        {
            isUsed = true;
            this.Price *= 0.9;
        }
        newToy = new Toy() { Id = 1, Name = this.toyName, Price = this.Price, IsSecondHand = isUsed, Type = toyTypes[this.selectedType] };
        toys.Add(newToy);
        OnPropertyChanged(nameof(NumToys));
    }
}