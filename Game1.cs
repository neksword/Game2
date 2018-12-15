using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        Texture2D texture2;
        Texture2D textureCoin;
        Texture2D textureWin;
        Texture2D textureBoss;
        Vector2 position = Vector2.Zero;
        Vector2 positionCoin = Vector2.Zero;
        Vector2 positionBoss = Vector2.Zero;
        //
        int currentTime = 0; // сколько времени прошло
        int period = 50; // частота обновления в миллисекундах
        int frameWidth = 32;
        int frameHeight = 32;
        Point currentFrame = new Point(0, 0);
        Point spriteSize = new Point(8, 1);
        Color color = Color.White;  //цвет для фона
        //
        int i = 0;
        int j = 0;
        float speed = 5f;
        float speedBoss = 5f;

        //мышь
        MouseState lastMouseState;

        Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            position = new Vector2(100, 900);
            positionBoss = new Vector2(950, 70);
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //загрузка звуков

            song = Content.Load<Song>("Fon");

            // начинаем проигрывание мелодии
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            //загрузка текстур
            textureCoin = Content.Load<Texture2D>("coin");
            texture = Content.Load<Texture2D>("stena");
            texture2 = Content.Load<Texture2D>("pers");
            textureWin = Content.Load<Texture2D>("win");
            textureBoss = Content.Load<Texture2D>("boss");
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (keyboardState.IsKeyDown(Keys.Left))
                position.X -= speed;
            if (keyboardState.IsKeyDown(Keys.Right))
                position.X += speed;
            if (keyboardState.IsKeyDown(Keys.Up))
                position.Y -= speed;
            if (keyboardState.IsKeyDown(Keys.Down))
                position.Y += speed;

            //мышь

            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X ||
               currentMouseState.Y != lastMouseState.Y)
                position = new Vector2(currentMouseState.X, currentMouseState.Y);
            lastMouseState = currentMouseState;

            // добавляем к текущему времени прошедшее время
            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            // если текущее время превышает период обновления спрайта
            if (currentTime > period)
            {
                currentTime -= period; // вычитаем из текущего времени период обновления
                ++currentFrame.X; // переходим к следующему фрейму в спрайте
                if (currentFrame.X >= spriteSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= spriteSize.Y)
                        currentFrame.Y = 0;
                }
            }

            // анимация босса 
            positionBoss.X += speedBoss;
            if (positionBoss.X > 1400 - textureBoss.Width || positionBoss.X < 650)
                speedBoss *= -1;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Draw(textureBoss, positionBoss, Color.White); // BOSS

            spriteBatch.Draw(texture2, position, Color.White);

            if (Collide())                                                                      // WIN!!!!
                spriteBatch.Draw(textureWin, new Vector2((Window.ClientBounds.Width / 2) - (textureWin.Width / 2),
                                                        (Window.ClientBounds.Height / 2) - (textureWin.Height / 2)), Color.White);

            if (Collide2())                                                                      // boss!!!!
                spriteBatch.Draw(textureWin, new Vector2((Window.ClientBounds.Width / 2) - (textureWin.Width / 2),
                                                        (Window.ClientBounds.Height / 2) - (textureWin.Height / 2)), Color.White);

            //Рамка
            for (i = 0; i < 1950; i = i + 50)
            {
                spriteBatch.Draw(texture, new Vector2(i, 0), Color.White);
                spriteBatch.Draw(texture, new Vector2(0, i), Color.White);
                spriteBatch.Draw(texture, new Vector2(i, 1030), Color.White);
                spriteBatch.Draw(texture, new Vector2(1870, i), Color.White);
            }

            //Первый коридор
            for (j = 50; j < 650; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(1700, j + 150), Color.White);
                spriteBatch.Draw(texture, new Vector2(1400, j), Color.White);
            }

            //Ловушка
            for (j = 50; j < 550; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(1000, j + 145), Color.White);
                spriteBatch.Draw(texture, new Vector2(j + 730, 320), Color.White);
            }
            //Ловушка коридор

            for (j = 50; j < 300; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(j + 920, 645), Color.White);
            }

            //карман 1
            for (j = 50; j < 170; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(1230, j + 150), Color.White);
            }

            //карман 2
            for (j = 50; j < 180; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(780, j + 120), Color.White);
            }


            for (j = 50; j < 500; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(j, 170), Color.White);
                spriteBatch.Draw(texture, new Vector2(600, j), Color.White);
                spriteBatch.Draw(texture, new Vector2(j + 120, 340), Color.White);
                spriteBatch.Draw(texture, new Vector2(j + 120, 500), Color.White);
                spriteBatch.Draw(texture, new Vector2(j + 400, 500), Color.White);
            }

            for (j = 50; j < 1750; j = j + 50)
            {
                spriteBatch.Draw(texture, new Vector2(j, 800), Color.White);
            }

            spriteBatch.Draw(textureCoin, new Vector2(90, 90),
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                     Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);


            spriteBatch.End();
            base.Draw(gameTime);
        }
        // пересечение с монеткой
        protected bool Collide()
        {
            Rectangle pers = new Rectangle((int)position.X,
                (int)position.Y, 70, 100);
            Rectangle coin = new Rectangle((int)90,
                (int)90, 32, 32);

            return pers.Intersects(coin);
        }
        //Пересечение с боссом
        protected bool Collide2()
        {
            Rectangle pers = new Rectangle((int)position.X,
                (int)position.Y, 70, 100);
            Rectangle boss = new Rectangle((int)positionBoss.X,
                (int)positionBoss.Y, 91, 96);

            return pers.Intersects(boss);
        }
    }
}