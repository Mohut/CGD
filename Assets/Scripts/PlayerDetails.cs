using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int PlayerID;
    public Vector3 StartPos;

    public Color Color;

    
    public int MaxFields;

    public int CurrentFields;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = StartPos;
        //gameObject.GetComponent<SpriteRenderer>().flipX = !(transform.position.x > 0);

       
        gameObject.GetComponent<SpriteRenderer>().color = Color;
    }
}
