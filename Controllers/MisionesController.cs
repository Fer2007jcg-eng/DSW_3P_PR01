using DSW_3P_PR01.DTOs;
using DSW_3P_PR01.Models;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/misiones")]
public class MisionesController : ControllerBase
{
    private readonly IMisionService _service;

    public MisionesController(IMisionService service)
    {
        _service = service;
    }

    // GET: api/misiones
    [HttpGet]
    [ProducesResponseType(
        typeof(List<Mision>),
        StatusCodes.Status200OK
    )]
    public IActionResult ObtenerTodos()
    {
        return Ok(_service.ObtenerTodos());
    }

    // GET: api/misiones/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(Mision),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        Mision? mision = _service.ObtenerPorId(id);

        if (mision is null)
        {
            return NotFound(new
            {
                mensaje = $"No existe una misión con el ID {id}."
            });
        }

        return Ok(mision);
    }

    // POST: api/misiones
    [HttpPost]
    [ProducesResponseType(
        typeof(Mision),
        StatusCodes.Status201Created
    )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Agregar(
        [FromBody] CrearMisionDto nuevaMision
    )
    {
        try
        {
            Mision creada = _service.Agregar(nuevaMision);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creada.Id },
                creada
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PUT: api/misiones/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(
        int id,
        [FromBody] Mision misionActualizada
    )
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "El ID debe ser un entero positivo."
                });
            }

            bool actualizado = _service.Actualizar(
                id,
                misionActualizada
            );

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = $"No existe una misión con el ID {id}."
                });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // DELETE: api/misiones/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El ID debe ser un entero positivo."
            });
        }

        bool eliminado = _service.Eliminar(id);

        if (!eliminado)
        {
            return NotFound(new
            {
                mensaje = $"No existe una misión con el ID {id}."
            });
        }

        return NoContent();
    }
}