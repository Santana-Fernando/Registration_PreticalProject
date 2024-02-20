using AutoMapper.Configuration;
using Domain.Login.Entities;
using Domain.Usuario.Entidades;
using Domain.Usuario.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Data.Repository.Usuario
{
    public class UsuariosRepository : IUsuario
    {
        private readonly ApplicationDbContext _context;
        public UsuariosRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Usuarios usuario)
        {
            _context.Add(usuario);
            _context.SaveChanges();
        }

        public async Task<Usuarios> GetById(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuarios> GetByEmail(string email)
        {
            return await _context.Usuarios.SingleOrDefaultAsync(u => u.email == email);
        }

        public async Task<IEnumerable<Usuarios>> GetList()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public void Update(Usuarios usuario)
        {
            _context.Update(usuario);
            _context.SaveChanges();
        }

        public void Remove(Usuarios usuario)
        {
            _context.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
