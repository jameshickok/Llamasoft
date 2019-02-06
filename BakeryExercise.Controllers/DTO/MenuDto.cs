namespace BakeryExercise.Controllers.DTO
{
    using System;

    public class MenuDto
    {
        public MenuDto()
        {
        }

        public Guid MenuId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
