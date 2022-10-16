using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap pathTilemap;
    [SerializeField]
    private Tilemap borderTilemap;
    private PlayerMovement controls;

    private Vector2 direction;

    private float playedTime;
    private float speed = 1;
    private void Awake()
    {
        controls = new PlayerMovement();
        playedTime = 0.0f;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Main.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        if (!CanMove(direction)) return;
        Debug.Log("Move" + direction);
        Vector2 check = new Vector2(0, 0);
        if(direction ==  check) return;
        this.direction = (Vector3) direction;

    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = pathTilemap.WorldToCell((transform.position + (Vector3)direction));
       
        if (direction == this.direction* -1 || !pathTilemap.HasTile(gridPosition) || borderTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("LevelBorder"))
        {
            Debug.Log("Autsch");
            direction = direction * -1;
            //speed = Mathf.Max(0.5f, speed - 1);
        }
    }

    private void Update()
    {
        //playedTime += Time.deltaTime;
        //speed = Mathf.Min(speed + (1 / (playedTime * 10)), 3);
        transform.Translate((Vector3)(direction) * 1 * Time.deltaTime);
    }
}
