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
    public override bool Equals(IListOf<T> other) => other.EqualsEmptyLo(this);

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
    public override IListOf<T> PopFront() => this;

    /// <inheritdoc />
    public override IListOf<T> Take(int count) => this;

    /// <inheritdoc />
    public override IListOf<IListOf<T>> SplitAt(T separator) => SplitAtHelper(separator).subLists;

    /// <inheritdoc />
    public override (IListOf<IListOf<T>> subLists, IListOf<T> acc) SplitAtHelper(T separator) =>
        (new EmptyLo<IListOf<T>>(), new EmptyLo<T>());

    /// <inheritdoc />
    public override string ToString() => "[]";

    /// <inheritdoc />
    public override string ToStringHelper() => "]";
}
