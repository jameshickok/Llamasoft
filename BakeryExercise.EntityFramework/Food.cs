using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BakeryExercise.EntityFramework
{
    public class Food
    {
        public Guid FoodId { get; set; }
        public string Name { get; set; }
        public bool IsVegan { get; set; }
    }
}
