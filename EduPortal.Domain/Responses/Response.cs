using Serilog;
using System.Net;
using System.Runtime.CompilerServices;
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

        public static Response<T> Fail(List<string> errors, HttpStatusCode status, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            foreach (var error in errors)
            {
                string filteredErrorMessage = error.Split(new string[] { "at " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                Log.Warning($"Oluşan Hata: {filteredErrorMessage} Yeni hata oluştu - Sınıf: {className}, Satır: {lineNumber}");
            }
            return new Response<T> { Errors = errors, StatusCode = status };
        }


        public static Response<T> Fail(string errorMessage, HttpStatusCode status, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            string filteredErrorMessage = errorMessage.Split(new string[] { "at " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            string className = Path.GetFileNameWithoutExtension(filePath);
            Log.Warning($"Yeni hata oluştu: {filteredErrorMessage} - Sınıf: {className}, Satır: {lineNumber}");

            return new Response<T> { Errors = new List<string> { filteredErrorMessage }, StatusCode = status };
        }
    }
}
