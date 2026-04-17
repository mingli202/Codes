using Sokoban.Interfaces;
using Sokoban.Lists;
using Sokoban.Objects;
using System.Windows.Media;

namespace Sokoban
{
    /// <summary>
    /// Transforms a string representation of a Sokoban level into the internal data definition(<see cref = "ILoloCell" />).
    /// The level is described with two aligned, multi-line strings: one for the ground layout and one for the content.
    /// </summary>
    public static class StringBoardParser
    {
        #region Constants
        const string NEW_LINE_CHARACTER = "\n";

        const char FLOOR_CHARACTER = '_';

        const string BLUE_COLOR_CHARACTER = "B";
        const string RED_COLOR_CHARACTER = "R";
        const string YELLOW_COLOR_CHARACTER = "Y";
        const string GREEN_COLOR_CHARACTER = "G";

        const string WALL_CHARACTER = "W";
        const string BOX_CHARACTER = BLUE_COLOR_CHARACTER;

        const string BLUE_TROPHY_CHARACTER = "b";
        const string RED_TROPHY_CHARACTER = "r";
        const string YELLOW_TROPHY_CHARACTER = "y";
        const string GREEN_TROPHY_CHARACTER = "g";

        const string PLAYER_UP_CHARACTER = "^";
        const string PLAYER_LEFT_CHARACTER = "<";
        const string PLAYER_RIGHT_CHARACTER = ">";
        const string PLAYER_DOWN_CHARACTER = "v";

        public const string WIDTH_MISMATCH_ERROR_MSG = "Content width does not match ground width";
        public const string HEIGHT_MISMATCH_ERROR_MSG = "Content height does not match ground height";
        #endregion

        /// <summary>
        /// Parses the given ground and contents multi-line strings into a linked grid of <see cref="ILoloCell"/>.
        /// </summary>
        /// <param name="ground">A multi-line string describing the ground tiles. </param>
        /// <param name="contents">A multi-line string describing the contents on top of the ground. </param>
        /// <param name="x">Starting X coordinate (column index) for the top-left cell. Defaults to 0. This is advanced internally during parsing.</param>
        /// <param name="y">Starting Y coordinate (row index) for the top-left cell. Defaults to 0. This is advanced internally during parsing.</param>
        /// <returns>The head of a linked list of rows, each row being a linked list of cells, representing the full parsed level.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the width or height of <paramref name="ground"/> and <paramref name="contents"/> do not match.
        /// </exception>
        /// 
        /// <remarks>
        /// Termination argument:
        /// - The data we recurse over is remaining rows to parse, which starts as a string that cannot have a negative number of characters.
        /// - Removing the first row never makes the number of characters negative.
        /// - Each recursive call removes exactly one row.
        /// - Therefore the row count (the length of the rest of the strings) eventually reaches zero, so the recursion must terminate.
        /// </remarks>
        public static ILoloCell ParseBoard(string ground, string contents, int x=0, int y=0)
        {
            EnsureStringsConform(ground, contents);

            ground = ground.Trim();
            string groundRow = GetFirstRow(ground);
            string groundRest = RemoveFirstRow(ground);

            contents = contents.Trim();
            string contentRow = GetFirstRow(contents);
            string contentRest = RemoveFirstRow(contents);

            if (groundRest.Length == 0 && contentRest.Length == 0)
            {
                return new LinkLoloCell(ParseRow(groundRow, contentRow, 0, y), new EmptyLoloCell());
            }

            return new LinkLoloCell(ParseRow(groundRow, contentRow, x, y), ParseBoard(groundRest, contentRest, 0, y + 1));
        }

        /// <summary>
        /// Parses the given ground and content aligned rows into a linked list of cells (<see cref="ILoCell"/>).
        /// </summary>
        /// <param name="groundRow">A single line of ground characters</param>
        /// <param name="contentRow">A single line of content characters</param>
        /// <param name="x">The current X coordinate (column index) for the first character in the row.</param>
        /// <param name="y">The current Y coordinate (row index) corresponding to this row.</param>
        /// <returns>A linked list representing the parsed row.</returns>
        /// 
        /// <remarks>
        /// Termination argument:
        /// - The data is the number of remaining characters in the row, which starts over 0.
        /// - Substring(1) never produces a negative length.
        /// - Each recursive call trims one character from the left, reducing the length by one.
        /// - When only 1 character is left or there are no more characters (should not happen), recursion stops, so termination is guaranteed.
        /// </remarks>
        static ILoCell ParseRow(string groundRow, string contentRow, int x, int y)
        {
            string cleanGroundRow = groundRow.Replace(NEW_LINE_CHARACTER, string.Empty);
            char firstGround = cleanGroundRow[0];

            string cleanContentRow = contentRow.Replace(NEW_LINE_CHARACTER, string.Empty);
            char firstContent = cleanContentRow[0];

            if (cleanGroundRow.Length <= 1 && cleanContentRow.Length <= 1)
            {
                return new LinkLoCell(ParseCharactersToCell(firstGround, firstContent, x, y), new EmptyLoCell());
            }

            string groundRest = cleanGroundRow.Substring(1);
            string contentRest = cleanContentRow.Substring(1);

            return new LinkLoCell(ParseCharactersToCell(firstGround, firstContent, x, y), ParseRow(groundRest, contentRest, x + 1, y));
        }

        /// <summary>
        /// Ensures that the given ground and contents multi-line strings (considered as grids) have the same width and height.
        /// </summary>
        /// <param name="ground">A multi-line string describing the ground tiles. </param>
        /// <param name="contents">A multi-line string describing the contents on top of the ground. </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the width or height of <paramref name="ground"/> and <paramref name="contents"/> do not match.
        /// </exception>
        static void EnsureStringsConform(string ground, string contents)
        {
            if (GetWidth(ground) != GetWidth(contents))
            {
                throw new ArgumentException(WIDTH_MISMATCH_ERROR_MSG);
            }

            if (GetHeight(ground) != GetHeight(contents))
            {
                throw new ArgumentException(HEIGHT_MISMATCH_ERROR_MSG);
            }
        }

        /// <summary>
        /// Computes the height (number of rows) of a given multi-line string, where rows are delimited by "\n".
        /// </summary>
        /// <param name="rows">A multi-line string.</param>
        /// <returns>The number of rows in <paramref name="rows"/> (>= 1).</returns>
        /// 
        /// <remarks>
        /// Termination argument:
        /// - The data we recurse over is the remaining rows, which is never of negative length.
        /// - Each recursive call processes the string with its first row removed, decreasing the length of the string by one row.
        /// - Eventually no rows remain, so recursion must terminate.
        /// </remarks>
        static int GetHeight(string rows)
        {
            string withoutFirstRow = RemoveFirstRow(rows);

            if (withoutFirstRow.Length == 0)
            {
                return 1;
            }

            return 1 + GetHeight(withoutFirstRow);
        }

        /// <summary>
        /// Computes the width (number of columns) of the given row in a multi-line string.
        /// </summary>
        /// <param name="row">A multi-line string (only the first row is considered).</param>
        /// <returns>The length of the first row, excluding the newline character.</returns>
        static int GetWidth(string row)
        {
            return GetFirstRow(row).Trim().Replace(NEW_LINE_CHARACTER, "").Length;
        }

        /// <summary>
        /// Converts the given ground and content characters into a concrete <see cref="ICell"/>.
        /// </summary>
        /// <param name="groundChar">The ground character (<c>_</c> for floor, or a target color <c>B</c>/<c>R</c>/<c>Y</c>/<c>G</c>).</param>
        /// <param name="contentChar">The content character on top of the ground (<c>_</c>, <c>W</c>, <c>B</c>, trophy <c>b</c>/<c>r</c>/<c>y</c>/<c>g</c>, or player <c>^</c>/<c>&lt;</c>/<c>&gt;</c>/<c>v</c>).</param>
        /// <param name="x">The X coordinate (column index).</param>
        /// <param name="y">The Y coordinate (row index).</param>
        /// <returns>An <see cref="ICell"/> instance representing the combined ground/content at the specified location.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the given ground character is not in recognized set of ground characters (See Constants).
        /// </exception>
        static ICell ParseCharactersToCell(char groundChar, char contentChar, int x, int y)
        {
            if (groundChar == FLOOR_CHARACTER)
            {
                if (contentChar == FLOOR_CHARACTER)
                {
                    return new Floor(new EmptyContent(), new Point(x, y));
                }
                else
                {
                    return new Floor(CharacterToContent(contentChar), new Point(x, y));
                }
            }

            if (IsTargetCharacter(groundChar))
            {
                if (contentChar == FLOOR_CHARACTER)
                {
                    return new Target(CharToColor(groundChar), new EmptyContent(), new Point(x, y));
                } 
                else
                {
                    return new Target(CharToColor(groundChar), CharacterToContent(contentChar), new Point(x, y));
                }
            }

            throw new ArgumentException($"Unknown ground character : '{groundChar}'");
        }

        /// <summary>
        /// Determines whether the given character denotes a target color.
        /// </summary>
        /// <param name="character">The character to test.</param>
        /// <returns>true if the character is a target color character; otherwise, false.</returns>
        static bool IsTargetCharacter(char character)
        {
            string charStr = character.ToString();
            return charStr == BLUE_COLOR_CHARACTER
                || charStr == RED_COLOR_CHARACTER
                || charStr == YELLOW_COLOR_CHARACTER
                || charStr == GREEN_COLOR_CHARACTER;
        }

        /// <summary>
        /// Converts a given content character to a concrete <see cref="IContent"/> implementation.
        /// </summary>
        /// <param name="character">A content character.</param>
        /// <returns>A corresponding <see cref="IContent"/> instance (e.g., <see cref="Wall"/>, <see cref="Box"/>, <see cref="Trophy"/>, <see cref="Player"/>).</returns>
        /// <exception cref="ArgumentException">Thrown when the character is not a recognized Sokoban content symbol.</exception>
        static IContent CharacterToContent(char character)
        {
            return character.ToString() switch
            {
                WALL_CHARACTER => new Wall(),
                BOX_CHARACTER => new Box(),

                BLUE_TROPHY_CHARACTER or
                RED_TROPHY_CHARACTER or
                YELLOW_TROPHY_CHARACTER or
                GREEN_TROPHY_CHARACTER => new Trophy(CharToColor(character)),

                PLAYER_UP_CHARACTER or
                PLAYER_DOWN_CHARACTER or
                PLAYER_LEFT_CHARACTER or
                PLAYER_RIGHT_CHARACTER => new Player(),

                _ => throw new ArgumentException($"Unknown content character : '{character}'")
            };
        }

        /// <summary>
        /// Converts the given color character to a <see cref="Color"/>.
        /// </summary>
        /// <param name="character">The color character to convert (case-insensitive).</param>
        /// <returns>The corresponding <see cref="Colors.Blue"/>, <see cref="Colors.Red"/>, <see cref="Colors.Yellow"/>, or <see cref="Colors.Green"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the character is not a recognized color.</exception>
        static Color CharToColor(char character)
        {
            return character.ToString().ToUpper() switch
            {
                BLUE_COLOR_CHARACTER => Colors.Blue,
                RED_COLOR_CHARACTER => Colors.Red,
                YELLOW_COLOR_CHARACTER => Colors.Yellow,
                GREEN_COLOR_CHARACTER => Colors.Green,
                _ => throw new ArgumentException($"Unrecognized Sokoban color character: '{character}'")
            };
        }

        /// <summary>
        /// Returns the first row (substring up to but not including the first newline) from the given multi-line string.
        /// If no newline exists, returns the whole string.
        /// </summary>
        /// <param name="level">A multi-line string.</param>
        /// <returns>The first row of <paramref name="level"/>.</returns>
        static string GetFirstRow(string level)
        {
            if (level.IndexOf(NEW_LINE_CHARACTER) == -1)
            {
                return level.Substring(0);
            }
            return level.Substring(0, level.IndexOf(NEW_LINE_CHARACTER));
        }

        /// <summary>
        /// Removes the first row (up to and including the first newline) from the given multi-line string.
        /// If no newline exists, returns an empty string.
        /// </summary>
        /// <param name="level">A multi-line string.</param>
        /// <returns>The remainder of <paramref name="level"/> after removing its first row.</returns>
        static string RemoveFirstRow(string level)
        {
            if (level.IndexOf(NEW_LINE_CHARACTER) == -1)
            {
                return string.Empty;
            }
            return level.Substring(level.IndexOf(NEW_LINE_CHARACTER) + 1);
        }
    }
}
