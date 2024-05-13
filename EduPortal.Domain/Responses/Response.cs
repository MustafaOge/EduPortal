using System.Net;
using System.Text.Json.Serialization;
using EduPortal.Domain.Entities;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.CompilerServices;

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
            // Hataları loglarken sadece hata mesajlarını almak için bir döngü kullanabiliriz
            foreach (var error in errors)
            {
                string filteredErrorMessage = error.Split(new string[] { "at " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                Log.Warning($"Oluşan Hata: {filteredErrorMessage} Yeni hata oluştu - Sınıf: {className}, Satır: {lineNumber}");
            }
            return new Response<T> { Errors = errors, StatusCode = status };
        }


        public static Response<T> Fail(string errorMessage, HttpStatusCode status, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            // Stack Trace bilgisini içeren kısımları filtreleyerek sadece hata mesajını alıyoruz
            string filteredErrorMessage = errorMessage.Split(new string[] { "at " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            string className = Path.GetFileNameWithoutExtension(filePath);
            Log.Warning($"Yeni hata oluştu: {filteredErrorMessage} - Sınıf: {className}, Satır: {lineNumber}");

            return new Response<T> { Errors = new List<string> { filteredErrorMessage }, StatusCode = status };
        }



    }
}
