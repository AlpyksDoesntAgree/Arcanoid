using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegManager : MonoBehaviour
{
    [System.Serializable]
    public class CreateNewUser
    {
        public string Name;
        public string Login;
        public string Password;

        public CreateNewUser(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }
    }

    [System.Serializable]
    public class RegistrationResponse
    {
        public bool status;
        public int id;
        public string name;
    }

    [SerializeField] private InputField loginField;
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField passField;
    [SerializeField] private InputField repeatPassField;

    private string ApiUrl = $"http://localhost:5039/api/UsersLogins/createNewUser"; 

    public void RegisterNewUser()
    {
        if (passField.text == repeatPassField.text && passField.text != "")
        {
            string name = nameField.text;
            string login = loginField.text;
            string password = passField.text;



            CreateNewUser newUser = new CreateNewUser(name, login, password);
            StartCoroutine(SendRegistrationRequest(newUser));
        }
    }

    private IEnumerator SendRegistrationRequest(CreateNewUser newUser)
    {
        string json = JsonUtility.ToJson(newUser);

        UnityWebRequest request = new UnityWebRequest(ApiUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User registered successfully!");
            string responseText = request.downloadHandler.text;
            RegistrationResponse response = JsonUtility.FromJson<RegistrationResponse>(responseText);

            PlayerPrefs.SetInt("id", response.id);
            PlayerPrefs.SetString("UserName", response.name);
            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.Save();

            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Registration failed: " + request.error);
        }
    }
}
