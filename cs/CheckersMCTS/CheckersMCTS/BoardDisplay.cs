using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMCTS
{
    internal class BoardDisplay
    {
        // Size
        private const int SquareWidth = 70;
        private const int CheckerWidth = 40;
        public int Width => SquareWidth * BoardState.BoardSize;

        // Board state
        public CheckersState BoardState { get; }
        public List<CheckerSprite> CheckerSprites { get; set; }

        // Gameplay state
        private enum CheckersGameState
        {
            SelectPiece,
            SelectMove,
            GameOver,
            EditingBoard
        }
        private CheckersGameState GameplayState;
        private MouseState PrevMouse;


        public BoardDisplay(CheckersState state)
        {
            GameplayState = CheckersGameState.SelectPiece;
            BoardState = state;

            CheckerSprites = new List<CheckerSprite>();
            foreach (Checker checker in state.Checkers)
            {
                int x = (int)(SquareWidth * checker.Coordinates.X) + (SquareWidth - CheckerWidth) / 2;
                int y = (int)(SquareWidth * checker.Coordinates.Y) + (SquareWidth - CheckerWidth) / 2;
                Rectangle pos = new(x, y, CheckerWidth, CheckerWidth);
                Rectangle hitbox = new(x, y, SquareWidth, SquareWidth);
                CheckerSprites.Add(new CheckerSprite(pos, hitbox, checker));
            }
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            Point? click = mouseState.LeftButton == ButtonState.Pressed && PrevMouse.LeftButton == ButtonState.Released ? mouseState.Position : null;
            switch(GameplayState)
            {
                case CheckersGameState.SelectPiece:
                    break;
                case CheckersGameState.SelectMove:
                case CheckersGameState.GameOver:
                case CheckersGameState.EditingBoard:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Draw(SpriteBatch spriteBatch)   
        {
            bool isLight = true;
            for (int x = 0; x < BoardState.BoardSize; x++)
            {
                for (int y = 0; y < BoardState.BoardSize; y++)
                {
                    spriteBatch.FillRectangle(x * SquareWidth, y * SquareWidth, SquareWidth, SquareWidth, isLight ? Color.BurlyWood : Color.Brown);
                    isLight = !isLight;
                }
                isLight = !isLight;
            }

            foreach (CheckerSprite checkerSprite in CheckerSprites)
            {
                checkerSprite.Draw(spriteBatch);
            }
        }
    }
}
