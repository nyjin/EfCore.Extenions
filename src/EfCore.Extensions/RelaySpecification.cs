using Ardalis.Specification;

namespace EfCore.Extensions;

public class RelaySpecification<T> : Specification<T>
{
    public ISpecificationBuilder<T> GetQuery() => Query;
}