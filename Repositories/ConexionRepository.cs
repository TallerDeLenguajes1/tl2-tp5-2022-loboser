#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public class ConexionRepository : IConexionRepository
    {
        private readonly IConfiguration _configuration;
        public ConexionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(){
            return _configuration.GetConnectionString("Default");
        }
    }
}