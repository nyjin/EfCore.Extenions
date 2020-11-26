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
        private readonly ILogger<TodoController> _logger;
        private readonly IRepository<TodoItem> _repository;

        public TodoController(ILogger<TodoController> logger, IRepository<TodoItem> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public Task<List<TodoItem>> Get() => _repository.GetAllAsync();
    }
}
