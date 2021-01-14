﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppTFG.Servicios
{
    interface IServicioBaseDatos<T> where T : class
    {
        Task<List<T>> ObtenerTabla();
        Task<T> BuscarPorId(int id);
        Task<T> Agregar(T dato);
        Task<T> Actualizar(T dato);
        Task<bool> Eliminar(int dato);
    }
}

