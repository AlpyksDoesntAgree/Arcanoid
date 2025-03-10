using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum movement
    {
        keyboard,
        mouse
    }

    public int savedMovement;
    private Transform _player;
    private float _speed = 10.0f;
    private bool IsSized = false;
    [HideInInspector] public bool isEnabled = true;
    [SerializeField] private movement currentMovement;
    private GameObject[] _blocks;
    [SerializeField] private Animator _winAnim;

    [SerializeField] private Transform _leftPoint;
    [SerializeField] private Transform _rightPoint;


    void Start()
    {
        _winAnim.enabled = false;
        _player = GetComponent<Transform>();
        savedMovement = PlayerPrefs.GetInt("Movement", 1);
    }

    void Update()
    {
        currentMovement = (movement)savedMovement;
        if (currentMovement == movement.keyboard)
        {
            if (isEnabled)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    _player.position = Vector3.MoveTowards(_player.position, _leftPoint.position, _speed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    _player.position = Vector3.MoveTowards(_player.position, _rightPoint.position, _speed * Time.deltaTime);
                }
            }
        }
        if (currentMovement == movement.mouse)
        {
            if (isEnabled)
            {
                Vector3 mouseScreenPos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10));

                if (worldPos.x <= 2.28 && worldPos.x >= -7.5)
                {
                    _player.position = new Vector2(worldPos.x, -4);
                }
            }
        }

        _blocks = GameObject.FindGameObjectsWithTag("Block");
        if( _blocks.Length<=0)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            Rigidbody2D rb;
            foreach (GameObject ball in balls)
            {
                rb = ball.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.zero;
            }
            _winAnim.enabled = true;
        }
    }

    public IEnumerator PlatformSize(string dif)
    {
        if (!IsSized)
        {
            if (dif == "plus")
            {
                _player.gameObject.transform.localScale = new Vector3(1, 0.85f, 1);
                IsSized = true;
                
                yield return new WaitForSeconds(5);

                ReturnSize();
            }
            else
            {
                _player.gameObject.transform.localScale = new Vector3(0.5f, 0.85f, 1);
                IsSized = true;

                yield return new WaitForSecondsRealtime(5);

                ReturnSize();
            }
        }
    }

    public void StartSize(string dif)
    {
        StartCoroutine(PlatformSize(dif));
    }

    public void ReturnSize()
    {
        _player.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 1);
        IsSized = false;
    }
}
