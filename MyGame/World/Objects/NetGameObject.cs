using Microsoft.Xna.Framework;

using MyGame.Graphics;
using Common.Network;

namespace MyGame.World.Objects
{
    abstract public class NetGameObject : GameObject
    {
        public static uint INVALID_NET_ID = 0xFFFFFFFF;

        public uint networkId = INVALID_NET_ID;


        virtual public bool Serialize(NetSerializator serializator)
        {
            return true;
        }

        virtual public bool Deserialize(NetSerializator serializator)
        {
            return true;
        }

        // GameObject interface
        override abstract public void Draw(GameTime gameTime, MySpriteBatch spriteBatch);
        override abstract public void Update(GameTime gameTime);
        // Game object interface end
    }
}
