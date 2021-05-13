using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using EfCore.Extensions.Models;

namespace EfCore.Extensions.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IRepository<TodoItem> _repository;

        public TodoController(IRepository<TodoItem> repository) => _repository = repository;

        [HttpGet]
        public Task<List<TodoItem>> Get() => _repository.GetAllAsync();
    }
}
