namespace Models.Apis;

public class ResponseModel<T> : ResponseModelBase
{
    public T? Data { get; set; }
}