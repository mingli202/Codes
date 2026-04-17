namespace Sokoban;

/// <summary>
/// Represents an optional value.
/// </summary>
/// <typeparam name="T">The wrapped value type.</typeparam>
public abstract record Option<T>
{
    /// <summary>
    /// Creates a new Some option.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A new Some option.</returns>
    public static Option<U> Some<U>(U value) => new Some<U>(value);

    /// <summary>
    /// Creates a new None option.
    /// </summary>
    /// <returns>A new None option.</returns>
    public static Option<U> None<U>() => new None<U>();

    /// <summary>
    /// Returns None if the option is None, otherwise calls f with the wrapped value and returns the result.
    /// </summary>
    /// <param name="f">The function to apply to the wrapped value.</param>
    /// <typeparam name="TRet">The type of the result.</typeparam>
    /// <returns>The result of applying f to the wrapped value, or None if the option is None.</returns>
    public abstract Option<TRet> AndThen<TRet>(Func<T, Option<TRet>> f);

    /// <summary>
    /// Returns the wrapped value if the option is Some, otherwise throws an exception with the specified message.
    /// </summary>
    /// <param name="message">The message to include in the exception.</param>
    /// <returns>The wrapped value.</returns>
    public abstract T Expect(string message);

    /// <summary>Returns true if the option is None, otherwise false.</summary>
    public abstract bool IsNone();

    /// <summary>Returns true if the option is Some, otherwise false.</summary>
    public abstract bool IsSome();

    /// <summary>
    /// Returns None if the option is None, otherwise calls f with the wrapped value and returns the result.
    /// </summary>
    /// <param name="f">The function to apply to the wrapped value.</param>
    /// <typeparam name="TRet">The type of the result.</typeparam>
    /// <returns>The result of applying f to the wrapped value, or None if the option is None.</returns>
    public abstract Option<TRet> Map<TRet>(Func<T, TRet> f);

    /// <summary>
    /// Returns the specified default value if the option is None, otherwise calls f with the wrapped value and returns the result.
    /// </summary>
    /// <param name="defaultValue">The default value to return if the option is None.</param>
    /// <param name="f">The function to apply to the wrapped value.</param>
    /// <typeparam name="TRet">The type of the result.</typeparam>
    /// <returns>The result of applying f to the wrapped value, or the default value if the option is None.</returns>
    public abstract TRet MapOr<TRet>(TRet defaultValue, Func<T, TRet> f);

    /// <summary>
    /// Returns this option if it is Some, otherwise returns <paramref name="optb"/>.
    /// </summary>
    /// <param name="optb">The fallback option.</param>
    /// <returns>This option if it is Some; otherwise <paramref name="optb"/>.</returns>
    public abstract Option<T> Or(Option<T> optb);

    /// <summary>
    /// Returns this option if it is Some, otherwise returns a Some created from <paramref name="f"/>.
    /// </summary>
    /// <param name="f">The function to produce a fallback value.</param>
    /// <returns>This option if it is Some; otherwise a new Some from <paramref name="f"/>.</returns>
    public abstract Option<T> OrElse(Func<T> f);

    /// <summary>
    /// Returns the wrapped value if the option is Some, otherwise throws an exception.
    /// </summary>
    /// <returns>The wrapped value.</returns>
    public T Unwrap() => Expect("called unwrap on a None");

    /// <summary>
    /// Returns the wrapped value if the option is Some, otherwise returns the specified default value.
    /// </summary>
    /// <param name="defaultValue">The default value to return if the option is None.</param>
    /// <returns>The wrapped value if the option is Some, otherwise the specified default value.</returns>
    public T UnwrapOr(T defaultValue) => OrElse(() => defaultValue).Unwrap();

    /// <summary>
    /// Returns the wrapped value if the option is Some, otherwise returns the result of calling the specified function.
    /// </summary>
    /// <param name="f">The function to call if the option is None.</param>
    /// <returns>The wrapped value if the option is Some, otherwise the result of calling the specified function.</returns>
    public T UnwrapOrElse(Func<T> f) => OrElse(f).Unwrap();
}

/// <summary>
/// Represents a present value.
/// </summary>
/// <typeparam name="T">The wrapped value type.</typeparam>
/// <param name="Value">The wrapped value.</param>
public record Some<T>(T Value) : Option<T>
{
    /// <inheritdoc />
    public override Option<TRet> AndThen<TRet>(Func<T, Option<TRet>> f) => f(Value);

    /// <inheritdoc />
    public override T Expect(string message) => Value;

    /// <inheritdoc />
    public override bool IsNone() => false;

    /// <inheritdoc />
    public override bool IsSome() => true;

    /// <inheritdoc />
    public override Option<TRet> Map<TRet>(Func<T, TRet> f) => new Some<TRet>(f(Value));

    /// <inheritdoc />
    public override TRet MapOr<TRet>(TRet defaultValue, Func<T, TRet> f) => f(Value);

    /// <inheritdoc />
    public override Option<T> Or(Option<T> optb) => this;

    /// <inheritdoc />
    public override Option<T> OrElse(Func<T> f) => this;
}

/// <summary>
/// Represents the absence of a value.
/// </summary>
/// <typeparam name="T">The wrapped value type.</typeparam>
public record None<T> : Option<T>
{
    /// <inheritdoc />
    public override Option<TRet> AndThen<TRet>(Func<T, Option<TRet>> f) => new None<TRet>();

    /// <inheritdoc />
    public override T Expect(string message) => throw new ArgumentException(message);

    /// <inheritdoc />
    public override bool IsNone() => true;

    /// <inheritdoc />
    public override bool IsSome() => false;

    /// <inheritdoc />
    public override Option<TRet> Map<TRet>(Func<T, TRet> f) => new None<TRet>();

    /// <inheritdoc />
    public override TRet MapOr<TRet>(TRet defaultValue, Func<T, TRet> f) => defaultValue;

    /// <inheritdoc />
    public override Option<T> Or(Option<T> optb) => optb;

    /// <inheritdoc />
    public override Option<T> OrElse(Func<T> f) => new Some<T>(f());
}
