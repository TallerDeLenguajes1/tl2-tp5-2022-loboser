using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public interface IPedidoRepository
    {
        List<Pedido> GetPedidos();
        Pedido GetPedidoByNro(int nro);
        List<Pedido> GetPedidosByCadete(int idCadete);
        List<Pedido> GetPedidosByCliente(int idCliente);
        void AltaPedido(Pedido Pedido);
        void BajaPedido(int nro);
        void EditarPedido(Pedido Pedido);
    }
}