using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface ICadeteriaRepository
    {
        Cadeteria GetCadeteria();
        void AltaCadete(AltaCadeteViewModel Cadete);
        void BajaCadete(int id);
        void EditarCadete(EditarCadeteViewModel Cadete);
    }
}