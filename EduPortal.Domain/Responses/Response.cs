using System.Net;
using System.Text.Json.Serialization;

namespace EduPortal.Core.Responses
{
    public record Response<T>
    {
        public T? Data { get; init; }
        public List<string>? Errors { get; init; }

        [JsonIgnore] public HttpStatusCode StatusCode { get; init; }

        //Static factory method design pattern

        public static Response<T> Success(T data, HttpStatusCode status)
        {
            return new Response<T> { Data = data, StatusCode = status };
        }

        public static Response<T> Fail(List<string> errors, HttpStatusCode status)
        {
            return new Response<T> { Errors = errors, StatusCode = status };
        }

        public static Response<T> Fail(string error, HttpStatusCode status)
        {
            return new Response<T> { Errors = new List<string> { error }, StatusCode = status };
        }

    
    }
}
