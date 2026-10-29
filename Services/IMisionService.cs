using DSW_3P_PR01.DTOs;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Services;

public interface IMisionService
{
    List<Mision> ObtenerTodos();
    Mision? ObtenerPorId(int id);
    Mision Agregar(CrearMisionDto nuevaMision);
    bool Actualizar(int id, Mision misionActualizada);
    bool Eliminar(int id);
}