using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MyGame.Graphics;
using MyGame.World.Objects;

namespace MyGame.World
{
    public class GameWorld
    {
        private ObjectManager objectManager;

        public Player playerObject;

        public GameWorld()
        {
            objectManager = new ObjectManager();
        }


        public void Create(ContentManager contentManager)
        {
            playerObject = objectManager.CreateGameObject<Player>();
        }

        public void Update(GameTime gameTime)
        {
            objectManager.Update(gameTime);
        }

        public void Draw(GameTime gameTime, MySpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(new Rectangle(0, 0, 10, 10), Color.Red);

            objectManager.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
