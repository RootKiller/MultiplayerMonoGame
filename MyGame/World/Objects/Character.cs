using Microsoft.Xna.Framework;

using MyGame.Graphics;
using Common.Network;
using System;

namespace MyGame.World.Objects
{
    public class Character : NetGameObject
    {
        public double X;
        public double Y;

        public byte Health = 100;

        override public bool Serialize(NetSerializator serializator)
        {
            return serializator.Write(X)
                && serializator.Write(Y)

                && serializator.Write(Health);
        }

        override public bool Deserialize(NetSerializator serializator)
        {
            return serializator.Read(ref X)
                && serializator.Read(ref Y)

                && serializator.Read(ref Health);
        }

        override public void Update(GameTime gameTime)
        {

        }

        override public void Draw(GameTime gameTime, MySpriteBatch spriteBatch)
        {
            spriteBatch.Draw(new Rectangle((int)X, (int)Y, 30, 30), Color.Black);
        }
    }
}
