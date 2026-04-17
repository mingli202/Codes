using ImageLib;

namespace Sokoban.cells;

/// <summary>
/// Cardinal directions the player can face or move.
/// </summary>
public enum PlayerDirection
{
    Up,
    Down,
    Left,
    Right,
}

/// <summary>
/// The player cell with a facing direction.
/// </summary>
/// <param name="direction">The player's facing direction.</param>
public class Player(PlayerDirection direction) : Cell
{
    private readonly PlayerDirection _direction = direction;

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
    public override bool Equals(Cell other) => other.EqualsPlayer(this);

    /// <inheritdoc />
    public override bool EqualsPlayer(Player other) => other._direction == _direction;

    /// <inheritdoc />
    public override bool IsPlayer() => true;

    /// <summary>
    /// Converts a direction character to a <see cref="PlayerDirection"/>.
    /// </summary>
    /// <param name="c">The direction character.</param>
    /// <returns>The corresponding direction.</returns>
    public static PlayerDirection DirectionFrom(char c) =>
        c switch
        {
            '^' => PlayerDirection.Up,
            'v' => PlayerDirection.Down,
            '<' => PlayerDirection.Left,
            '>' => PlayerDirection.Right,
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
            PlayerDirection.Up => new RotateImage(image, 90),
            PlayerDirection.Down => new RotateImage(image, 270),
            PlayerDirection.Left => new RotateImage(image, 180),
            PlayerDirection.Right => image,
            _ => throw new ArgumentException(nameof(_direction)),
        };

    /// <summary>
    /// Returns the character representation of the player direction.
    /// </summary>
    /// <returns>The direction character.</returns>
    public override string ToString() =>
        _direction switch
        {
            PlayerDirection.Up => "^",
            PlayerDirection.Down => "v",
            PlayerDirection.Left => "<",
            PlayerDirection.Right => ">",
            _ => throw new ArgumentException(nameof(_direction)),
        };
}

