using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using Sokoban.Colors;

namespace Sokoban.cells;

/// <summary>
/// A target cell that accepts a trophy of a matching color.
/// </summary>
/// <param name="color">The target color.</param>
public class Target(AColor color) : Cell
{
    private readonly AColor _color = color;

    /// <summary>
    /// Creates a target from a color character.
    /// </summary>
    /// <param name="color">The color character.</param>
    public Target(char color)
        : this(AColor.From(color)) { }

    /// <inheritdoc />
    public override bool IsTrophyOnTarget(Cell other) => other.IsTrophyOnTargetTarget(_color);

    /// <inheritdoc />
    public override bool Equals(Cell other) => other.EqualsTarget(this);

    /// <inheritdoc />
    public override bool EqualsTarget(Target other) => other._color.Equals(_color);

    /// <summary>
    /// Gets the image of the target, which is a colored circle with a white ring inside.
    /// </summary>
    /// <returns>The image of the target.</returns>
    public override WorldImage GetImage() =>
        new OverlayImage(
            new CircleImage(20, OutlineMode.Fill, Color.FromRgb(_color.R, _color.G, _color.B)),
            new OverlayImage(
                new CircleImage(40, OutlineMode.Fill, Color.FromRgb(255, 255, 255)),
                new CircleImage(60, OutlineMode.Fill, Color.FromRgb(_color.R, _color.G, _color.B))
            )
        );

    /// <summary>
    /// Returns the character representation of the target color.
    /// </summary>
    /// <returns>The target character.</returns>
    public override string ToString() =>
        _color switch
        {
            Blue => "B",
            Green => "G",
            Red => "R",
            Yellow => "Y",
            _ => throw new ArgumentOutOfRangeException(nameof(_color)),
        };
}

