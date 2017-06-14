using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Common.Network;

namespace MyGame.World.Objects
{
    public class Player : Character
    {
        private NetSerializator serializator = new NetSerializator();

        override public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            bool up = kbState.IsKeyDown(Keys.W);
            bool down = kbState.IsKeyDown(Keys.S);
            bool left = kbState.IsKeyDown(Keys.A);
            bool right = kbState.IsKeyDown(Keys.D);

            double speed = 100.0;

            if (up)
            {
                Y = Y - speed * gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (down)
            {
                Y = Y + speed * gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (left)
            {
                X = X - speed * gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (right)
            {
                X = X + speed * gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kbState.IsKeyDown(Keys.Space) && serializator.IsEmpty())
            {
                Serialize(serializator);
            }

            if (kbState.IsKeyDown(Keys.LeftAlt))
            {
                NetSerializator deserializator = new NetSerializator(serializator.GetBuffer(), 0, (int)serializator.GetSize());
                Deserialize(deserializator);
                serializator.Clear();
            }
        }

    }
}
