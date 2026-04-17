using ImageLib;
using Sokoban.Enum;

namespace Sokoban.Cells;

/// <summary>
/// The player cell with a facing direction.
/// </summary>
/// <param name="direction">The player's facing direction.</param>
public class Player(Direction direction) : Cell
{
    private readonly Direction _direction = direction;

    /// <summary>
    /// Creates a player from a direction character.
    /// </summary>
    /// <param name="direction">The direction character.</param>
    public Player(char direction)
        : this(DirectionFrom(direction)) { }

    /// <inheritdoc />
    public override WorldImage GetImage() =>
        GetPlayerImageWithDirection(new FromFileImage("assets/player.png"));

    /// <inheritdoc />
    public override bool Equals(ICell other) => other.EqualsPlayer(this);

    /// <inheritdoc />
    public override bool EqualsPlayer(Player other) => other._direction == _direction;

    /// <inheritdoc />
    public override bool IsPlayer() => true;

    /// <summary>
    /// Converts a direction character to a <see cref="Direction"/>.
    /// </summary>
    /// <param name="c">The direction character.</param>
    /// <returns>The corresponding direction.</returns>
    public static Direction DirectionFrom(char c) =>
        c switch
        {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(c)),
        };

    /// <summary>
    /// Returns the player image rotated to match the current direction.
    /// </summary>
    /// <param name="image">The base player image.</param>
    /// <returns>The rotated image.</returns>
    private WorldImage GetPlayerImageWithDirection(WorldImage image) =>
        _direction switch
        {
            Direction.Up => new RotateImage(image, 90),
            Direction.Down => new RotateImage(image, 270),
            Direction.Left => new RotateImage(image, 180),
            Direction.Right => image,
            _ => throw new ArgumentException(nameof(_direction)),
        };

    /// <summary>
    /// Returns the character representation of the player direction.
    /// </summary>
    /// <returns>The direction character.</returns>
    public override string ToString() =>
        _direction switch
        {
            Direction.Up => "^",
            Direction.Down => "v",
            Direction.Left => "<",
            Direction.Right => ">",
            _ => throw new ArgumentException(nameof(_direction)),
        };
}

