using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface IPedidoRepository
    {
        List<Pedido> GetPedidos();
        List<Pedido> GetPedidosByCadete(int idCadete);
        void AltaPedido(AltaPedidoViewModel Pedido);
        void BajaPedido(int nro);
        void EditarPedido(EditarPedidoViewModel Pedido);
        void AsignarPedidoCadete(int nro, int id);
    }
}