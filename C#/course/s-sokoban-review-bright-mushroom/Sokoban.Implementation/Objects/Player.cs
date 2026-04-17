using System;
using System.Collections.Generic;
using System.Text;
using ImageLib;
using Sokoban.Interfaces;

namespace Sokoban.Objects
{
    public class Player : AContent
    {
        public Player() { }
        public override WorldImage DrawContent(int size)
        {
            return ScaleImageToSize("./Assets/Player.png", size);
        }

        public override bool IsPlayer() => true;
    }
}

