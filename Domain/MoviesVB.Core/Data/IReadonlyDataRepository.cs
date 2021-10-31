using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoviesVB.Core.Data
{
    public interface IReadonlyDataRepository<TDocument> 
        where TDocument : IDocument
    {
        Task<TDocument> GetAsync(Guid documentId, bool allowNull = false);

        Task<IList<TDocument>> FindAllAsync(Expression<Func<TDocument, bool>> filter);

    }
}
