namespace Sokoban.Lists;

/// <summary>
/// A non-empty list node.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
/// <param name="first">The first element.</param>
/// <param name="rest">The rest of the list.</param>
public class LinkLo<T>(T first, ListOf<T> rest) : ListOf<T>
{
    private readonly T _first = first;
    private readonly ListOf<T> _rest = rest;

    /// <inheritdoc />
    public override bool All(Func<T, int, bool> predicate) =>
        predicate(_first, 0) && _rest.All((item, index) => predicate(item, index + 1));

    /// <inheritdoc />
    public override bool Any(Func<T, int, bool> predicate) =>
        predicate(_first, 0) || _rest.Any((item, index) => predicate(item, index + 1));

    /// <inheritdoc />
    public override bool Equals(ListOf<T> other) => other.EqualsLinkLo(this);

    /// <inheritdoc />
    public override bool EqualsLinkLo(LinkLo<T> other) =>
        other._first is not null && other._first.Equals(_first) && other._rest.Equals(_rest);

    /// <inheritdoc />
    public override bool EqualsEmptyLo(EmptyLo<T> other) => false;

    /// <inheritdoc />
    public override Option<T> Find(Func<T, int, bool> predicate) =>
        predicate(_first, 0)
            ? new Some<T>(_first)
            : _rest.Find((item, index) => predicate(item, index + 1));

    /// <inheritdoc />
    public override TResult Fold<TResult>(
        TResult initialValue,
        Func<TResult, T, int, TResult> fn
    ) => fn(_rest.Fold(initialValue, (u, item, index) => fn(u, item, index + 1)), _first, 0);

    /// <inheritdoc />
    public override int Length() => _rest.Length() + 1;

    /// <inheritdoc />
    public override ListOf<ListOf<T>> SplitAt(T separator)
    {
        var (subLists, acc) = SplitAtHelper(separator);
        return new LinkLo<ListOf<T>>(acc, subLists);
    }

    /// <inheritdoc />
    public override (ListOf<ListOf<T>> subLists, ListOf<T> acc) SplitAtHelper(T separator)
    {
        var (subLists, acc) = _rest.SplitAtHelper(separator);

        if (separator is not null && separator.Equals(_first))
        {
            return (new LinkLo<ListOf<T>>(acc, subLists), new EmptyLo<T>());
        }

        return (subLists, new LinkLo<T>(_first, acc));
    }

    /// <inheritdoc />
    public override string ToString() => "[" + _first?.ToString() + _rest.ToStringHelper();

    /// <inheritdoc />
    public override string ToStringHelper() => ", " + _first?.ToString() + _rest.ToStringHelper();
}
