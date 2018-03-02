using CheeseMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class EditAddCheeseViewModel
    {
        public int CheeseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public EditAddCheeseViewModel() {}

        public EditAddCheeseViewModel(int cheeseId, string name, string description)
        {
            CheeseId = cheeseId;
            Name = name;
            Description = description;
        }
    }
}