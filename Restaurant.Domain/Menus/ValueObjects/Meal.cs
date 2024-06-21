using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Menus.ValueObjects
{
    // should be different aggregate? or within restaurant
    // add menu (has resId) -> rest.addMenu(menuId) // but how to create it within single transaction? events
    // or
    // rest.add(menu) ??
    internal class Meal
    {
    }
}
