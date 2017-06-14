
using Microsoft.Xna.Framework;

using System.Collections;

using MyGame.Graphics;

namespace MyGame.World.Objects
{
    class ObjectManager
    {
        private ArrayList objects = new ArrayList();


        public T CreateGameObject<T>() where T : GameObject, new()
        {
            T go = new T();
            objects.Add(go);
            return go;
        }

        public void DestroyGameObject(GameObject go)
        {
            objects.Remove(go);
        }

        public T GetObject<T>(int index) where T : GameObject
        {
            return (T)objects[index];
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject go in objects)
            {
                go.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, MySpriteBatch spriteBatch)
        {
            foreach (GameObject go in objects)
            {
                go.Draw(gameTime, spriteBatch);
            }
        }
    }
}
