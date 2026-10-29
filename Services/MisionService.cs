using DSW_3P_PR01.DTOs;
using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repository;

    private static readonly string[] DificultadesPermitidas =
    {
        "Fácil",
        "Media",
        "Difícil"
    };

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    private static void ValidarNombre(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException(
                "El nombre de la misión es obligatorio."
            );
        }
    }

    private static string ValidarDificultad(string? dificultad)
    {
        if (string.IsNullOrWhiteSpace(dificultad))
        {
            throw new ArgumentException(
                "La dificultad es obligatoria. Los valores permitidos son Fácil, Media y Difícil."
            );
        }

        string? dificultadValida = DificultadesPermitidas.FirstOrDefault(
            d => d.Equals(
                dificultad.Trim(),
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (dificultadValida is null)
        {
            throw new ArgumentException(
                "La dificultad debe ser Fácil, Media o Difícil."
            );
        }

        return dificultadValida;
    }

    private static void ValidarRecompensa(int recompensaXP)
    {
        if (recompensaXP < 0)
        {
            throw new ArgumentException(
                "La recompensa XP no puede ser negativa."
            );
        }
    }

    private static void ValidarMision(Mision mision)
    {
        ValidarNombre(mision.Nombre);

        mision.Nombre = mision.Nombre.Trim();

        mision.Dificultad = ValidarDificultad(mision.Dificultad);

        ValidarRecompensa(mision.RecompensaXP);
    }

    public List<Mision> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Mision? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Mision Agregar(CrearMisionDto nuevaMision)
    {
        ArgumentNullException.ThrowIfNull(nuevaMision);

        ValidarNombre(nuevaMision.Nombre);

        string nombre = nuevaMision.Nombre.Trim();

        string dificultad = ValidarDificultad(
            nuevaMision.Dificultad
        );

        ValidarRecompensa(nuevaMision.RecompensaXP);

        Mision mision = new()
        {
            Nombre = nombre,
            Dificultad = dificultad,
            RecompensaXP = nuevaMision.RecompensaXP,
            Completada = false
        };

        return _repository.Agregar(mision);
    }

    public bool Actualizar(int id, Mision misionActualizada)
    {
        ArgumentNullException.ThrowIfNull(misionActualizada);

        ValidarMision(misionActualizada);

        return _repository.Actualizar(id, misionActualizada);
    }

    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }
}