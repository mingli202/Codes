namespace Sokoban.Enum;

/// <summary>
/// Describes why gameplay ended for a level.
/// </summary>
/// <param name="Message">The message shown to the player.</param>
/// <param name="LevelNumber">The zero-based level index associated with the outcome.</param>
public abstract record GameOverReason(string Message, int LevelNumber);

/// <summary>
/// Indicates that the player completed a level successfully.
/// </summary>
/// <param name="levelNumber">The completed level number.</param>
public record Win(int levelNumber) : GameOverReason("You win!", levelNumber);

/// <summary>
/// Indicates that the player failed a level.
/// </summary>
/// <param name="reason">The loss message shown to the player.</param>
/// <param name="levelNumber">The failed level number.</param>
public record Lose(string reason, int levelNumber) : GameOverReason(reason, levelNumber);
