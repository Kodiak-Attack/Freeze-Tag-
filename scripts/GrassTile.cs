using AO;

public partial class GrassTile : Component
{
    public Sprite_Renderer SpriteRenderer;
    public Vector2 GridPosition;
    public bool IsWalkable = true;
    
    public override void Awake()
    {
        // Create sprite renderer for the grass tile
        SpriteRenderer = Entity.AddComponent<Sprite_Renderer>();
        
        // Load and set the grass texture
        var grassTexture = Assets.GetAsset<Texture>("textures/grass.png");
        if (grassTexture != null)
        {
            SpriteRenderer.Texture = grassTexture;
            SpriteRenderer.Tint = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); // White tint to show original texture
            Log.Info($"Grass tile loaded texture successfully at {Entity.Position}");
        }
        else
        {
            // Try alternative path
            grassTexture = Assets.GetAsset<Texture>("grass.png");
            if (grassTexture != null)
            {
                SpriteRenderer.Texture = grassTexture;
                SpriteRenderer.Tint = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                Log.Info($"Grass tile loaded texture with alternative path at {Entity.Position}");
            }
            else
            {
                // Fallback to green color if texture not found
                SpriteRenderer.Tint = new Vector4(0.2f, 0.8f, 0.2f, 1.0f);
                Log.Info($"Grass tile using fallback color at {Entity.Position} - texture not found");
            }
        }
        
        // Set the size of the grass tile
        Entity.LocalScale = new Vector2(1.0f, 1.0f);
        
        Log.Info($"Grass tile created at {Entity.Position} with scale {Entity.LocalScale}");
    }
    
    public void SetGridPosition(int x, int y)
    {
        GridPosition = new Vector2(x, y);
        Entity.Position = new Vector2(x, y);
    }
    
    public void SetWalkable(bool walkable)
    {
        IsWalkable = walkable;
        // Change color based on walkability
        if (walkable)
        {
            SpriteRenderer.Tint = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); // White tint for walkable grass
        }
        else
        {
            SpriteRenderer.Tint = new Vector4(0.5f, 0.3f, 0.1f, 1.0f); // Brown tint for non-walkable
        }
    }
} 