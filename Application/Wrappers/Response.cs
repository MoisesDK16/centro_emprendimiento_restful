using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{

    public class Response<T>
    {

        public Response()
        {
            Errors = new List<string>();
        }

        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = new List<string>();
        }

        public Response(List<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        public static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }
    }
}
