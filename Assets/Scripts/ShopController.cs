using System.Collections;
using System.Collections.Generic;

using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Text _coinsText;
    private int _usersCoins;
    private int user_id;
    void Start()
    {
        user_id = PlayerPrefs.GetInt("id");
        _usersCoins = PlayerPrefs.GetInt("Coins");
        _coinsText.text = _usersCoins.ToString();
    }

    void Update()
    {

    }

    public void Buying(int price)
    {
        if (_usersCoins >= price)
        {
            StartCoroutine(SpendCoins(user_id, price));
            _usersCoins -= price;
            PlayerPrefs.SetInt("Coins", _usersCoins);
            _coinsText.text = _usersCoins.ToString();
        }
    }

    public void MoneyAdd(int price)
    {
        StartCoroutine(AddCoins(user_id, price));
        _usersCoins -= price;
        PlayerPrefs.SetInt("Coins", _usersCoins);
        _coinsText.text = _usersCoins.ToString();
    }

    IEnumerator SpendCoins(int userId, int amount)
    {
        string url = $"http://localhost:5039/api/UsersLogins/{userId}/spend-coins";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(amount.ToString());

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Монеты добавлены!");
            }
            else
            {
                Debug.LogError($"Ошибка: {www.error}");
            }
        }
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
                Debug.Log("Монеты добавлены!");
            }
            else
            {
                Debug.LogError($"Ошибка: {www.error}");
            }
        }
    }
}

