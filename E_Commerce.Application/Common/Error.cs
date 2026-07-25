using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public record Error(string Code , string Description, ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(string code = "General.Failure", string description = "Failure Error Has Occured") 
            => new Error(code, description, ErrorType.Failure);
        public static Error NotFound(string code = "General.NotFound", string description = "NotFound Error Has Occured") 
            => new Error(code, description, ErrorType.NotFound);
        public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden Error Has Occured") 
            => new Error(code, description, ErrorType.Forbidden);
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Unauthorized Error Has Occured") 
            => new Error(code, description, ErrorType.Unauthorized);
        public static Error Conflict(string code = "General.Conflict", string description = "Conflict Error Has Occured") 
            => new Error(code, description, ErrorType.Conflict);
        public static Error Validation(string code = "General.Validation", string description = "Validation Error Has Occured") 
            => new Error(code, description, ErrorType.Validation);
        public static Error InValidCredentials(string code = "General.InValidCredentials", string description = "InValidCredentials Error Has Occured") 
            => new Error(code, description, ErrorType.InValidCredentials);

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType 
    {
        Failure = 0,
        NotFound,
        Forbidden,
        Unauthorized,
        Conflict,
        Validation,
        InValidCredentials // 6
    }
}
