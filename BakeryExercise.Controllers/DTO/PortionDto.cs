using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryExercise.Controllers.DTO
{
    public class PortionDto
    {
        public Guid PortionId { get; set; }
        public Guid FoodId { get; set; }
        public double Amount { get; set; } //This datatype does not match the Portions.Amount datatype in EntityFramework.
    }
}
