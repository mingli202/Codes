using Sokoban.Enums;
using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sokoban.Objects
{
    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private Point MovePoint(int dx, int dy)
        {
            return new Point(x + dx, y + dy);
        }

        public Point MovePoint(PlayerFacingEnum direction)
        {
            return direction switch
            {
                PlayerFacingEnum.LEFT => MovePoint(-1, 0),
                PlayerFacingEnum.UP => MovePoint(0, -1),
                PlayerFacingEnum.RIGHT => MovePoint(1, 0),
                PlayerFacingEnum.DOWN => MovePoint(0, 1),
                _ => throw new ArgumentException("Unsupportes direction")
            };
        }
    }
}
