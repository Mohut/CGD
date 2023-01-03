using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private SpriteRenderer crown;
    [SerializeField] private SpriteRenderer speedBoostImage;
    [SerializeField] private FrontChecker frontChecker;
    [SerializeField] private PlayerDetails playerDetails;
    private Color color;
    public Rigidbody2D controller;
    private Vector3 destination;
    private Vector3 newdestination;
    private Vector3 olddestination;
    private Vector2 direction;
    [SerializeField] private float speedBoostThreshold;
    [SerializeField] private float speedBoost;
    public float speed;
    private float initialSpeed;
    public List<Item> items;

    [Header("Map")]
    public Tilemap pathTilemap;
    public Tilemap borderTilemap;
    private SpriteRenderer spriteRenderer;

    private bool used = false;
    private float passed = 0;

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
        initialSpeed = speed;

        GameManager.Instance.showPlayerAhead += ShowCrown;
    }

    private void OnDestroy()
    {
        GameManager.Instance.showPlayerAhead -= ShowCrown;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameStarted == false)
            return;
        
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
                if (Vector3.Distance(forward, Vector3.zero) < 0.001f)
                {
                    speed = initialSpeed;
                    return;
                }
                
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

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    private void ShowCrown(int playerIndex)
    {
        if (playerIndex == playerDetails.PlayerID)
        {
            crown.enabled = true;
        }
        else
        {
            crown.enabled = false;
        }
    }

    IEnumerator Co_ShowSpeedBoost()
    {
        speedBoostImage.enabled = true;
        yield return new WaitForSeconds(0.3f);
        speedBoostImage.enabled = false;
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        if (items.Count > 0)
        {
            items[0].TriggerEffect(gameObject);
            Logger.Instance.WriteToFile(LogId.ItemUsage, Time.time + " " + items[0].name);
            items.RemoveAt(0);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        if (GameManager.Instance.GameStarted == false)
        {
            GameManager.Instance.ShowPlayer(playerDetails.PlayerID);
            return;
        }

        Vector2 direction = context.ReadValue<Vector2>();
        Vector3 gridpos = CanMove(direction);
        // if gridpos is 0, the player cant move in this direction, because there is a wall
        if (Vector3.Distance(gridpos, Vector3.zero) < 0.001f || Vector2.Distance(direction, Vector2.zero) < 0.001f)
        {
            speed = initialSpeed;
            return;
        }
        
        this.direction = direction;
        newdestination = gridpos;

        if (Vector3.Distance(olddestination, newdestination) < Vector3.Distance(newdestination, destination))
        {
            transform.position = olddestination;
            destination = newdestination;
            transform.right = destination - transform.position;
        }
        
        // Apply Speedboost or reset
        if (Vector3.Distance(transform.position, olddestination) < speedBoostThreshold ||
            Vector3.Distance(transform.position, destination) < speedBoostThreshold)
        {
            speed += speedBoost;
            StartCoroutine(Co_ShowSpeedBoost());
        }
        else
        {
            speed = initialSpeed;
        }
        
        Logger.Instance.WriteToFile(LogId.SpeedPercentage, speed.ToString());
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    private Vector3 CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = pathTilemap.WorldToCell((transform.position + (Vector3)direction));

        if (direction == this.direction * -1 || !pathTilemap.HasTile(gridPosition) ||
            borderTilemap.HasTile(gridPosition) )
            return Vector3.zero;
        return gridPosition + new Vector3(0.5f, 0.5f, 0);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") == false)
            return;

        if (frontChecker.BehindPlayer == false || (col.gameObject.GetComponentInChildren<FrontChecker>().BehindPlayer && frontChecker.BehindPlayer))
        {
            Respawn();
            SoundManager.Instance.PlayPlayerHitSound();
            Logger.Instance.WriteToFile(LogId.PlayerHit, "Player" + gameObject.GetComponent<PlayerDetails>().PlayerID + " hit Player" + gameObject.GetComponent<PlayerDetails>().PlayerID);
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LevelPath"))
        {
            SetTileColour(color, Vector3Int.FloorToInt(transform.position));
        }
    }
    
    public void SetTileColour(Color colour, Vector3Int position)
    {
        if (playerDetails.CurrentFields <= 0) 
            return;
        
       
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Colour".
        pathTilemap.SetTileFlags(position, TileFlags.None);
        Color before = pathTilemap.GetColor(position);
        if(before != playerDetails.Color)
            playerDetails.CurrentFields--;
        // Set the colour.
        pathTilemap.SetColor(position, colour);
        
        onTileColored?.Invoke(playerDetails.PlayerID, position, before);
        
        Logger.Instance.WriteToFile(LogId.Heatmap, playerDetails.PlayerID + " : " + position);
    }

    private void Respawn()
    {
        transform.position = playerDetails.StartPos;
        destination = playerDetails.StartPos;
        olddestination = destination;
        newdestination = destination;
        direction = Vector2.zero;
    }
    
}
