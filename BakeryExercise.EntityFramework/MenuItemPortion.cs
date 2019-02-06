using System;
using System.Collections.Generic;
using System.Text;

namespace BakeryExercise.EntityFramework
{
    public class MenuItemPortion
    {
        public Guid MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public Guid PortionId { get; set; }
        public Portion Portion { get; set; }
    }
}
