using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    
    
    [Header("Player")]
    public Rigidbody2D controller;

   
    private Vector2 direction;

    public float speed;
    
    [Header("Map")]
    
    
    public Tilemap pathTilemap;
    public Tilemap borderTilemap;

    private SpriteRenderer spriteRenderer;
 

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        //controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Vector2 check = new Vector2(0, 0);
        
        if (!CanMove(direction) || direction == check ) return;
        
        this.direction = (Vector3) direction;
        
        spriteRenderer.flipX = !(direction.x < 0);

        switch(direction.y)
        {
            case -1:
                transform.eulerAngles = new Vector3(0,0,-90);
                break;
            case 1:
                transform.eulerAngles = new Vector3(0,0,90);
                break;
            default:
                transform.eulerAngles = new Vector3(0,0,0);
                break;
        }
       
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = pathTilemap.WorldToCell((transform.position + (Vector3)direction));
       
        if (direction == this.direction* -1 || !pathTilemap.HasTile(gridPosition) || borderTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("LevelPaths"))
        {
            Color playerColor = gameObject.GetComponent<PlayerDetails>().Color;
            SetTileColour(playerColor, Vector3Int.FloorToInt(transform.position), pathTilemap);
        }
    }
    
    private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
    {
        Debug.Log(position);
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Colour".
        tilemap.SetTileFlags(position, TileFlags.None);
 
        // Set the colour.
        tilemap.SetColor(position, colour);
    }

    private void FixedUpdate()
    {
        controller.MovePosition(controller.position + direction * speed * Time.fixedDeltaTime);

    }
}
