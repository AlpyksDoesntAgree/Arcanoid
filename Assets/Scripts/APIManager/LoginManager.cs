using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Сериальчики
[System.Serializable]
public class LoginRequest
{
    public string Login;
    public string Password;
}

[System.Serializable]
public class LoginResponse
{
    public bool status;
    public UserData data;
}

[System.Serializable]
public class UserData
{
    public int id;
    public string name;
    public int coins;
}

//Логика
public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField loginInput;
    [SerializeField] private InputField passwordInput;

    private string apiURL = "http://localhost:5039/api/UsersLogins";

    public void AuthButton()
    {
        string login = loginInput.text;
        string password = passwordInput.text;
        StartCoroutine(SendLoginReq(login, password));
    }

    private IEnumerator SendLoginReq(string login, string password)
    {
        LoginRequest request = new LoginRequest { Login = login, Password = password };
        string jsonData = JsonUtility.ToJson(request);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        using (UnityWebRequest www = new UnityWebRequest(apiURL + "/getUser", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

                if (response.status)
                {
                    Debug.Log($"Вход успешен! ID: {response.data.id} Имя: {response.data.name}, Монеты: {response.data.coins}");

                    PlayerPrefs.SetInt("id", response.data.id);
                    PlayerPrefs.SetString("UserName", response.data.name);
                    PlayerPrefs.SetInt("Coins", response.data.coins);
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    Debug.Log("Ошибка: " + jsonResponse);
                }
            }
            else
            {
                Debug.LogError($"Ошибка запроса: {www.responseCode}");
                Debug.LogError($"Ответ сервера: {www.downloadHandler.text}");
            }
        }
    }
}
