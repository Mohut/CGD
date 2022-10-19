using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap pathTilemap;
    [SerializeField]
    private Tilemap borderTilemap;
    public Rigidbody2D controller;

    private Vector2 direction;

    public float speed;

    private void Awake()
    {
        //controller = gameObject.GetComponent<Rigidbody2D>();
   
    }
    

    // Start is called before the first frame update
    void Start()
    {
        //controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("hihihihihi");
        Vector2 direction = context.ReadValue<Vector2>();
        Vector2 check = new Vector2(0, 0);
        if (!CanMove(direction) || direction == check ) return;
        //Debug.Log("Move" + direction);
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
            //Debug.Log("Autsch");
            //direction = direction * -1;
            //speed = Mathf.Max(0.5f, speed - 1);
        }
    }

    private void FixedUpdate()
    {
        //playedTime += Time.deltaTime;
        //speed = Mathf.Min(speed + (1 / (playedTime * 10)), 3);
        //transform.Translate((Vector3)(direction) * speed * Time.deltaTime);
       
        
        //Debug.Log(pos);
        controller.MovePosition(controller.position + direction * speed * Time.fixedDeltaTime);
        //controller.velocity = (Vector2) direction * speed * Time.deltaTime;
        //Debug.Log("Move" + controller.velocity);
        //controller.AddForce(direction*speed*Time.fixedDeltaTime, ForceMode2D.Force);

    }
}
