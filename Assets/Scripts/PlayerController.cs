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

    private void Start()
    {
        color = gameObject.GetComponent<PlayerDetails>().Color;
        destination = transform.position;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Vector3 check = new Vector3(0,0, 0);
        Vector3 gridpos = CanMove(direction);
        if (gridpos == check || direction == Vector2.zero ) return;
        this.direction = direction;
        newdestination = gridpos;

        switch(direction.y)
        {
            case -1:
                transform.eulerAngles = new Vector3(0,0,-90);
                break;
            case 1:
                transform.eulerAngles = new Vector3(0,0,90);
                break;
            default:
                if (direction.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 180);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0,0,0);
                }
                break;
        }
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
 
        // Set the colour.
        tilemap.SetColor(position, colour);
    }

    private void Respawn()
    {
        transform.position = playerDetails.StartPos;
    }

    private void FixedUpdate()
    {
        Debug.Log(destination);
        if (Vector3.Distance(transform.position, destination) < Mathf.Epsilon)
        {
            Debug.Log(newdestination);
            if(newdestination != destination)
                destination = newdestination;
            else
            {
                destination = destination + (Vector3)direction;
                newdestination = destination;
            }
        }
        else
        {
            //controller.MovePosition(controller.position + direction * speed * Time.fixedDeltaTime);
            transform.position = Vector3.MoveTowards(transform.position, destination,
                speed * Time.deltaTime);
            
        }
       
    }
}
