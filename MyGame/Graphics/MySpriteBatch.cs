using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Graphics
{
    public class MySpriteBatch : SpriteBatch
    {
        private Texture2D whiteTexture;

        public MySpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            whiteTexture = new Texture2D(graphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(Rectangle rectangle, Color color)
        {
            Draw(whiteTexture, rectangle, color);
        }
    }
}
