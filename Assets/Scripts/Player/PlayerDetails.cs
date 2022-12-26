using System;
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

    [SerializeField] private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = StartPos;
        //gameObject.GetComponent<SpriteRenderer>().flipX = !(transform.position.x > 0);

        spriteRenderer.color = Color;
    }

    private void Update()
    {
        spriteRenderer.color = Color.Lerp(Color.white, Color, (float)CurrentFields / (float)MaxFields);
    }
}
