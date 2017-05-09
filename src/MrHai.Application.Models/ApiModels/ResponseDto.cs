using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Application.Models.ApiModels
{
    public class ResponseDto
    {
        public ResponseDto(int status = 200, string msg = "")
        {
            this.Status = status;
            this.Msg = msg;
        }

        public static ResponseDto ErrorResponse(int status = (int)ResponseStatus.Error, string msg = "")
        {
            return new ResponseDto(status, msg);
        }
        public static ResponseDto ErrorResponse(string msg = "")
        {
            return new ResponseDto((int)ResponseStatus.Error, msg);
        }
        public int Status { get; set; }
        public string Msg { get; set; }
    }
    public class ResponseDto<T> : ResponseDto
    {
        public ResponseDto(int status = 200, string msg = "") : base(status, msg)
        {
        }

        public T Data { get; set; }

    }
    public enum ResponseStatus
    {
        Ok = 200,
        NoFound = 404,
        Error = 500
    }
}
