using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BakeryExercise.EntityFramework
{
    /// <summary>
    /// Class Portion.
    /// </summary>
    public class Portion
    {
        public Guid PortionId { get; set; }
        public Food Food { get; set; }
        public int Amount { get; set; }
        public ICollection<MenuItemPortion> MenuItemPortions { get; set; }
    }
}
