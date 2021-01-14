﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AppTFG.Datos;

namespace AppTFG.Servicios
{
    class ServicioBaseDatos<T> : IServicioBaseDatos<T> where T : class
    {
        BaseDatos bd;

        public ServicioBaseDatos()
        {
            bd = App.BD;
        }

        public virtual async Task<List<T>> ObtenerTabla()
        {
            return await bd.Set<T>().ToListAsync();
        }

        public virtual async Task<T> BuscarPorId(int id)
        {
            return await bd.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> Agregar(T dato)
        {
            await bd.Set<T>().AddAsync(dato);
            await bd.SaveChangesAsync();
            return dato;
        }

        public virtual async Task<T> Actualizar(T dato)
        {
            bd.Set<T>().Update(dato);
            await bd.SaveChangesAsync();
            return dato;
        }

        public virtual async Task<bool> Eliminar(int id)
        {
            var entity = await BuscarPorId(id);
            bd.Set<T>().Remove(entity);
            await bd.SaveChangesAsync();
            return true;
        }
    }
}