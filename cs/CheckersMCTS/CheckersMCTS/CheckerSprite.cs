using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMCTS
{
    internal class CheckerSprite
    {
        public Rectangle Position { get; set; }
        public Rectangle Hitbox { get; set; }
        public Checker Checker { get; set; }

        public CheckerSprite(Rectangle position, Rectangle hitbox, Checker checker)
        {
            Position = position;
            Hitbox = hitbox;
            Checker = checker;
        }

        public bool IsClicked(Vector2 mousePosition)
        {
            return Position.Contains(mousePosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Checker.Type.HasFlag(CheckerType.Red) ? Color.Red: Color.White;
            spriteBatch.FillRectangle(Position, color);
        }
    }
}
