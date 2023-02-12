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
    [SerializeField] private SpriteRenderer itemSpriteRenderer;
    [SerializeField] private SpriteRenderer crown;
    [SerializeField] private SpriteRenderer speedBoostImage;
    [SerializeField] private FrontChecker frontChecker;
    [SerializeField] private PlayerDetails playerDetails;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float invincibilityTime;
    private Color color;
    public Rigidbody2D controller;
    private Vector3 destination;
    private Vector3 newdestination;
    private Vector3 olddestination;
    private Vector2 direction;
    private bool isInvincible = false;

    [SerializeField] private float speedBoostThreshold;
    [SerializeField] private float speedBoost;
    [SerializeField] private float maxSpeed;
    private LineRenderer lineRenderer;
    public float speed;
    private float initialSpeed;
    public List<Item> items;

    [Header("Map")]
    public Tilemap pathTilemap;
    public Tilemap borderTilemap;
    private SpriteRenderer spriteRenderer;

    private bool used = false;
    private float passed = 0;

    private bool onWall = false;
    private bool hasGun = false;

    private Vector3 wallPos;
    [SerializeField] private LayerMask wallMask;

    public Action<int, Vector3, Color> onTileColored;
    private int overOwnFields = 0;
    private int overEnemyFields = 0;
    private int overNeutralFields = 0;


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
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        GameManager.Instance.showPlayerAhead += ShowCrown;
        GameManager.Instance.gameOverEvent += (int index) =>
        {
            PlayerGameOver();
        };
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
                    onWall = true;
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

        if (hasGun)
        {
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, wallPos);
            
        }
    }

    public void AddItem(Item item)
    {
        if (items.Count > 0 )
            items[0] = item;
        else
        {
            items.Add(item);
        }
        itemSpriteRenderer.sprite = item.sprite;
        itemSpriteRenderer.enabled = true;
        if (item.name == "Gun")
        {
            hasGun = true;
            lineRenderer.enabled = true;
        }
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
            audioSource.clip = items[0].sound;
            audioSource.Play();
            items[0].TriggerEffect(gameObject);
            Logger.Instance.WriteToFile(LogId.ItemUsage,  items[0].name);
            items.RemoveAt(0);
            itemSpriteRenderer.enabled = false;
            hasGun = false;
            lineRenderer.enabled = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
        Vector2 oldDirection = this.direction;
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
        
            
        RaycastHit2D wallHit = Physics2D.Raycast((Vector2) transform.position, direction, 40f, wallMask);
        wallPos = wallHit.point;
        
        Vector3 gridpos = CanMove(direction);
        // if gridpos is 0, the player cant move in this direction, because there is a wall
        if (Vector3.Distance(gridpos, Vector3.zero) < 0.001f || Vector2.Distance(direction, Vector2.zero) < 0.001f)
        {
            onWall = true;
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
            if (onWall)
            {
                onWall = false;
                
            }
            else
            {
                if (Vector2.Distance(this.direction, oldDirection) > 0.01f)
                {
                    if(speed >= maxSpeed) return;
                    speed += speedBoost;
                    
                    StartCoroutine(Co_ShowSpeedBoost());
                }
            }
                
        }
        else
        {
            speed = initialSpeed;
        }

        
        
        int playernumber = GameManager.Instance.SpawnManager.PlayerColorDictionary[color];
        Logger.Instance.WriteToFile(LogId.SpeedPercentage, "Player " + playernumber + " Speed To: " + speed);
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
        if (!col.gameObject.CompareTag("Player"))
            return;
        if (isInvincible)
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
        if (before != playerDetails.Color)
        {
            playerDetails.CurrentFields--;
            if (before == Color.white)
            {
                overNeutralFields++;
            }
            else
            {
                overEnemyFields++;
            }
        }
        else
        {
            overOwnFields++;
        }
        // Set the colour.
        pathTilemap.SetColor(position, colour);

        onTileColored?.Invoke(playerDetails.PlayerID, position, before);
        GameManager.Instance.RegisterField(position);
    }

    private IEnumerator Invincibility(float time)
    {
        yield return new WaitForSeconds(0.1f);
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        isInvincible = true;
        collider.isTrigger = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
        collider.isTrigger = false;
    }

    private void PlayerGameOver()
    {
        Logger.Instance.WriteToFile(LogId.FieldTypes, "Player " + playerDetails.PlayerID + " OverNeutralFields: " + overNeutralFields);
        Logger.Instance.WriteToFile(LogId.FieldTypes, "Player " + playerDetails.PlayerID + " OverEnemyFields: " + overEnemyFields);
        Logger.Instance.WriteToFile(LogId.FieldTypes, "Player " + playerDetails.PlayerID + " OverOwnFields: " + overOwnFields);
    }

    public void Respawn()
    {
        transform.position = playerDetails.StartPos;
        destination = playerDetails.StartPos;
        olddestination = destination;
        newdestination = destination;
        direction = Vector2.zero;
        StartCoroutine(Invincibility(invincibilityTime));
    }
    
}
