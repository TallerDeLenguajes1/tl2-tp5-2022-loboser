using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface ICadeteriaRepository
    {
        Cadeteria GetCadeteria();
        List<Cadete> GetCadetes();
        Cadete GetCadeteById(int id);
        void AltaCadete(Cadete Cadete);
        void BajaCadete(int id);
        void EditarCadete(Cadete Cadete);
    }
}