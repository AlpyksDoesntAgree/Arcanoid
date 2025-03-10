using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHPBlock : MonoBehaviour
{
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
        _bonusSpawner.SpawnBonus(gameObject.transform.position);
        Destroy(gameObject);
    }
}
