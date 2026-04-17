namespace Sokoban;

/// <summary>
/// Represents a state transition or control command produced by the game.
/// </summary>
public abstract record Action;

/// <summary>
/// Ends the current game session.
/// </summary>
public record Quit : Action;

/// <summary>
/// Replaces the current game state with a new one.
/// </summary>
/// <param name="state">The state to activate.</param>
public record SwitchState(GameState state) : Action;

/// <summary>
/// Renders the current game state.
/// </summary>
public record Render : Action;

/// <summary>
/// Indicates that no state change is required.
/// </summary>
public record Nothing : Action;
