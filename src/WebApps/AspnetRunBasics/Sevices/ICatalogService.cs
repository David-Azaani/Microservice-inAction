using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Model;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Sevices
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalog();
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
        Task<CatalogModel> GetCatalog(string id);
        Task<CatalogModel> CreateCatalog(CatalogModel model);
    }
}