using System;
using System.Threading.Tasks;

namespace MoviesVB.Core.Data
{
    public interface IDeleteDataRepository<TDocument>
    {
        Task DeleteAsync(Guid documentId, bool throwIfNotFound = true);
    }
}
