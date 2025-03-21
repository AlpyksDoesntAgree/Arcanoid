using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BallMove : MonoBehaviour
{
    private GameObject _player;

    public bool ballIsActive = true;
    private Vector3 ballPosition;
    private Rigidbody2D rb;
    private Vector3 StartJumpDirection;
    [SerializeField] private Sprite[] skins;
    private SpriteRenderer _playerSkin;
    private int curSkin;

    private bool IsSpeeded = false;
    void Start()
    {
        curSkin = PlayerPrefs.GetInt("SelectedSkin");
        Debug.Log(curSkin);
        _player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        _playerSkin = GetComponent<SpriteRenderer>();
        _playerSkin.sprite = skins[curSkin];

        int ballLayer = LayerMask.NameToLayer("Balls");
        Physics2D.IgnoreLayerCollision(ballLayer, ballLayer, true);
    }

    void Update()
    {
        StartJumpDirection = Input.mousePosition;
        StartJumpDirection.z = 0.0f;
        StartJumpDirection = Camera.main.ScreenToWorldPoint(StartJumpDirection);
        StartJumpDirection = StartJumpDirection - transform.position;

        if (Input.GetButtonDown("Jump"))
        {
            if (!ballIsActive)
            {
                rb.velocity = new Vector2(StartJumpDirection.x, 5);   
                ballIsActive = !ballIsActive;
            }
        }

        if (!ballIsActive && _player != null)
        {
            ballPosition.x = _player.transform.position.x;
            ballPosition.y = _player.transform.position.y+0.3f;

            transform.position = ballPosition;
        }
    }

    public IEnumerator BallSpeed(string dif)
    {
        if (!IsSpeeded)
        {
            if (dif == "plus")
            {
                rb.velocity *= 2f;
                IsSpeeded = true;

                yield return new WaitForSeconds(5);

                ReturnSpeed();
            }
            else
            {
                rb.velocity *= 0.5f;
                IsSpeeded = true;

                yield return new WaitForSeconds(5);

                ReturnSpeed();
            }
        }
    }

    public void StartSpeed(string dif)
    {
        StartCoroutine(BallSpeed(dif));
    }

    public void ReturnSpeed()
    {
        rb.velocity = new Vector2(2, 6);
        IsSpeeded = false;
    }

    public void Triple()
    {
        GameObject[] _balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject _ball in _balls)
        {
            Vector3 ballPosition = _ball.transform.position;
            BallMove originalBall = _ball.GetComponent<BallMove>();

            GameObject newBall1 = Instantiate(_ball, new Vector3(ballPosition.x + 0.275f, ballPosition.y - 0.75f, 1), Quaternion.identity);
            GameObject newBall2 = Instantiate(_ball, new Vector3(ballPosition.x - 0.275f, ballPosition.y - 0.75f, 1), Quaternion.identity);

            SetupNewBall(newBall1, new Vector2(newBall1.transform.position.x + 3, 5));
            SetupNewBall(newBall2, new Vector2(newBall2.transform.position.x - 3, 5));
        }
    }

    private void SetupNewBall(GameObject ball, Vector2 initialVelocity)
    {
        BallMove ballMove = ball.GetComponent<BallMove>();
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        if (ballMove != null && rb != null)
        {
            ballMove.ballIsActive = true;
            rb.velocity = initialVelocity;
        }
    }
}