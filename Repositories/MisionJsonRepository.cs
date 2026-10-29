using System.Text.Json;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _ruta;

    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(
            environment.ContentRootPath,
            "Data",
            "misiones.json"
        );

        string? directorio = Path.GetDirectoryName(_ruta);

        if (directorio is not null)
        {
            Directory.CreateDirectory(directorio);
        }

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    private List<Mision> Leer()
    {
        string json = File.ReadAllText(_ruta);

        return JsonSerializer.Deserialize<List<Mision>>(json)
               ?? new List<Mision>();
    }

    private void Guardar(List<Mision> misiones)
    {
        string json = JsonSerializer.Serialize(misiones, _opciones);

        File.WriteAllText(_ruta, json);
    }

    public List<Mision> ObtenerTodos()
    {
        return Leer();
    }

    public Mision? ObtenerPorId(int id)
    {
        return Leer().FirstOrDefault(m => m.Id == id);
    }

    public Mision Agregar(Mision nuevaMision)
    {
        List<Mision> misiones = Leer();

        nuevaMision.Id = misiones.Count == 0
            ? 1
            : misiones.Max(m => m.Id) + 1;

        misiones.Add(nuevaMision);

        Guardar(misiones);

        return nuevaMision;
    }

    public bool Actualizar(int id, Mision misionActualizada)
    {
        List<Mision> misiones = Leer();

        Mision? existente = misiones.FirstOrDefault(m => m.Id == id);

        if (existente is null)
        {
            return false;
        }

        existente.Nombre = misionActualizada.Nombre;
        existente.Dificultad = misionActualizada.Dificultad;
        existente.RecompensaXP = misionActualizada.RecompensaXP;
        existente.Completada = misionActualizada.Completada;

        Guardar(misiones);

        return true;
    }

    public bool Eliminar(int id)
    {
        List<Mision> misiones = Leer();

        Mision? existente = misiones.FirstOrDefault(m => m.Id == id);

        if (existente is null)
        {
            return false;
        }

        misiones.Remove(existente);

        Guardar(misiones);

        return true;
    }
}