using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    private PlayerMove _player;
    private BallMove _ball;
    private Transform _modifier;
    void Start()
    {
        _ball = GameObject.Find("Ball").GetComponent<BallMove>();
        _player = GameObject.Find("Player").GetComponent<PlayerMove>();
        _modifier = GetComponent<Transform>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            switch (_modifier.tag)
            {
                case "Triple":
                    _ball.Triple();
                    Destroy(_modifier.gameObject);
                    break;
                case "SizeUp":
                    _player.StartSize("plus");
                    Destroy(_modifier.gameObject);
                    break;
                case "SizeDown":
                    _player.StartSize("minus");
                    Destroy(_modifier.gameObject);
                    break;
                case "SpeedUp":
                    _ball.StartSpeed("plus");
                    Destroy(_modifier.gameObject);
                    break;
                case "SpeedDown":
                    _ball.StartSpeed("minus");
                    Destroy(_modifier.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}