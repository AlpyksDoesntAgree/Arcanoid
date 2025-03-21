using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
    private bool _coinsAdded = false;
    private int _userID;
    private int _userCoins;

    private void Awake()
    {
        _userID = PlayerPrefs.GetInt("id");
        _userCoins = PlayerPrefs.GetInt("Coins");
    }
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

            if (!_coinsAdded)
            {
                StartCoroutine(AddCoins(_userID, 50));
                _coinsAdded = true;
            }
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

    IEnumerator AddCoins(int userId, int amount)
    {
        string url = $"http://localhost:5039/api/UsersLogins/{userId}/add-coins";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(amount.ToString());

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("succsess!");
                PlayerPrefs.SetInt("Coins", _userCoins + amount);
            }
            else
            {
                Debug.LogError($"error: {www.error}");
            }
        }
    }
}
