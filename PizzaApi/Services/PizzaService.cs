
using PizzaApi.Models;

namespace PizzaApi.Services;

public class PizzaService
{
    private readonly List<Pizza> _pizzas =
    [
        new Pizza
        {
            Id = 1,
            Name = "Classic Margherita",
            Description = "Tomato, mozzarella, and fresh basil",
            Price = 10.99m,
            IsGlutenFree = false
        },
        new Pizza
        {
            Id = 2,
            Name = "Pepperoni",
            Description = "Tomato, mozzarella, and pepperoni",
            Price = 12.99m,
            IsGlutenFree = false
        },
        new Pizza
        {
            Id = 3,
            Name = "Vegetarian",
            Description = "Tomato, mozzarella, peppers, and mushrooms",
            Price = 11.99m,
            IsGlutenFree = false
        },
        new Pizza
        {
            Id = 4,
            Name = "BBQ Chicken Pizza",
            Description = "Barbecue sauce, chicken, red onions, and mozzarella",
            Price = 13.99m,
            IsGlutenFree = false
        }
    ];

    public List<Pizza> GetAll() => _pizzas;

    public Pizza? GetById(int id) =>
        _pizzas.FirstOrDefault(pizza => pizza.Id == id);

    public Pizza Create(Pizza pizza)
    {
        int nextId = _pizzas.Count == 0
            ? 1
            : _pizzas.Max(p => p.Id) + 1;

        pizza.Id = nextId;
        _pizzas.Add(pizza);

        return pizza;
    }

    public bool Update(int id, Pizza updatedPizza)
    {
        Pizza? existingPizza = GetById(id);

        if (existingPizza is null)
        {
            return false;
        }

        existingPizza.Name = updatedPizza.Name;
        existingPizza.Description = updatedPizza.Description;
        existingPizza.Price = updatedPizza.Price;
        existingPizza.IsGlutenFree = updatedPizza.IsGlutenFree;

        return true;
    }

    public bool Delete(int id)
    {
        Pizza? pizza = GetById(id);

        if (pizza is null)
        {
            return false;
        }

        _pizzas.Remove(pizza);
        return true;
    }
}
