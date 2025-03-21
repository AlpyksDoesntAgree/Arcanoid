using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Text _coinsText;
    private int _usersCoins;
    private int user_id;
    [SerializeField] private Button[] _buyButtons;
    [SerializeField] private Button[] _equipButtons;

    private int[] skinIds = { 1, 2, 3 };

    private void Awake()
    {
        user_id = PlayerPrefs.GetInt("id");
        _usersCoins = PlayerPrefs.GetInt("Coins");
    }

    void Start()
    {
        _coinsText.text = _usersCoins.ToString();
        CheckAllSkins();
    }

    public void SelectSkin(int skinId)
    {
        PlayerPrefs.SetInt("SelectedSkin", skinId);
        PlayerPrefs.Save();
        int debugSkin = PlayerPrefs.GetInt("SelectedSkin");
        Debug.Log(debugSkin);
    }

    private void CheckAllSkins()
    {
        for (int i = 0; i < skinIds.Length; i++)
        {
            int skinId = skinIds[i];
            bool isOwned = PlayerPrefs.GetInt($"id_{user_id}_Skin_{skinId}", 0) == 1;

            _buyButtons[i].gameObject.SetActive(!isOwned);
            _equipButtons[i].gameObject.SetActive(isOwned);
        }
    }

    public void Buying(int skinId)
    {
        int price = 0;
        switch(skinId)
        {
            case 1: price = 50; break;
            case 2: price = 60; break;
            case 3: price = 80; break;
        }

        if (_usersCoins >= price)
        {
            StartCoroutine(SpendCoins(user_id, price));
            _usersCoins -= price;
            PlayerPrefs.SetInt("Coins", _usersCoins);
            _coinsText.text = _usersCoins.ToString();
            StartCoroutine(BuySkin(user_id, skinId));
        }
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

                PlayerPrefs.SetInt($"id_{user_id}_Skin_{skinId}", 1);
                PlayerPrefs.Save();

                CheckAllSkins();
            }
            else
            {
                Debug.LogError($"Ошибка: {www.error}");
            }
        }
    }

    [System.Serializable]
    public class BuySkinResponse
    {
        public bool status;
    }
}
