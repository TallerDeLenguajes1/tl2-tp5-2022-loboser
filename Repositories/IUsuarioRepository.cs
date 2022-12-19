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
        Usuario GetUsuario(Usuario Logeo);
        Usuario GetUsuarioLikeUser(string User);
        void AltaUsuario(Usuario Usuario);
        void BajaUsuario(Usuario Usuario);
    }
}