using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessAPI.Tests;

public class MockBookRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    private readonly Faker<TEntity> fakeEntity;

    #endregion

    #region Ctor

    public MockBookRepository()
    {
        fakeEntity = new Faker<TEntity>();
        //.RuleFor(o => o.Authors, f => { return new List<string>() { f.Name.FullName(), f.Name.FullName() }; })
        //.RuleFor(o => o.CoverPage, f => f.Image.LoremPixelUrl())
        //.RuleFor(o => o.Id, f => Guid.NewGuid());
    }

    #endregion

    #region Methods

    public Task<IList<TEntity>> GetBooksAsync(int limit = 10)
    {
        IList<TEntity> entity = fakeEntity.Generate(limit).ToList();

        return Task.FromResult(entity);
    }

    public Task<TEntity?> GetByIdAsync(Guid id)
    {
        _ = fakeEntity.RuleFor(o => o.Id, f => id);
        var entity = fakeEntity.Generate() ?? null;

        return Task.FromResult(entity);
    }

    public Task<bool> CreateAsync(TEntity entity)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        return Task.FromResult(true);
    }

    public Task<bool> UpdateAsync(TEntity entity)
    {
        return Task.FromResult(true);
    }

    #endregion
}