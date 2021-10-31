using MongoDB.Driver;
using MoviesVB.Core.Data;
using MoviesVB.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoviesVB.Data.Repositories
{
    internal abstract class DataRepository<TDocument> : IDataRepository<TDocument>, IDeleteDataRepository<TDocument>
        where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        protected DataRepository(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public async Task<Guid> AddAsync(TDocument document)
        {
            ArgumentGuard.NotNull(document, nameof(document));

            await _collection.InsertOneAsync(document);

            return document.Id;
        }

        public async Task DeleteAsync(Guid documentId, bool throwIfNotFound = true)
        {
            ArgumentGuard.NotEmpty(documentId, nameof(documentId));

            var result = await _collection.DeleteOneAsync(x => x.Id == documentId);

            if (result.IsAcknowledged && result.DeletedCount == 0 && throwIfNotFound)
            {
                throw new Exception($"No record found that matches the given Id {documentId}");
            }
        }

        public async Task<IList<TDocument>> FindAllAsync(Expression<Func<TDocument, bool>> filter)
        {
            ArgumentGuard.NotNull(filter, nameof(filter));

            var documents = await _collection.FindAsync(filter);

            using (documents)
            {
                return await documents.ToListAsync();
            }
        }

        public async Task<TDocument> GetAsync(Guid documentId, bool allowNull = false)
        {
            ArgumentGuard.NotEmpty(documentId, nameof(documentId));

            var documents = await _collection.FindAsync(u => u.Id == documentId);
            var document = await documents.FirstOrDefaultAsync();

            documents.Dispose();

            if (document == null && !allowNull)
            {
                throw new Exception($"No record found that matches the given Id {documentId}");
            }

            return document;
        }

        public async Task UpsertAsync(TDocument document)
        {
            ArgumentGuard.NotNull(document, nameof(document));

            var options = new FindOneAndReplaceOptions<TDocument, TDocument>
            {
                IsUpsert = true
            };

            await _collection.FindOneAndReplaceAsync<TDocument>(d => d.Id == document.Id, document, options);
        }

    }
}
