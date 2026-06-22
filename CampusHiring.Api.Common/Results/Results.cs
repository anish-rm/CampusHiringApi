namespace CampusHiring.Api.Common.Results;

public readonly record struct Error(string Code, string Description)
{
    public static readonly Error None = new("","");
    public bool isNone => string.IsNullOrEmpty(Code);
}

public readonly record struct Result
{
    public bool IsSuccess { get; }
    public Error[] Errors { get; }

    private Result(bool isSuccess, Error[] errors)
        => (IsSuccess, Errors) = (isSuccess, errors);

    public static Result Success() => new(true, Array.Empty<Error>());
    public static Result Failure(params Error[] errors) => new(false, errors);
    public static Result BadRequest(params Error[] errors) => new(false, errors);
    public static Result NotFound(params Error[] errors) => new(false, errors);

}

public readonly record struct Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error[] Errors { get; }

    private Result(bool isSuccess, Error[] errors, T? value)
        => (IsSuccess, Errors, Value) = (isSuccess, errors, value);

    public static Result<T> Success(T Value) => new(true, Array.Empty<Error>(), Value);
    public static Result<T> Failure(params Error[] errors) => new(false, errors, default);
    public static Result<T> BadRequest(params Error[] errors) => new(false, errors,default);
    public static Result<T> NotFound(params Error[] errors) => new(false, errors, default);

}
