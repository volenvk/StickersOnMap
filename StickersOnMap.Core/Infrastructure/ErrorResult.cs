namespace StickersOnMap.Core.Infrastructure
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ErrorResult : ObjectResult
    {
        /// <param name="message">сообщение для пользователя</param>
        /// <param name="status">статус код ошибки, значения: 400-599 </param>
        public ErrorResult(string message, int status)
            : base(new {message, status})
        {
            if (status < 400 || status > 599)
                throw new ArgumentException("status must be in range 400-599");

            StatusCode = status;
        }

        public static ErrorResult BadRequest(string message) =>
            new ErrorResult(message, StatusCodes.Status400BadRequest);

        public static ErrorResult NotFound(string message) =>
            new ErrorResult(message, StatusCodes.Status404NotFound);
        
        public static ErrorResult Conflict(string message) =>
            new ErrorResult(message, StatusCodes.Status409Conflict);

        public static ErrorResult NotAcceptable(string message) =>
            new ErrorResult(message, StatusCodes.Status406NotAcceptable);
        
        public static ErrorResult InternalServerError(string message) =>
            new ErrorResult(message, StatusCodes.Status500InternalServerError);
    }
}