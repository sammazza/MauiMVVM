using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMVVM.Models
{
    // Not implement ObservableObject 
    // bc we are moving towards separation to use a MVVM model
    // so the binding will be elsewhere
    public class Toy
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public ToyType? Type { get; set; }
        public bool IsSecondHand { get; set; }
    }
}
