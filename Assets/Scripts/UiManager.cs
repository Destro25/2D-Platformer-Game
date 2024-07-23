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

        // Create an instance of the RegistrationData class
        RegistrationData registrationData = new RegistrationData(username, password);

        // Convert the instance to JSON
        string jsonPayload = JsonUtility.ToJson(registrationData);

        Debug.Log("JSON Payload: " + jsonPayload);  // For debugging

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

        Debug.Log("JSON Payload for Login: " + jsonPayload);

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

            Debug.Log("Stored Token: " + token);  // Log the stored token for debugging
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
        string saveFilePath = Application.persistentDataPath + "/playerlvl1.json";
        if (!System.IO.File.Exists(saveFilePath))
        {
            feedbackText.text = "No local save data found.";
            yield break;
        }

        string jsonData = System.IO.File.ReadAllText(saveFilePath);
        Debug.Log("Raw JSON Data: " + jsonData);  // Debug the raw JSON data

        string uploadUrl = baseURL + "gameData/save";
        string token = PlayerPrefs.GetString("auth_token");

        if (string.IsNullOrEmpty(token))
        {
            feedbackText.text = "No token found. Please log in.";
            yield break;
        }

        Debug.Log("Token: " + token);  // Log the token for debugging

        UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-auth-token", token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Save data uploaded successfully!";
        }
        else
        {
            feedbackText.text = "Error uploading save data: " + request.error;
            Debug.LogError(request.downloadHandler.text);  // Log server response
        }
    }


    private IEnumerator DownloadSaveData()
    {
        string downloadUrl = baseURL + "gameData/load";
        string token = PlayerPrefs.GetString("auth_token");
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-auth-token", token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Save data downloaded successfully!";
            string jsonData = request.downloadHandler.text;
            string saveFilePath = Application.persistentDataPath + "/savefile.json";
            System.IO.File.WriteAllText(saveFilePath, jsonData);
        }
        else
        {
            feedbackText.text = "Error downloading save data: " + request.error;
            Debug.LogError(request.downloadHandler.text);  // Log server response
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
