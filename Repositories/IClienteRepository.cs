using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface IClienteRepository
    {
        Cliente GetCliente(int id);
        void AltaCliente(Cliente cliente);
        void EditarCliente(Cliente cliente);
    }
}