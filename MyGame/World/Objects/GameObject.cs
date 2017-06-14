using Microsoft.Xna.Framework;
using MyGame.Graphics;

namespace MyGame.World.Objects
{
    public abstract class GameObject
    {
        abstract public void Draw(GameTime gameTime, MySpriteBatch spriteBatch);
        abstract public void Update(GameTime gameTime);
    }
}
