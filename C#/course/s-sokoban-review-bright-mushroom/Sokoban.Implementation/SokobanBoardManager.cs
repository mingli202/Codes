using Sokoban.Lists;
using ImageLib.FunWorld;
using ImageLib.Interfaces;
using Sokoban.Enums;
using Sokoban.Objects;
using ImageLib;

namespace Sokoban
{
    public class SokobanBoardManager : World
    {
        private const int DEFAULT_CELL_SIZE = 64;

        readonly ILoloCell board;
        readonly int cellSize;

        public SokobanBoardManager(ILoloCell board, int cellSize = DEFAULT_CELL_SIZE)
        {
            this.board = board;
            this.cellSize = cellSize;
        }

        public SokobanBoardManager(string groundLevel, string contentLevel, int cellSize=DEFAULT_CELL_SIZE) 
            : this(StringBoardParser.ParseBoard(groundLevel, contentLevel), cellSize) { }

        protected override World OnKeyDown(string key)
        => key.ToLowerInvariant() switch
        {
            "w" or "up" => MovePlayer(PlayerFacingEnum.UP),
            "s" or "down" => MovePlayer(PlayerFacingEnum.DOWN),
            "a" or "left" => MovePlayer(PlayerFacingEnum.LEFT),
            "d" or "right" => MovePlayer(PlayerFacingEnum.RIGHT),
            _ => this
        };

        /// <summary>
        /// Move the player in the given direction, if possible. If the move is not possible, return the same board.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private World MovePlayer(PlayerFacingEnum direction)
        {
            Point playerPos = this.board.GetPlayerPosition();
            Point newPlayerPos = playerPos.MovePoint(direction);
            ILoloCell newBoard = this.board.MovePlayer(playerPos, newPlayerPos);

            return new SokobanBoardManager(newBoard, this.cellSize);
        }

        /// <summary>
        /// Initializes the game by starting the world with the appropriate dimensions based on the board size and cell size.
        /// </summary>
        public bool StartGame()
        {
            return this.BigBang(this.board.Width() * this.cellSize, this.board.Height() * this.cellSize);
        }

        /// <summary>
        /// Converts the current board state into a WorldImage that can be rendered in the game. Each cell is drawn as a square with the specified side length.
        /// </summary>
        /// <returns></returns>
        public WorldImage ConvertToWorldImage()
        {
            return board.DrawLoLoCell(this.cellSize);
        }

        protected override IWorldScene MakeScene()
        {
            return WorldScene.FromImage(ConvertToWorldImage());
        }

        /// <summary>
        /// Returns true if the level has been won, which is determined by checking if all goals are satisfied (i.e., all boxes are on their corresponding targets). Otherwise, returns false.
        /// </summary>
        public bool LevelWon()
        {
            return this.board.IsLevelWon();
        }
    }
}

