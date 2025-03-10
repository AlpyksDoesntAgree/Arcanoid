using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    int RandomX;
    private BonusSpawner _bonusSpawner;
    void Start()
    {
        _bonusSpawner = GameObject.Find("Event").GetComponent<BonusSpawner>();
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RandomX = Random.Range(-5,5);
        collision.rigidbody.velocity = new Vector2(RandomX, -5);
        _bonusSpawner.SpawnBonus(gameObject.transform.position);
        Destroy(gameObject);
    }
}
