using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace MandelBrot
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MandelBrot : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const int xRes = 2046;
        const int yRes = 1364;
        const int MAXITERATION = 5000;
        //uint array faster than color array
        uint[] mandelColors = new uint[yRes*xRes];
        public MandelBrot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            //hattu vakio
            const double vakioKerroin = 0.75;
            //creates 4 tasks
            Task segmentOne = new Task(delegate
            {
                //divides the y axis to 4 segments which aren't exactly even so tasks end at about the same time
                for (int y = 0; y < (int)Math.Floor(yRes / 2.0 * vakioKerroin); y += 1)
                {
                    for (int x = 0; x < xRes; x += 1)
                    {
                        mandelColors[x+y * xRes] = gen.MandelBrotGen(x, y, xRes, yRes,MAXITERATION);

                    }
                }
            });

            Task segmentTwo = new Task(delegate
            {
                //divides the y axis to 4 segments which aren't exactly even so tasks end at about the same time
                for (int y = (int)Math.Floor(yRes / 2.0 * vakioKerroin); y < (int)Math.Floor(yRes / 2.0); y += 1)
                {
                    for (int x = 0; x < xRes; x += 1)
                    {
                        mandelColors[x + y * xRes] = gen.MandelBrotGen(x, y, xRes, yRes, MAXITERATION);
                    }
                }
            });
            Task segmentThree = new Task(delegate
            {
                //divides the y axis to 4 segments which aren't exactly even so tasks end at about the same time
                for (int y = (int)Math.Floor(yRes / 2.0); y < (int)Math.Floor(yRes * 0.60); y += 1)
                {
                    for (int x = 0; x < xRes; x += 1)
                    {
                        mandelColors[x + y * xRes] = gen.MandelBrotGen(x, y, xRes, yRes, MAXITERATION);
                    }
                }
            });
            Task segmentFour = new Task(delegate
            {
                //divides the y axis to 4 segments which aren't exactly even so tasks end at about the same time
                for (int y = (int)Math.Floor(yRes * 0.60); y < (int)Math.Floor(yRes * 1.0); y += 1)
                {
                    for (int x = 0; x < xRes; x += 1)
                    {
                        mandelColors[x + y * xRes] = gen.MandelBrotGen(x, y, xRes, yRes, MAXITERATION);
                    }
                }
            });
            //outputs the time of each task
            segmentFour.ContinueWith(delegate { Console.WriteLine("4 ready" + watch.ElapsedMilliseconds); });
            segmentThree.ContinueWith(delegate { Console.WriteLine("3 ready" + watch.ElapsedMilliseconds); });
            segmentTwo.ContinueWith(delegate { Console.WriteLine("2 ready" + watch.ElapsedMilliseconds); });
            segmentOne.ContinueWith(delegate { Console.WriteLine("1 ready" + watch.ElapsedMilliseconds); });
            //starts calculation
            segmentOne.Start();
            segmentTwo.Start();
            segmentThree.Start();
            segmentFour.Start();
            Task.WaitAll(segmentOne, segmentTwo, segmentThree, segmentFour);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            //Camera.Position = new Vector(-800, 0);
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            Texture2D mandelImage = new Texture2D(graphics.GraphicsDevice, xRes, yRes);
            mandelImage.SetData(mandelColors);
            spriteBatch.Begin();
            spriteBatch.Draw(mandelImage, new Vector2(0, 0));
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
