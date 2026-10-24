
using Microsoft.AspNetCore.Mvc;
using PizzaApi.Models;
using PizzaApi.Services;

namespace PizzaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzasController : ControllerBase
{
    private readonly PizzaService _pizzaService;

    public PizzasController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll()
    {
        return Ok(_pizzaService.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Pizza> GetById(int id)
    {
        Pizza? pizza = _pizzaService.GetById(id);

        if (pizza is null)
        {
            return NotFound();
        }

        return Ok(pizza);
    }

    [HttpPost]
    public ActionResult<Pizza> Create(Pizza pizza)
    {
        Pizza createdPizza = _pizzaService.Create(pizza);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdPizza.Id },
            createdPizza);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        if (!_pizzaService.Update(id, pizza))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_pizzaService.Delete(id))
        {
            return NotFound();
        }

        return NoContent();
    }
}
