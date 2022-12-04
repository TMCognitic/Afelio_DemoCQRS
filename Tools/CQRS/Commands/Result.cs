using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.CQRS.Commands
{
    public class Result
    {
        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Failure(string message)
        {
            return new Result(false, message);
        }

        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;

        public string? Message { get; init; }

        private Result(bool isSucess, string? message = null)
        {
            IsSuccess = isSucess;
            Message = message;
        }
    }
}
