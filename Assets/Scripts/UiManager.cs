using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UIManager : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button registerButton;
    public Button loginButton;
    public Button uploadButton;
    public Button downloadButton;
    public Text feedbackText;
    private const string baseURL = "http://localhost:3000/api/";

    private void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        uploadButton.onClick.AddListener(OnUploadButtonClicked);
        downloadButton.onClick.AddListener(OnDownloadButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            StartCoroutine(RegisterUser(username, password));
        }
        else
        {
            feedbackText.text = "Username and password cannot be empty.";
        }
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            StartCoroutine(LoginUser(username, password));
        }
        else
        {
            feedbackText.text = "Username and password cannot be empty.";
        }
    }

    private void OnUploadButtonClicked()
    {
        feedbackText.text = "Uploading save data...";
        StartCoroutine(UploadSaveData());
    }

    private void OnDownloadButtonClicked()
    {
        feedbackText.text = "Downloading save data...";
        StartCoroutine(DownloadSaveData());
    }

    private IEnumerator RegisterUser(string username, string password)
    {
        string registerUrl = baseURL + "auth/register";

        RegistrationData registrationData = new RegistrationData(username, password);
        string jsonPayload = JsonUtility.ToJson(registrationData); //only works with classes


        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Registration successful!";

            string responseText = request.downloadHandler.text;

            // Assuming the response text is a JSON object containing the token
            AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(responseText);
            string token = authResponse.token;

            PlayerPrefs.SetString("auth_token", token);
            PlayerPrefs.Save();
        }
        else
        {
            feedbackText.text = "Error: " + request.error;
            Debug.LogError(request.downloadHandler.text); // Log server response
        }
    }

    private IEnumerator LoginUser(string username, string password)
    {
        string loginUrl = baseURL + "auth/login";

        RegistrationData loginData = new RegistrationData(username, password);
        string jsonPayload = JsonUtility.ToJson(loginData);

        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Login successful!";
            string responseText = request.downloadHandler.text;

            // Assuming the response text is a JSON object containing the token
            AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(responseText);
            string token = authResponse.token;

            PlayerPrefs.SetString("auth_token", token);
            PlayerPrefs.Save();
        }
        else
        {
            feedbackText.text = "Error: " + request.error;
            Debug.LogError(request.downloadHandler.text);
        }
    }

    [System.Serializable]
    public class AuthResponse
    {
        public string token;
    }

    private IEnumerator UploadSaveData()
    {
        string[] filePaths = {
        Application.persistentDataPath + "/playerlvl1.json",
        Application.persistentDataPath + "/playerlvl2.json",
        Application.persistentDataPath + "/playerlvl3.json"
    };

        string uploadUrl = baseURL + "gameData/save";
        string token = PlayerPrefs.GetString("auth_token");

        if (string.IsNullOrEmpty(token))
        {
            feedbackText.text = "No token found. Please log in.";
            yield break;
        }

        foreach (string filePath in filePaths)
        {
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                int level = GetLevelFromFilePath(filePath);

                //Debug.Log("Uploading data for level: " + level);


                string formattedJson = "{\n     \"level\": " + level + ",\n"
                    + "     \"gameData\": "
                    + jsonData + "}";

                //Debug.Log(formattedJson);

                UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(formattedJson);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("x-auth-token", token);

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    feedbackText.text = "Error uploading save data for level " + level + ": " + request.error;
                    Debug.LogError(request.downloadHandler.text);
                    yield break;
                }
            }
        }

        feedbackText.text = "Save data uploaded successfully!";
    }

    private int GetLevelFromFilePath(string filePath)
    {
        if (filePath.Contains("playerlvl1")) return 1;
        if (filePath.Contains("playerlvl2")) return 2;
        if (filePath.Contains("playerlvl3")) return 3;
        return 0;
    }


    private IEnumerator DownloadSaveData()
    {
        string downloadUrl = baseURL + "gameData/load";
        string token = PlayerPrefs.GetString("auth_token");

        if (string.IsNullOrEmpty(token))
        {
            feedbackText.text = "No token found. Please log in.";
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-auth-token", token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Save data downloaded successfully!";
            string jsonData = request.downloadHandler.text;

            jsonData = jsonData.TrimStart('[').TrimEnd(']');

            string[] jsonObjects = jsonData.Split(new[] { "},{" }, System.StringSplitOptions.None);


            foreach (var jsonObject in jsonObjects)
            {
                //Debug.Log(jsonObject);
                string formattedJson = "{" + jsonObject + "}";

                // Find the level number in the JSON object
                int levelStartIndex = formattedJson.IndexOf("\"level\":") + "\"level\":".Length;
                int levelEndIndex = formattedJson.IndexOf(',', levelStartIndex);
                int level = int.Parse(formattedJson.Substring(levelStartIndex, levelEndIndex - levelStartIndex));

                //Debug.Log(level);

                int gameDataStartIndex = formattedJson.IndexOf("\"game_data\":") + "\"game_data\":".Length;
                string gameDataJson = formattedJson.Substring(gameDataStartIndex).TrimEnd('}') + "}";

                //Debug.Log(gameDataJson);
                string saveFilePath = Application.persistentDataPath + "/playerlvl" + level + ".json";
                System.IO.File.WriteAllText(saveFilePath, gameDataJson);
            }
        }
        else
        {
            feedbackText.text = "Error downloading save data: " + request.error;
            Debug.LogError(request.downloadHandler.text);
        }
    }

    public class RegistrationData
    {
        public string username;
        public string password;

        public RegistrationData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
