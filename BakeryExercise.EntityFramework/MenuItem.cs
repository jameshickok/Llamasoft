using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BakeryExercise.EntityFramework
{
    public class MenuItem
    {
        public Guid MenuItemId { get; set; }
        public Menu Menu { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ICollection<MenuItemPortion> MenuItemPortions { get; set; }
    }
}
