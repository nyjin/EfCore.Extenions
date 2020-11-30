using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EfCore.Extensions.Models;
using EfCore.Extensions.WebApi.Services;

namespace EfCore.Extensions.WebApi.Profiles
{
    public class TodoProfile : Profile
    {
        private static readonly IEnumerable<string> TodoItemPropNames;

        static TodoProfile() => TodoItemPropNames = RuntimeHelper.GetPropertyNames(typeof(TodoItem));

        public TodoProfile()
        {
            CreateMap<TodoItemSettingDto, TodoItem>().ReverseMap();
            CreateMap<TodoItemSettingDto, IDictionary<string, object>>()
                .ConvertUsing(src => RuntimeHelper.ToDictionary(src)
                    .Where(x => !TodoItemPropNames.Contains(x.Key))
                    .ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
