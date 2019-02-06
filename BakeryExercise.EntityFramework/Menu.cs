using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeryExercise.EntityFramework
{
    public class Menu
    {
        public Guid MenuId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public ICollection<MenuItem> Items { get; set; }
    }
}
