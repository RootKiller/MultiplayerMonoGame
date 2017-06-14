using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyGame.World;
using MyGame.Graphics;
using MyGame.Network;

using Common.Network;
using System;

namespace MyGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TheGame : Game
    {
        GraphicsDeviceManager graphics;
        MySpriteBatch spriteBatch;

        private GameWorld gameWorld;

        private Client client;

        public TheGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            client = new Client();
            client.Connect("127.0.0.1", 7991);

            //client.RegisterPacketHandler(NetConstants.PacketId.CharacterSyncAck, (NetPacket packet) =>
            //{
            //    System.Console.WriteLine("ACK!");
            //});

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            gameWorld = new GameWorld();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new MySpriteBatch(GraphicsDevice);

            gameWorld.Create(this.Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            client.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
         
            client.Update(gameTime);

            gameWorld.Update(gameTime);

           // NetSerializator serializator = new NetSerializator();
            //gameWorld.playerObject.Serialize(serializator);
            //client.Send(NetConstants.PacketId.CharacterSync, serializator);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            gameWorld.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
