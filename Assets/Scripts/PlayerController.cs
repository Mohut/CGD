using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private FrontChecker frontChecker;

    [SerializeField] private PlayerDetails playerDetails;
    private Color color;
    public Rigidbody2D controller;
    private Vector3 destination;
    private Vector3 newdestination;
    private Vector3 olddestination;
    private Vector2 direction;
    public float speed;

    [Header("Map")]
    public Tilemap pathTilemap;
    public Tilemap borderTilemap;
    private SpriteRenderer spriteRenderer;

    public Action<int, Vector3, Color> onTileColored;
    
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        color = playerDetails.Color;
        destination = playerDetails.StartPos;
        olddestination = destination;
        newdestination = destination;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Vector3 gridpos = CanMove(direction);
        if (Vector3.Distance(gridpos, Vector3.zero) < 0.001f || Vector2.Distance(direction, Vector2.zero) < 0.001f) return;
        this.direction = direction;
        newdestination = gridpos;

        if (Vector3.Distance(olddestination, newdestination) < Vector3.Distance(newdestination, destination))
        {
            olddestination = destination;
            destination = newdestination;
        }


        // if (direction.x < 0)
        //     transform.eulerAngles = new Vector3(0, 180, transform.eulerAngles.z);
        // else
        // {
        //     transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        // }
        // switch(direction.y)
        // {
        //     case -1:
        //         transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,-90);
        //         break;
        //     case 1:
        //         transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,90);
        //         break;
        //     default:
        //         transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
        //       
        //         break;
        // }
    }

    private Vector3 CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = pathTilemap.WorldToCell((transform.position + (Vector3)direction));

        if (direction == this.direction * -1 || !pathTilemap.HasTile(gridPosition) ||
            borderTilemap.HasTile(gridPosition))
            return Vector3.zero;
        return gridPosition + new Vector3(0.5f, 0.5f, 0);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") == false)
            return;

        if (frontChecker.BehindPlayer == false || (col.gameObject.GetComponentInChildren<FrontChecker>().BehindPlayer && frontChecker.BehindPlayer))
            Respawn();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("LevelPaths"))
        {
            SetTileColour(color, Vector3Int.FloorToInt(transform.position), pathTilemap);
        }
    }
    
    private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
    {
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Colour".
        tilemap.SetTileFlags(position, TileFlags.None);
        Color before = tilemap.GetColor(position);
        // Set the colour.
        tilemap.SetColor(position, colour);
        
        onTileColored?.Invoke(playerDetails.PlayerID, position, before);
    }

    private void Respawn()
    {
        transform.position = playerDetails.StartPos;
        destination = playerDetails.StartPos;
        olddestination = destination;
        newdestination = destination;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, destination) < 0.0001f)
        {
            if (newdestination != destination)
            {
                
                olddestination = destination;
                destination = newdestination;
                
            }
            else
            {
                Vector3 forward = CanMove(direction);
                if (Vector3.Distance(forward, Vector3.zero) < 0.001f) return;
                olddestination = destination;
                destination += (Vector3)direction;
                newdestination = destination;
            }
            
            transform.right = destination - transform.position;
        }
        else
        {
            //controller.MovePosition(controller.position + direction * speed * Time.fixedDeltaTime);
            transform.position = Vector3.MoveTowards(transform.position, destination,
                speed * Time.deltaTime);
            
        }
       
    }
}
