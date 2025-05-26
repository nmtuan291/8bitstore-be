using Neo4j.Driver;

namespace _8bitstore_be.Services
{
    public class Neo4jService
    {
        private readonly IDriver _driver;

        public Neo4jService(IDriver driver)
        {
            _driver = driver;
        }
    }
}
