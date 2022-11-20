#nullable disable

namespace tl2_tp4_2022_loboser.Repositories
{
    public class ConexionRepository : IConexionRepository
    {
        private readonly IConfiguration _configuration;
        //private IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        public ConexionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(){
            // return configuration.GetConnectionString("Default");
            return _configuration.GetConnectionString("Default");
        }
    }
}