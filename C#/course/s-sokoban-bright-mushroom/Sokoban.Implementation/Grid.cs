using Sokoban.Lists;

namespace Sokoban;

/// <summary>
/// A grid of elements of type T.
/// </summary>
/// <typeparam name="T">The type of the elements in the grid.</typeparam>
public class Grid<T>
{
    /// <summary>
    /// The underlying list of lists.
    /// </summary>
    protected readonly ListOf<ListOf<T>> _grid;

    /// <summary>
    /// The number of rows in the grid.
    /// </summary>
    public int NRows => _grid.Length();

    /// <summary>
    /// The number of columns in the grid.
    /// </summary>
    public int NCols => _grid.Get(0).MapOr(0, (row) => row.Length());

    /// <summary>
    /// Creates a new grid.
    /// </summary>
    /// <param name="grid">The list of lists to use as the underlying grid.</param>
    /// <exception cref="ArgumentException">Thrown if the grid is not a valid rectangle.</exception>
    public Grid(ListOf<ListOf<T>> grid)
    {
        if (!IsValidRectangle(grid))
        {
            throw new ArgumentException("grid must be a valid rectangle");
        }
        _grid = grid;
    }

    /// <summary>
    /// Checks if all elements in the grid satisfy the specified predicate.
    /// Stops after the first element that does not satisfy the predicate.
    /// </summary>
    /// <param name="f">The predicate to check for.</param>
    /// <returns>True if all elements satisfy the predicate, false otherwise.</returns>
    public bool All(Func<T, int, int, bool> f) =>
        _grid.All((row, i) => row.All((cell, k) => f(cell, i, k)));

    /// <summary>
    /// Checks if any element in the grid satisfies the specified predicate.
    /// Stops after the first element that satisfies the predicate.
    /// </summary>
    /// <param name="f">The predicate to check for.</param>
    /// <returns>True if any element satisfies the predicate, false otherwise.</returns>
    public bool Any(Func<T, int, int, bool> f) =>
        _grid.Any((row, i) => row.Any((cell, k) => f(cell, i, k)));

    /// <summary>
    /// Finds the first element that satisfies the specified predicate.
    /// </summary>
    /// <param name="f">The predicate to test elements against.</param>
    /// <returns>The first element that satisfies the predicate, or None if no element satisfies the predicate.</returns>
    public Option<T> Find(Func<T, int, int, bool> f)
    {
        Option<T> initialValue = new None<T>();
        return Fold(
            initialValue,
            (acc, item, i, k) => f(item, i, k) ? acc.Or(new Some<T>(item)) : acc
        );
    }

    /// <summary>Gets the indices of the first occurrence of the specified predicate.</summary>
    /// <param name="f">The predicate to match against.</param>
    /// <returns>Some(index) of the first occurrence of the specified predicate, or None if no element satisfies the predicate.</returns>
    public Option<(int, int)> IndexOfWith(Func<T, int, int, bool> f)
    {
        Option<(int, int)> initialValue = new None<(int, int)>();
        return Fold(
            initialValue,
            (acc, item, i, k) => f(item, i, k) ? acc.Or(new Some<(int, int)>((i, k))) : acc
        );
    }

    /// <summary>Gets the indices of the first occurrence of the specified element.</summary>
    /// <param name="val">The element to search for.</param>
    /// <returns>Some(index) of the first occurrence of the specified element, or None if the element is not found.</returns>
    public Option<(int, int)> IndexOf(T val) =>
        IndexOfWith((item, _, _) => item is not null && item.Equals(val));

    /// <summary>
    /// Folds every element into an accumulator by applying an operation, returning the final result.
    /// </summary>
    /// <param name="initial">The initial value of the accumulator.</param>
    /// <param name="f">The function to apply to every element of this grid of sequence.</param>
    /// <typeparam name="TRet">The type of the accumulator.</typeparam>
    /// <returns>The final result of the fold.</returns>
    public TRet Fold<TRet>(TRet initial, Func<TRet, T, int, int, TRet> f) =>
        _grid.Fold(initial, (u, row, i) => row.Fold(u, (u, cell, k) => f(u, cell, i, k)));

    /// <summary>
    /// Gets the element at the specified row and column.
    /// </summary>
    /// <param name="row">The row of the element to get.</param>
    /// <param name="col">The column of the element to get.</param>
    /// <returns>The element at the specified row and column.</returns>
    public Option<T> Get(int row, int col) =>
        _grid.Get(row).AndThen((rowCells) => rowCells.Get(col));

    /// <summary>
    /// Maps the elements of the grid to a new type.
    /// </summary>
    /// <typeparam name="TRet">The type to map to.</typeparam>
    /// <param name="f">The function to map with.</param>
    /// <returns>A grid of mapped elements.</returns>
    public Grid<TRet> Map<TRet>(Func<T, int, int, TRet> f) =>
        new(_grid.Map((row, i) => row.Map((cell, k) => f(cell, i, k))));

    /// <summary>
    /// Sets the element at the specified row and column.
    /// </summary>
    /// <param name="row">The row of the element to set.</param>
    /// <param name="col">The column of the element to set.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>A new grid with the element set.</returns>
    public Grid<T> Set(int row, int col, T value) =>
        _grid
            .Get(row)
            .MapOr(this, (rowGrid) => new Grid<T>(_grid.Set(row, rowGrid.Set(col, value))));

    /// <summary>
    /// Swaps the elements at the specified rows and columns.
    /// </summary>
    /// <param name="row1">The row of the first element to swap.</param>
    /// <param name="col1">The column of the first element to swap.</param>
    /// <param name="row2">The row of the second element to swap.</param>
    /// <param name="col2">The column of the second element to swap.</param>
    /// <returns>A new grid with the elements swapped.</returns>
    public Grid<T> Swap(int row1, int col1, int row2, int col2) =>
        Get(row1, col1)
            .MapOr(
                this,
                (el1) =>
                    Get(row2, col2).MapOr(this, (el2) => Set(row1, col1, el2).Set(row2, col2, el1))
            );

    /// <summary>
    /// Checks if the grid is a valid rectangle.
    /// </summary>
    /// <param name="grid">The grid to check.</param>
    /// <returns>True if the grid is a valid rectangle, false otherwise.</returns>
    private static bool IsValidRectangle(ListOf<ListOf<T>> grid) =>
        grid.Get(0)
            .MapOr(false, (firstRow) => grid.All((row, _) => row.Length() == firstRow.Length()));

    /// <summary>
    /// Gets a string representation of the grid.
    /// </summary>
    /// <returns>A string representation of the grid.</returns>
    public override string ToString() => _grid.ToString();

    /// <summary>
    /// Gets a pretty string representation of the grid.
    /// </summary>
    /// <returns>A string representation of the grid, with each row on a separate line.</returns>
    public string ToPrettyString() =>
        "[" + _grid.Fold("\n", (acc, val, _) => "\n    " + val.ToString() + acc) + "]";

    /// <summary>
    /// Returns the underlying list of lists.
    /// </summary>
    /// <returns>The underlying list of lists.</returns>
    public ListOf<ListOf<T>> ToListOfList() => _grid;
}
