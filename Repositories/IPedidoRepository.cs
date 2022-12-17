using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface IPedidoRepository
    {
        List<Pedido> GetPedidos();
        List<Pedido> GetPedidosByCadete(int idCadete);
        List<Pedido> GetPedidosByCliente(int idCliente);
        Pedido GetPedidoByNro(int nro);
        void AltaPedido(Pedido Pedido);
        void BajaPedido(int nro);
        void EditarPedido(Pedido Pedido);
        void CambiarEstado(int nro);
        void AsignarPedidoCadete(int nro, int id);
    }
}