using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface IUsuarioRepository
    {
        List<Usuario> GetUsuarios();
        Usuario GetUsuario(Usuario Logeo);
        Usuario GetUsuarioByUser(string User);
        Usuario GetUsuarioById(int id);
        Usuario GetUsuarioByClienteId(int idCliente);
        Usuario GetUsuarioByCadeteId(int idCadete);
        void AltaUsuario(Usuario Usuario);
        void EditarUsuario(Usuario Usuario);
        void BajaUsuario(Usuario Usuario);
    }
}