using AO;

public partial class GrassMapManager : Component
{
    public int MapWidth = 10;
    public int MapHeight = 10;
    public float TileSize = 1.0f;
    
    private GrassTile[,] grassTiles;
    private ulong rngSeed;
    
    public override void Awake()
    {
        // Initialize RNG seed
        rngSeed = RNG.Seed(Entity.NetworkId);
        
        // Initialize the grass tiles array
        grassTiles = new GrassTile[MapWidth, MapHeight];
        
        // Generate the grass map
        GenerateGrassMap();
    }
    
    public void GenerateGrassMap()
    {
        // Calculate the starting position to center the map
        float startX = -(MapWidth * TileSize) / 2.0f;
        float startY = -(MapHeight * TileSize) / 2.0f;
        
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                // Create grass tile entity
                var grassEntity = Entity.Create();
                
                // Add grass tile component
                var grassTile = grassEntity.AddComponent<GrassTile>();
                
                // Set position
                float worldX = startX + (x * TileSize);
                float worldY = startY + (y * TileSize);
                grassEntity.Position = new Vector2(worldX, worldY);
                
                // Set grid position
                grassTile.SetGridPosition(x, y);
                
                // Randomly make some tiles non-walkable (10% chance)
                bool isWalkable = RNG.RangeFloat(ref rngSeed, 0.0f, 1.0f) > 0.1f;
                grassTile.SetWalkable(isWalkable);
                
                // Store reference
                grassTiles[x, y] = grassTile;
                
                // Spawn the entity on the network (server only)
                if (Network.IsServer)
                {
                    Network.Spawn(grassEntity);
                }
            }
        }
        
        Log.Info($"Generated grass map with {MapWidth}x{MapHeight} tiles");
    }
    
    public GrassTile GetTileAt(int x, int y)
    {
        if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
        {
            return grassTiles[x, y];
        }
        return null;
    }
    
    public GrassTile GetTileAtWorldPosition(Vector2 worldPos)
    {
        // Convert world position to grid coordinates
        float startX = -(MapWidth * TileSize) / 2.0f;
        float startY = -(MapHeight * TileSize) / 2.0f;
        
        int gridX = (int)((worldPos.X - startX) / TileSize);
        int gridY = (int)((worldPos.Y - startY) / TileSize);
        
        return GetTileAt(gridX, gridY);
    }
    
    public bool IsPositionWalkable(Vector2 worldPos)
    {
        var tile = GetTileAtWorldPosition(worldPos);
        return tile != null && tile.IsWalkable;
    }
} 