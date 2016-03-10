using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;


/// <summary>
/// This is the main type for your game.
/// </summary>
public class TestGame : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;
    

    CameraActor _camera;

    private ArrayList _cubeActors = new ArrayList();

    private float _cameraSpeed = 40f;

    public TestGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        _graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
        _graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
        _graphics.ApplyChanges();
        this.Window.Position = new Point(10, 10);
        // create camera
        _camera = new CameraActor();

        Console.WriteLine("Game initialized");

    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        // create a new SpriteBatch, which can be used to draw textures
        //_spriteBatch = new SpriteBatch(GraphicsDevice);
        Effect effect = Content.Load<Effect>("Effects/Test");
        // create model actors
        Model cube = Content.Load<Model>("Green_Cube");
        for (int i = -10; i < 10; i++)
            for (int j = -10; j < 10; j++)
                for (int k = -4; k < 4; k++)
                {
                    if (i == 0 && j == 0 && k == 0) { } else
                        _cubeActors.Add(new ModelActor(Content.Load<Model>("Green_Cube"),effect, new Vector3(i * 30f, k * 30f, j * 30f), new Vector3(0f, 0f, 0f), new Vector3(0.3f, 0.3f, 0.3f)));
                }
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

        updateCamera((float)gameTime.ElapsedGameTime.TotalSeconds);
            
        base.Update(gameTime);

    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        //TODO activate depthtest?!? not really working but better as without it
        GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
        GraphicsDevice.RasterizerState = RasterizerState.CullNone;

        foreach (ModelActor actor in _cubeActors)
        {
            actor.draw(_camera);
        }

       base.Draw(gameTime);
    }

    private void updateCamera(float deltaTime)
    {
        float deltaCameraY = 0;
        float deltaCameraX = 0;
        float cameraForward = 0f;
        float cameraSideward = 0f;

        // adjust aspect ratio
        _camera.setAspectRatio(_graphics.GraphicsDevice.Viewport.AspectRatio);

        // mouse look
        MouseState mouseState = Mouse.GetState();

        deltaCameraY = (mouseState.X - 100) / 1000f;
        deltaCameraX = (mouseState.Y - 100) / 1000f;

        // reset mouse
        Mouse.SetPosition(100, 100);

        // movement
        KeyboardState k_state = Keyboard.GetState();
        if (k_state.IsKeyDown(Keys.W))
        {
            if (k_state.IsKeyDown(Keys.LeftShift)) cameraForward = 2f;
            else cameraForward = 1f;
        }
        else if (k_state.IsKeyDown(Keys.S))
        {
            if (k_state.IsKeyDown(Keys.LeftShift)) cameraForward = -2f;
            else cameraForward = -1f;
        }
        else
            cameraForward = 0f;
        if (k_state.IsKeyDown(Keys.A)) cameraSideward = -1f;
        else if (k_state.IsKeyDown(Keys.D)) cameraSideward = 1f;

        if (k_state.IsKeyDown(Keys.Up))
            _camera.moveUpward(_cameraSpeed * deltaTime);
        if (k_state.IsKeyDown(Keys.Down))
            _camera.moveUpward(-_cameraSpeed * deltaTime);
        if (k_state.IsKeyDown(Keys.Left)) _camera.rotate(0f, 0f, -0.03f * _cameraSpeed * deltaTime);
        else if (k_state.IsKeyDown(Keys.Right)) _camera.rotate(0f, 0f, 0.03f * _cameraSpeed * deltaTime);

        // update camera
        _camera.moveForward(cameraForward * _cameraSpeed * deltaTime);
        _camera.rotate(deltaCameraX, deltaCameraY, 0f);
        _camera.moveSideward(cameraSideward * _cameraSpeed * deltaTime);

        // test for collisions
        foreach (ModelActor actor in _cubeActors)
        {
            if (_camera.isColliding(actor))
            {
                _camera.moveForward(-cameraForward * _cameraSpeed * deltaTime);
                _camera.moveSideward(-cameraSideward * _cameraSpeed * deltaTime);
            }
        }
    }
}
