using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using AutoMapper;
using EfCore.Extensions.Models;

namespace EfCore.Extensions.WebApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TodoItemSettings> _todoSettingRepository;

        public TodoService(IMapper mapper, IRepository<TodoItemSettings> todoSettingRepository)
        {
            _mapper = mapper;
            _todoSettingRepository = todoSettingRepository;
        }

        public async Task<int> AddOrUpdatePropsAsync(int todoItemId, TodoItemSettingDto settingDto)
        {
            var todoSettings = _mapper.Map<IDictionary<string, object>>(settingDto);

            var settings = await _todoSettingRepository.GetAllAsync(x => x.Where(y => y.TodoItemId == todoItemId));
            var right = settings.ToDictionary(x => x.SettingKey, x => new StateObject<TodoItemSettings>(x));

            foreach(var setting in todoSettings)
            {
                if(right.ContainsKey(setting.Key))
                {
                    var subject = right[setting.Key].Item;
                    var leftValue = setting.Value?.ToString();

                    if(!string.Equals(leftValue, subject.SettingValue, StringComparison.OrdinalIgnoreCase))
                    {
                        right[setting.Key].Status = ObjectStatus.Updated;
                        subject.SettingValue = leftValue;
                    }
                }
                else
                {
                    var subject = new TodoItemSettings
                    {
                        SettingKey = setting.Key
                    };

                    right[setting.Key] = new StateObject<TodoItemSettings>(subject, ObjectStatus.Added);
                    subject.SettingType = setting.GetType().ToString();
                    subject.SettingValue = setting.Value?.ToString();
                    subject.TodoItemId = todoItemId;
                }
            }

            foreach(var r in right.Values)
            {
                switch (r.Status)
                {
                    case ObjectStatus.Added:
                        _todoSettingRepository.Add(r.Item);
                        break;
                    case ObjectStatus.Updated:
                        _todoSettingRepository.Update(r.Item);
                        break;
                    case ObjectStatus.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return await _todoSettingRepository.SaveAsync();
        }
    }
}
