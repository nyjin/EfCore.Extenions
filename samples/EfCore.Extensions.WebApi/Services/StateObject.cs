namespace EfCore.Extensions.WebApi.Services
{
    public class StateObject<T>
    {
        public StateObject(T item, ObjectStatus status = ObjectStatus.None)
        {
            Item = item;
            Status = status;
        }

        public T Item { get;set; }
        public ObjectStatus Status { get;set; }
    }
}