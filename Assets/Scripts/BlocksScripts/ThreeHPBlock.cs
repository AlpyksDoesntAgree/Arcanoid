using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeHPBlock : MonoBehaviour
{
    private SpriteRenderer _block;
    private int _health = 3;
    private BonusSpawner _bonusSpawner;
    [SerializeField] private Sprite _SecondHitPic;
    [SerializeField] private Sprite _lastHitPic;
    void Start()
    {
        _block = GetComponent<SpriteRenderer>();
        _bonusSpawner = GameObject.Find("Event").GetComponent<BonusSpawner>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _health -= 1;
        if (_health == 2)
            _block.sprite = _SecondHitPic;
        if (_health == 1)
            _block.sprite = _lastHitPic;
        if (_health == 0)
        {
            _bonusSpawner.SpawnBonus(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
