namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class ExecutionResultEntity<T>
    {
        public ExecutionResultEntity()
        {
        }

        public ExecutionResultEntity(T model)
        {
            data = model;
        }

        public T data { get; set; }

        public ExecutionErrorDetails error { get; set; }
    }
}