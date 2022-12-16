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
        Usuario Logear(Usuario Logeo);
        void AltaUsuario(Usuario usuario);
    }
}