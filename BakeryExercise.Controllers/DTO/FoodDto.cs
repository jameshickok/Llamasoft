using System;

namespace BakeryExercise.Controllers.DTO
{
    public class FoodDto
    {
        public Guid FoodId { get; set; }
        public string Name { get; set; }
        public bool IsVegan { get; set; }
    }
}
