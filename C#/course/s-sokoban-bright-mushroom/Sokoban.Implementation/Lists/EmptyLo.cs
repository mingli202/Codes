namespace Sokoban.Lists;

/// <summary>
/// An empty list.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public class EmptyLo<T> : ListOf<T>
{
    /// <inheritdoc />
    public override bool All(Func<T, int, bool> predicate) => true;

    /// <inheritdoc />
    public override bool Any(Func<T, int, bool> predicate) => false;

    /// <inheritdoc />
    public override bool Equals(ListOf<T> other) => other.EqualsEmptyLo(this);

    /// <inheritdoc />
    public override bool EqualsLinkLo(LinkLo<T> other) => false;

    /// <inheritdoc />
    public override bool EqualsEmptyLo(EmptyLo<T> other) => true;

    /// <inheritdoc />
    public override Option<T> Find(Func<T, int, bool> predicate) => new None<T>();

    /// <inheritdoc />
    public override TResult Fold<TResult>(
        TResult initialValue,
        Func<TResult, T, int, TResult> fn
    ) => initialValue;

    /// <inheritdoc />
    public override int Length() => 0;

    /// <inheritdoc />
    public override ListOf<ListOf<T>> SplitAt(T separator) => SplitAtHelper(separator).subLists;

    /// <inheritdoc />
    public override (ListOf<ListOf<T>> subLists, ListOf<T> acc) SplitAtHelper(T separator) =>
        (new EmptyLo<ListOf<T>>(), new EmptyLo<T>());

    /// <inheritdoc />
    public override string ToString() => "[]";

    /// <inheritdoc />
    public override string ToStringHelper() => "]";
}
