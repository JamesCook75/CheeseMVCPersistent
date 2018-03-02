using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            if (context.Categories.Count() == 0)
            {
                CheeseCategory hard = new CheeseCategory();
                CheeseCategory soft = new CheeseCategory();

                hard.Name = "Hard";
                context.Categories.Add(hard);
                soft.Name = "Soft";
                context.Categories.Add(soft);
                context.SaveChanges();
            }

            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = 
                new AddCheeseViewModel(context.Categories.ToList());
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                CheeseCategory newCheeseCategory =
                    context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

                Cheese newCheese = new Cheese
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    Category = newCheeseCategory
                };

                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/Cheese");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        public IActionResult Category(int id)
        {
            if (id == 0) { return Redirect("/Category"); }

            CheeseCategory theCategory = context.Categories
                .Include(cat => cat.Cheeses)
                .Single(cat => cat.ID == id);

            ViewBag.title = "Cheeses in category: " + theCategory.Name;
            return View("Index", theCategory.Cheeses);
        }

        //TODO
        public IActionResult Edit(int id)
        {
            Cheese editCheese = context.Cheeses.Single(e => e.ID == id);
            EditAddCheeseViewModel editAddCheeseViewModel =
                 new EditAddCheeseViewModel(id, editCheese.Name, editCheese.Description);
            return View(editAddCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditAddCheeseViewModel editAddCheeseViewModel)
        {
            Cheese editCheese = context.Cheeses.Single(c => c.ID == editAddCheeseViewModel.CheeseId);
            editCheese.Name = editAddCheeseViewModel.Name;
            editCheese.Description = editAddCheeseViewModel.Description;
            
            context.SaveChanges();

            return Redirect("/");
        }
    }
}
