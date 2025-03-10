using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private Transform block;
    private float maxX;
    private float minX;
    private float _speed = 2f;
    private bool movingRight = true;

    void Start()
    {
        block = GetComponent<Transform>();
        maxX = block.position.x + 3;
        minX = block.position.x - 3;
    }

    void Update()
    {
        if (movingRight)
        {
            block.position = Vector2.MoveTowards(block.position, new Vector3(maxX, block.position.y, block.position.z), _speed * Time.deltaTime);
            if (block.position.x >= maxX)
                movingRight = false;
        }
        else
        {
            block.position = Vector2.MoveTowards(block.position, new Vector3(minX, block.position.y, block.position.z), _speed * Time.deltaTime);
            if (block.position.x <= minX)
                movingRight = true;
        }
    }
}
