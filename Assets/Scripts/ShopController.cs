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

    public void Buying(int price, int skinId)
    {
        if (_usersCoins >= price)
        {
            StartCoroutine(SpendCoins(user_id, price));
            _usersCoins -= price;
            PlayerPrefs.SetInt("Coins", _usersCoins);
            _coinsText.text = _usersCoins.ToString();
            StartCoroutine(BuySkin(user_id, skinId));
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
                Debug.Log("������ ���������!");
            }
            else
            {
                Debug.LogError($"������: {www.error}");
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
                Debug.Log("������ ���������!");
            }
            else
            {
                Debug.LogError($"������: {www.error}");
            }
        }
    }

    IEnumerator BuySkin(int userId, int skinId)
    {
        string url = $"http://localhost:5039/api/UsersLogins/{userId}/buy-skin/{skinId}";
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, ""))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                BuySkinResponse response = JsonUtility.FromJson<BuySkinResponse>(jsonResponse);

                Debug.Log(response.message);
            }
            else
            {
                Debug.LogError($"������: {www.error}");
            }
        }
    }

    [System.Serializable]
    public class BuySkinResponse
    {
        public bool status;
        public string message;
    }
}

