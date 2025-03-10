using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [SerializeField]private GameObject[] _bonuses;
    private int _random;
    private int _randomBonus;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnBonus(Vector3 pos)
    {
        _random = Random.Range(1,5);
        if(_random == 1)
        {
            _randomBonus = Random.Range(1, 5);
            Instantiate(_bonuses[_randomBonus], pos, Quaternion.identity);
        }
    }
}
