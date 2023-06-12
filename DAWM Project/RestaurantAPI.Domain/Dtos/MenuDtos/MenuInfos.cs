using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Dtos.MenuDtos
{
    public class MenuInfos
    {
        public int MenuId { get; set; }
        public string Name { get; set; }

        public float Price { get; set; }

        public List<int> RecipiesIds { get; set; }
    }
}
