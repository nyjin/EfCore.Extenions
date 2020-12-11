using Ardalis.Specification;
using EfCore.Extensions.Models;
using Moq;
using Xunit;

namespace EfCore.Extensions.Tests
{
    public class RepositoryMoqTest
    {
        [Fact]
        public void Repository_Moq_Test()
        {
            var mockRepo = new Mock<IRepository<TodoItem>>();
            var repo = mockRepo.Object;
            repo.Add(new TodoItem());
            repo.AddAsync(new TodoItem());
            repo.Update(new TodoItem());
            repo.Save();
            repo.Detach(new TodoItem());
            repo.FirstOrDefaultAsync(It.IsAny<ISpecification<TodoItem>>());
            repo.GetAllAsync(It.IsAny<ISpecification<TodoItem>>());
            repo.Remove(new TodoItem());
            repo.Attach(new TodoItem());
            repo.GetRepository<User>();
        }
    }
}
