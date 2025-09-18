using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
public class ResultTests
{
    private static Result<Addresse> CheckIsFaulted()
    {
        var result = new Result<Addresse>(new Exception("Test"));
        Check.That(result).IsInstanceOf<Result<Addresse>>();
        Check.That(result.IsSuccess).IsFalse();
        Check.That(result.IsFaulted).IsTrue();
        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception?.Message).IsEqualTo("Test");

        return result;
    }

    private static Result<Addresse> CheckIsSuccess()
    {
        var result = new Result<Addresse>(new Addresse(null!, null!,
                                                       "Paris", null!));
        Check.That(result).IsInstanceOf<Result<Addresse>>();
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.IsFaulted).IsFalse();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value?.Ville).IsEqualTo("Paris");
        Check.That(result.Exception).IsNull();

        return result;
    }

    [TestMethod]
    public void FaultedIsTest()
    {
        CheckIsFaulted();
    }

    [TestMethod]
    public void SuccessIsTest()
    {
        CheckIsSuccess();
    }

    [TestMethod]
    public void ValidateFailTest()
    {
        var result = CheckIsFaulted();

        Check.ThatCode(() => result.Validate())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Test");
    }

    [TestMethod]
    public void ValidateTest()
    {
        var result = CheckIsSuccess();

        var address = result.Validate();

        Check.That(address).IsNotNull();
        Check.That(address.Ville).IsEqualTo("Paris");
    }

    [TestMethod]
    public void SuccessTest()
    {
        var result = Result<Compte>.Success(new Compte());
        Check.That(result.Value).IsNotNull();
        Check.That(result.Exception).IsNull();
    }

    [TestMethod]
    public void FailureTest()
    {
        var result = Result<Compte>.Failure(new KrosoftException("Test"));

        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
    }

    [TestMethod]
    public void Map_Should_Transform_Value_When_Success()
    {
        var result = Result<int>.Success(42);

        var mapped = result.Map(i => $"Value is {i}");

        Check.That(mapped.IsSuccess).IsTrue();
        Check.That(mapped.Value).IsEqualTo("Value is 42");
    }

    [TestMethod]
    public void Map_Should_Preserve_Exception_When_Failure()
    {
        var exception = new InvalidOperationException("Error");
        var result = Result<int>.Failure(exception);

        var mapped = result.Map(i => $"Value is {i}");

        Check.That(mapped.IsFaulted).IsTrue();
        Check.That(mapped.Exception).IsSameReferenceAs(exception);
    }

    [TestMethod]
    public void Bind_Should_Transform_Into_New_Result_When_Success()
    {
        var result = Result<int>.Success(5);

        var bound = result.Bind(i => Result<string>.Success($"Number: {i}"));

        Check.That(bound.IsSuccess).IsTrue();
        Check.That(bound.Value).IsEqualTo("Number: 5");
    }

    [TestMethod]
    public void Bind_Should_Return_Failure_When_Original_Is_Failure()
    {
        var exception = new NotImplementedException("param");
        var result = Result<int>.Failure(exception);

        var bound = result.Bind(i => Result<string>.Success($"Number: {i}"));

        Check.That(bound.IsFaulted).IsTrue();
        Check.That(bound.Exception).IsSameReferenceAs(exception);
        Check.That(bound.Value).IsNull();
    }

    [TestMethod]
    public void Bind_Should_Propagate_Inner_Failure()
    {
        var result = Result<int>.Success(1);

        var bound = result.Bind(_ => Result<string>.Failure(new InvalidOperationException("Inner fail")));

        Check.That(bound.IsFaulted).IsTrue();
        Check.That(bound.Exception).IsInstanceOf<InvalidOperationException>();
        Check.That(bound.Exception!.Message).IsEqualTo("Inner fail");
    }

    [TestMethod]
    public void Bind_Should_Use_Input_Value()
    {
        // Arrange
        var result = Result<int>.Success(3);

        // Act
        var bound = result.Bind(i =>
        {
            if (i % 2 == 0)
            {
                return Result<string>.Success("Even");
            }

            return Result<string>.Failure(new InvalidOperationException("Odd not allowed"));
        });

        // Assert
        Check.That(bound.IsFaulted).IsTrue();
        Check.That(bound.Exception).IsInstanceOf<InvalidOperationException>();
        Check.That(bound.Exception!.Message).IsEqualTo("Odd not allowed");
    }

    [TestMethod]
    public void ToFailureResult_ShouldCreateFailureResult()
    {
        var exception = new InvalidOperationException("fail");

        var result = exception.ToFailureResult<string>();

        Check.That(result).IsInstanceOf<Result<string>>();
        Check.That(result.IsFaulted).IsTrue();
        Check.That(result.Exception).IsSameReferenceAs(exception);
    }

    [TestMethod]
    public void ToSuccessResult_ShouldCreateSuccessResult()
    {
        var value = "ok";

        var result = value.ToSuccessResult();

        Check.That(result).IsInstanceOf<Result<string>>();
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsEqualTo(value);
    }

    [TestMethod]
    public void ToSuccessResult_ShouldWorkWithBind()
    {
        var value = "data";

        var result = value.ToSuccessResult()
                          .Bind(val =>
                          {
                              if (val.Length > 3)
                              {
                                  return val.Length.ToSuccessResult();
                              }

                              return Result<int>.Failure(new InvalidOperationException("Too short"));
                          });

        Check.That(result).IsInstanceOf<Result<int>>();
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsEqualTo(4);
    }

    [TestMethod]
    public void ToFailureResult_ShouldWorkWithBind()
    {
        var ex = new InvalidOperationException("fail");
        var result = ex.ToFailureResult<string>();
        Check.That(result).IsInstanceOf<Result<string>>();
        Check.That(result.IsFaulted).IsTrue();
        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsSameReferenceAs(ex);

        var binded = result.Bind(val => Result<int>.Success(val.Length));

        Check.That(binded).IsInstanceOf<Result<int>>();
        Check.That(binded.IsFaulted).IsTrue();
        Check.That(binded.Value).IsDefaultValue();
        Check.That(binded.Exception).IsSameReferenceAs(ex);
    }

    [TestMethod]
    public void ToSuccessResult_ShouldWorkWithMap()
    {
        var value = "data";

        var result = value.ToSuccessResult()
                          .Map(val => val.ToUpper());

        Check.That(result).IsInstanceOf<Result<string>>();
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsEqualTo("DATA");
    }

    [TestMethod]
    public void ToFailureResult_ShouldWorkWithMap()
    {
        var exception = new InvalidOperationException("fail");

        var result = exception.ToFailureResult<string>()
                              .Map(val => val.ToUpper());

        Check.That(result).IsInstanceOf<Result<string>>();
        Check.That(result.IsFaulted).IsTrue();
        Check.That(result.Exception).IsSameReferenceAs(exception);
    }

    [TestMethod]
    public void Bind_Should_Return_Success_Result()
    {
        var result = Result<int>.Success(10);

        var switched = result.Map(
                                  num => $"Number is {num}",
                                  _ => new InvalidOperationException("Should not reach here"));

        Check.That(switched.IsSuccess).IsTrue();
        Check.That(switched.Value).IsEqualTo("Number is 10");
    }

    [TestMethod]
    public void Bind_Should_Transform_Exception_When_Failure()
    {
        var exception = new Exception("Original error");
        var result = Result<int>.Failure(exception);

        var switched = result.Map(
                                  num => $"Number is {num}",
                                  _ => new InvalidOperationException("Transformed error"));

        Check.That(switched.IsFaulted).IsTrue();
        Check.That(switched.Exception).IsInstanceOf<InvalidOperationException>();
        Check.That(switched.Exception!.Message).IsEqualTo("Transformed error");
    }

    [TestMethod]
    public void Bind_Should_Not_Call_OnFailure_When_Success()
    {
        var result = Result<int>.Success(42);

        var switched = result.Map(
                                  num => $"Value is {num}",
                                  _ => new InvalidOperationException("This should not be called"));

        Check.That(switched.IsSuccess).IsTrue();
        Check.That(switched.Value).IsEqualTo("Value is 42");
    }

    [TestMethod]
    public void Bind_Should_Not_Call_OnSuccess_When_Failure()
    {
        var exception = new Exception("Test exception");
        var result = Result<int>.Failure(exception);

        var switched = result.Bind<int>(_ => throw new Exception("This should not be called"),
                                        _ => new InvalidOperationException("Handled failure"));

        Check.That(switched.IsFaulted).IsTrue();
        Check.That(switched.Exception).IsInstanceOf<InvalidOperationException>();
        Check.That(switched.Exception!.Message).IsEqualTo("Handled failure");
    }

    [TestMethod]
    public void Bind_Should_Not_Call_OnSuccess_When_Failure_Is_Returned()
    {
        var exception = new KrosoftTechnicalException("Test exception");
        var result = Result<int>.Failure(exception);

        var switched = result.Bind<int>(_ => throw new Exception("This should not be called"));

        Check.That(switched.IsFaulted).IsTrue();
        Check.That(switched.Exception).IsInstanceOf<KrosoftTechnicalException>();
        Check.That(switched.Exception!.Message).IsEqualTo("Test exception");
    }
}