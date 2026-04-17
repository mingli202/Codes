namespace Sokoban;

/// <summary>
/// Represents an input event received by the game.
/// </summary>
public abstract record Event;

/// <summary>
/// Represents a keyboard key press event.
/// </summary>
/// <param name="Key">The pressed key.</param>
public record KeyDown(string Key) : Event;

/// <summary>
/// Represents when the game ticks.
/// </summary>
public record Tick : Event;
