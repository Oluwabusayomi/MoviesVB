using System;
using System.Threading.Tasks;

namespace MoviesVB.Core.Data
{
    public interface IDataRepository<TDocument> : IReadonlyDataRepository<TDocument>
       where TDocument : IDocument
    {
        Task<Guid> AddAsync(TDocument document);
        Task UpsertAsync(TDocument document);


    }
}
