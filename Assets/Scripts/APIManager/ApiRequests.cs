using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : MonoBehaviour
{
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiRequests
{
    private static string apiUrl = "http://localhost:5039/api/";

    public static void GetUsers(Action<string> callback)
    {
        GameManager.Instance.StartCoroutine(GetUsersCoroutine(callback));
    }

    public static IEnumerator GetUsersCoroutine(Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "UsersLogins/getAllUsers"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback.Invoke(request.downloadHandler.text);
            }
            else
                Debug.Log(request.error);
        }
    }

    /*private static IEnumerator GetUsersCoroutine(Action<UserResponse> callback, User users)
    {
        string jsonData = JsonUtility.ToJson(users);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "UsersLogins/getAllUsers", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                UserResponse userResponse = JsonUtility.FromJson<UserResponse>(jsonResponse);
                callback?.Invoke(userResponse);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }*/
}

}
