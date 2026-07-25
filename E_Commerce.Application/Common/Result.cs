using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class Result // void
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> Errors { get; }

        public Result(bool isSuccess , IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Ok() 
            => new Result(true, Array.Empty<Error>());

        public static Result Fail(Error error) 
            => new Result(false, new[] { error });
        public static Result Fail(IReadOnlyList<Error> errors) 
            => new Result(false, errors);
    }
    public class Result<TValue> : Result  /// with value
    {
        private readonly TValue _value;
        public TValue data => IsSuccess ? _value : throw new InvalidOperationException("Failed to Get Data.");

        public Result(TValue value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }
        public Result(Error error) : base(false, new[] { error })
        {
            _value = default!;
        }
        public Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            _value = default!;
        }

        public static Result<TValue> Ok(TValue value) 
            => new Result<TValue>(value);

        public static Result<TValue> Fail(Error error) 
            => new Result<TValue>(error);
        public static Result<TValue> Fail(IReadOnlyList<Error> errors) 
            => new Result<TValue>(errors);
    }



}
