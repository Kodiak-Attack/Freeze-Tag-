using AO;

public partial class MyPlayer : Player
{
    public Sprite_Renderer PlayerSprite;
    public GrassMapManager MapManager;
    public Vector2 TargetPosition;
    public bool IsMoving = false;
    public float MoveSpeed = 3.0f;
    public CameraControl CameraControl;
    
    public override void Awake()
    {
        // Create player sprite
        PlayerSprite = Entity.AddComponent<Sprite_Renderer>();
        PlayerSprite.Tint = new Vector4(1.0f, 0.5f, 0.0f, 1.0f); // Orange color for player
        Entity.LocalScale = new Vector2(0.8f, 0.8f);
        
        // Set initial position
        Entity.Position = new Vector2(0, 0);
        TargetPosition = Entity.Position;
        
        // Find the map manager
        foreach (var manager in Scene.Components<GrassMapManager>())
        {
            MapManager = manager;
            break;
        }
        
        // Add camera control for local player following cursorrules
        if (IsLocal)
        {
            CameraControl = CameraControl.Create(0);
            CameraControl.Zoom = 1.45f;
        }
        
        Log.Info($"Player spawned at {Entity.Position}");
    }
    
    public override void Update()
    {
        if (!IsLocal) return;
        
        // Handle movement input
        HandleMovementInput();
        
        // Update movement
        UpdateMovement();
    }
    
    public override void LateUpdate()
    {
        if (IsLocal && CameraControl != null)
        {
            CameraControl.Position = Vector2.Lerp(CameraControl.Position, Entity.Position + new Vector2(0, 0.5f), 0.5f);
        }
    }
    
    public void HandleMovementInput()
    {
        // Simple movement pattern for testing
        // In a real implementation, you'd use the engine's input system
        float time = Time.TimeSinceStartup;
        
        // Move in a figure-8 pattern for more interesting testing
        float radius = 3.0f;
        float speed = 0.5f;
        Vector2 newTarget = new Vector2(
            MathF.Sin(time * speed) * radius,
            MathF.Sin(time * speed * 2) * radius * 0.5f
        );
        
        // Check if the target position is walkable
        if (MapManager != null && MapManager.IsPositionWalkable(newTarget))
        {
            TargetPosition = newTarget;
            IsMoving = true;
        }
    }
    
    public void UpdateMovement()
    {
        if (IsMoving)
        {
            // Simple teleport to target for now
            Entity.Position = TargetPosition;
            IsMoving = false;
        }
    }
    
    // ClientRpc to show player movement on other clients
    [ClientRpc]
    public void UpdatePlayerPosition(Vector2 newPosition)
    {
        if (!IsLocal)
        {
            Entity.Position = newPosition;
        }
    }
}
