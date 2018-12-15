using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Maps
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        Texture2D boxTexture1;
        Texture2D boxTexture2;
        Texture2D texturepers;
        Texture2D textureCoin;
        Texture2D textureWin;
        Texture2D textureBoss;
        Texture2D textureSprite;
        Texture2D textureStone;

        float scale = 1f;

        Vector2 position = Vector2.Zero;
        Vector2 positionCoin = Vector2.Zero;
        Vector2 positionBoss = Vector2.Zero;
        Vector2 positionBoss2 = new Vector2(700, 200);

        Vector2[,] map2; //координаты стен
        Vector2 h;
        SpriteFont textBlock;
        int currentTime = 0; // сколько времени прошло
        int period = 50; // частота обновления в миллисекундах
        int frameWidth = 32;
        int frameHeight = 32;
        int frameWidth2 = 47;
        int frameHeight2 = 99;
        int persWidth = 47;
        int persHeight = 99;
        Point currentFrame = new Point(0, 0);
        Point spriteSize = new Point(8, 1);
        Point currentFrame2 = new Point(0, 0);
        Point spriteSize2 = new Point(8, 1);
        Color color = Color.White;  //цвет для фона

     //для таймера
        double seconds = 0;
        int frames = 0;


        float speed = 5f;
        float speedBoss = 8f;

        int NumberMap = 1;
        int bossLevel = 1;
        List<Box> boxs;

       

        //мышь
        MouseState lastMouseState;
     

        KeyboardState keys;
        KeyboardState Oldkeys;

        Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            map2 = new Vector2[36, 63];

            position = new Vector2(100, 900);
            positionBoss = new Vector2(950, 70);
            IsMouseVisible = false;

            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures. 
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //загрузка звуков
            song = Content.Load<Song>("Fon");

            // начинаем проигрывание мелодии
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            //загрузка текстур
            textureCoin = Content.Load<Texture2D>("coin");
            texturepers = Content.Load<Texture2D>("pers");
            textureWin = Content.Load<Texture2D>("win");
            textureBoss = Content.Load<Texture2D>("boss");
            boxTexture1 = Content.Load<Texture2D>("box1");
            boxTexture2 = Content.Load<Texture2D>("box2");
            textureSprite = Content.Load<Texture2D>("sprite");
            textureStone = Content.Load<Texture2D>("stone");

            //счетчик
            textBlock = Content.Load<SpriteFont>("count");


            // TODO: use this.Content to load your game content here 
            CreateMaps();
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
        }

        void CreateMaps()
        {

            boxs = new List<Box>();

            int[,] map;



            if (NumberMap == 1)
            {
                bossLevel = 1;

                //спаун после босса
                if (Collide2())
                {
                    position = new Vector2(100, 900);
                    scale = 1f;
                    persWidth = 47;
                    persHeight = 99;
                    frames = 0;
                }
                map = new int[,]    {{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,1,1,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,0,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,0,0,0,1,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,1,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,1,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,2},
                                     {2,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2},
                                     {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}};

                int x = 0;
                int y = 0;

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        Rectangle rect = new Rectangle(x, y, 30, 30);
                        int a = map[i, j];

                        if (a == 1)
                        {
                            map2[i, j] = new Vector2(x, y);
                            Box box = new Box(boxTexture1, rect);
                            boxs.Add(box);
                        }

                        else if (a == 2)
                        {
                            map2[i, j] = new Vector2(x, y);
                            Box box = new Box(boxTexture2, rect);
                            boxs.Add(box);
                        }
                        else
                            map2[i, j] = new Vector2(0, 0);
                        x += 30;
                    }
                    x = 0;
                    y += 30;
                }

                for (int i = 0; i < map2.GetLength(0); i++)                //столкновение со стеной
                    for (int j = 0; j < map2.GetLength(1); j++)
                    {
                        if (map2[i, j] != map2[2, 2])
                            h = map2[i, j];
                        if (Collide3())
                        {
                            position = new Vector2(100, 900);
                            scale = 1f;
                            persWidth = 47;
                            persHeight = 99;
                            frames = 0;
                        }

                    }
            }

            else if (NumberMap == 2)
            {
                //спаун после босса
                if (Collide2_1())
                {
                    position = new Vector2(100, 900);
                    scale = 1f;
                    persWidth = 47;
                    persHeight = 99;
                    frames = 0;
                }
                bossLevel = 2;
                map = new int[,]    {{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                                     {2,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                                     {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}};

                int x = 0;
                int y = 0;

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        Rectangle rect = new Rectangle(x, y, 30, 30);
                        int a = map[i, j];

                        if (a == 1)
                        {
                            map2[i, j] = new Vector2(x, y);
                            Box box = new Box(boxTexture1, rect);
                            boxs.Add(box);
                        }

                        else if (a == 2)
                        {
                            map2[i, j] = new Vector2(x, y);
                            Box box = new Box(boxTexture2, rect);
                            boxs.Add(box);
                        }
                        else
                            map2[i, j] = new Vector2(0, 0);
                        x += 30;
                    }

                    x = 0;
                    y += 30;
                }
                for (int i = 0; i < map2.GetLength(0); i++)                //столкновение со стеной
                    for (int j = 0; j < map2.GetLength(1); j++)
                    {
                        if (map2[i, j] != map2[2, 2])
                            h = map2[i, j];
                        if (Collide3())
                        {
                            position = new Vector2(100, 900);
                            scale = 1f;
                            persWidth = 47;
                            persHeight = 99;
                            frames = 0;
                        }
                    }
            }
        }
        

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here 
            //   if (bossLevel == 2)
            //        content.Unload();

        }

    
        protected override void Update(GameTime gameTime)
        {
            frames = frames + 1;
            
            for (int i = 0; i < map2.GetLength(0); i++)
            {
                for (int j = 0; j < map2.GetLength(1); j++)
                
                    if (map2[i, j]!= map2[2, 2])
                        h = map2[i, j];
                        if (Collide3())
                            position = new Vector2(100, 900);             
                                                   
            }
            // столкновение со бонусом
            if (Collide4())
            {
                persWidth = 33;
                persHeight = 70;
                scale = 0.7f;                
            }
            

            KeyboardState keyboardState = Keyboard.GetState();
            // Allows the game to exit 
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Left))
                position.X -= speed;
            if (keyboardState.IsKeyDown(Keys.Right))
                position.X += speed;
            if (keyboardState.IsKeyDown(Keys.Up))
                position.Y -= speed;
            if (keyboardState.IsKeyDown(Keys.Down))
                position.Y += speed;

            keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.F) && Oldkeys.IsKeyUp(Keys.F))
            {
                NumberMap++;
                position = new Vector2(100, 900);
                frames = 0;
            }
            if (keys.IsKeyDown(Keys.D) && Oldkeys.IsKeyUp(Keys.D))
            {
                NumberMap = NumberMap - 1;
                position = new Vector2(100, 900);
                frames = 0;

            }
            if (keys.IsKeyDown(Keys.R) && Oldkeys.IsKeyUp(Keys.R))
            {
                position = new Vector2(100, 900);
                frames = 0;
            }

            Oldkeys = keys;
            CreateMaps();
            /*
            //мышь
            MouseState currentMouseState = Mouse.GetState();

              if (currentMouseState.X != lastMouseState.X ||
                   currentMouseState.Y != lastMouseState.Y)
                    position = new Vector2(currentMouseState.X, currentMouseState.Y);
                lastMouseState = currentMouseState;
           */
            // проверяем, не убежал ли наш спрайт с игрового поля
            if (position.X < 30)
                position.X = 30;
            if (position.Y < 30)
                position.Y = 30;
            if (position.X > Window.ClientBounds.Width - 130)
                position.X = Window.ClientBounds.Width - 130;
            if (position.Y > Window.ClientBounds.Height - 130)
                position.Y = Window.ClientBounds.Height - 130;

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
            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            // если текущее время превышает период обновления спрайта
            if (currentTime > period)
            {
                currentTime -= period; // вычитаем из текущего времени период обновления
                ++currentFrame2.X; // переходим к следующему фрейму в спрайте
                if (currentFrame2.X >= spriteSize2.X)
                {
                    currentFrame2.X = 0;
                    ++currentFrame2.Y;
                    if (currentFrame2.Y >= spriteSize2.Y)
                        currentFrame2.Y = 0;
                }
            }
           
                       
            // анимация босса 
            if (bossLevel == 1)
            {
                positionBoss.X += speedBoss;
                if (positionBoss.X > 1400 - 91 || positionBoss.X < 650)
                    speedBoss *= -1;
            }
            else if (bossLevel == 2)
            {

                positionBoss2.Y += speedBoss;
                if (positionBoss2.Y > 800 - 96 || positionBoss2.Y < 160)
                    speedBoss *= -1;
            }
            base.Update(gameTime);
        }


       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

           

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(textureStone, new Vector2(0,0), Color.White);
            spriteBatch.Draw(textureSprite, new Vector2(1125, 465), Color.White);
            spriteBatch.DrawString(textBlock, "Time:" + seconds, new Vector2(1550, 50), Color.Red);
            if (bossLevel == 1)
                spriteBatch.Draw(textureBoss, positionBoss, Color.White);                             // BOSS
            else if (bossLevel == 2)
                spriteBatch.Draw(textureBoss, positionBoss2, Color.White);
            else if (bossLevel == 3)
                spriteBatch.Draw(textureBoss, positionBoss2, Color.White);    //переделать на positionBoss3


            spriteBatch.Draw(texturepers, position, 
                new Rectangle(currentFrame2.X * frameWidth2,
                    currentFrame2.Y * frameHeight2,
                    frameWidth2, frameHeight2),
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0);
          



            foreach (Box box in boxs)                   // class
            {
                box.Draw(spriteBatch);
            }

            spriteBatch.Draw(textureCoin, new Vector2(90, 90),
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                    Color.White, 0, Vector2.Zero,
                    1, SpriteEffects.None, 0);

            if (Collide())                                                                      // WIN!!!!
                spriteBatch.Draw(textureWin, new Vector2((Window.ClientBounds.Width / 2) - (textureWin.Width / 2),
                                                        (Window.ClientBounds.Height / 2) - (textureWin.Height / 2)), Color.White);
            else
                seconds = frames / 60.0;

            spriteBatch.End();

            base.Draw(gameTime);
        }
        // пересечение с монеткой
        protected bool Collide()
        {
            Rectangle pers = new Rectangle((int)position.X,
                (int)position.Y, persWidth, persHeight);
            Rectangle coin = new Rectangle((int)90,
                (int)90, 32, 32);

            return pers.Intersects(coin);
        }
        //Пересечение с боссом
        protected bool Collide2()
        {
            Rectangle pers = new Rectangle((int)position.X,
                (int)position.Y, persWidth, persHeight);
            Rectangle boss = new Rectangle((int)positionBoss.X,
                (int)positionBoss.Y, 91, 96);

            return pers.Intersects(boss);
        }
        protected bool Collide2_1()
        {
            Rectangle pers = new Rectangle((int)position.X,
                (int)position.Y, persWidth, persHeight);
            Rectangle boss = new Rectangle((int)positionBoss2.X,
                (int)positionBoss2.Y, 91, 96);

            return pers.Intersects(boss);
        }
        //Пересечение со стеной
        protected bool Collide3()
        {
            
                Rectangle pers = new Rectangle((int)position.X,
                       (int)position.Y, persWidth, persHeight);
                Rectangle box = new Rectangle((int)h.X,
                       (int)h.Y, 30, 30);
                return pers.Intersects(box);
        }
        //Пересечение со спрайтом
        protected bool Collide4()
        {

            Rectangle pers = new Rectangle((int)position.X,
                   (int)position.Y, persWidth, persHeight);
            Rectangle sprite = new Rectangle(1125, 465, 16, 50);
            return pers.Intersects(sprite);
        }
    }
}