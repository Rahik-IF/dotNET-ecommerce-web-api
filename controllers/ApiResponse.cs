using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api.controllers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeStamp { get; set; }

        // constructor for succes response
        private ApiResponse(
            bool success,
            string message,
            T? data,
            List<string>? errors,
            int statusCode
             )
        {

            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
            TimeStamp = DateTime.UtcNow;
        }

        // A static method for successful response
        public static ApiResponse<T> SuccessResponse(
            T? data,
            int statusCode,
            string message = ""
        )
        {
            //return parameters to the constructor for success response
            return new ApiResponse<T>(true, message, data, null, statusCode);
        }

         // A static method for  error response
        public static ApiResponse<T> ErrorResponse(
            List<string> errors,
            int statusCode,
            string message = ""
        )
        {
            //return parameters to the constructor for success response
            return new ApiResponse<T>(false, message, default(T) ,errors,  statusCode);
        }
    }
}