    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HealthControl : MonoBehaviour
{
    private int _health = 3;
    private GameObject[] _balls;
    [SerializeField] private Transform _ballPrefab;
    private float _bally = -3.7f; //Затычка в Vector2
    [SerializeField] private Rigidbody2D _player;
    [SerializeField] private Text _HPText;
    [SerializeField] private Animator LoseAnimator;
    void Start()
    {
        _ballPrefab.GetComponent<BallMove>().ballIsActive = false;
        LoseAnimator.enabled = false;
    }

    void Update()
    {
        if (_health == 0)
        {
            _player.GetComponent<PlayerMove>().isEnabled = false;
            LoseAnimator.enabled = true;
        }
        _balls = GameObject.FindGameObjectsWithTag("Ball");

        if (_balls.Length <= 0)
        {
            _health -= 1;
            _HPText.text = _health.ToString();
            if (_health > 0)
            {
                Instantiate(_ballPrefab, new Vector2(-3, _bally), Quaternion.identity);
                _player.transform.position = new Vector2(-3, -4);
            }
            else { _HPText.text = 0.ToString();}
        }
    }
}
