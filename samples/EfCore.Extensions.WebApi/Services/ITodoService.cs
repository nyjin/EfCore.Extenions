using System.Threading.Tasks;

namespace EfCore.Extensions.WebApi.Services
{
    public interface ITodoService
    {
        Task<int> AddOrUpdatePropsAsync(int todoItemId, TodoItemSettingDto settingDto);
    }
}