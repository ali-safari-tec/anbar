using System.Collections.Generic;
using System.Threading.Tasks;
using anbar.Data.Dto;
using anbar.Data.Models;

namespace anbar.Data.Services
{
    public interface IDataBaseServices<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetId(int id);
        Task<T> Create(T entity);
        Task<bool> Delete(int id);
        Task<bool> DeleteInvoice(int id);
        Task<bool> DeleteBuyInvoice(int id);
        Task<bool> DeletePreInvoice(int id);
        Task<T> Update(int id, T entity);
        Task<IEnumerable<PersonDto>> GetSelectedPeople();
        Task<IEnumerable<ProductDto>> GetSelectedProducts();
        Task<IEnumerable<CostDto>> GetSelectedCost(int type, int year);
        Task<IEnumerable<InvoiceDto>> GetSelectedInvoicee(int type, int year);
        Task<IEnumerable<BuyInvoiceDto>> GetSelectedBuyInvoicee(int type, int year);
        Task<IEnumerable<PreInvoiceDto>> GetSelectedPreInvoicee(int year);
        Task<IEnumerable<Person>> SearchPeople(string searchText);
        Task<bool> ConvertProformaInvoiceAsync(int proformaInvoiceId);
        Task<IEnumerable<Product>> SearchProducts(string searchText);
        void Dispose();
    }
}
