using System;
using AutoMapper;
using EfCore.Extensions.Models;

namespace EfCore.Extensions.WebApi.Services
{

    public interface ITransferResult
    {

    }

    public class TodoItemSettingDto : ITransferResult
    {
        public string Name { get;set; }
        public bool IsCompleted { get;set; }
        public string Description { get;set; }
        public DateTime? ExpireDate { get;set; }
    }
}