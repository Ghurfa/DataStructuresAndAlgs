using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMCTS
{
    internal enum CheckerType
    {
        MovesUp     = 0b010,
        MovesDown   = 0b100,
        Red         = 0b001,
        WhitePawn   = MovesUp,
        RedPawn     = MovesDown & Red,
        King        = MovesUp & MovesDown,
        WhiteKing   = King,
        RedKing     = King & Red,
    }

    internal record Checker(Point Coordinates, CheckerType Type);
}
