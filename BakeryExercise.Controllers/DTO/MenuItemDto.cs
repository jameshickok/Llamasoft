namespace BakeryExercise.Controllers.DTO
{
    using System;

    public class MenuItemDto
    {
        public Guid MenuItemId { get; set; }
        public Guid MenuId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
    }
}
