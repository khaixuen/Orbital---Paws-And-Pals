using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public InputField emailField;
    public InputField passwordField;
    public Button loginButton;
    public Text messageText;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    private void OnLoginButtonClick()
    {
        string email = emailField.text;
        string password = passwordField.text;
        StartCoroutine(LoginUser(email, password));
    }

    private IEnumerator LoginUser(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://your-backend-url/api/auth/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                messageText.text = www.error;
            }
            else
            {
                // Assuming the response is a JSON object with a token
                string responseText = www.downloadHandler.text;
                // Parse JSON and extract token
                // For simplicity, assuming response format is { "token": "your-jwt-token" }
                var response = JsonUtility.FromJson<LoginResponse>(responseText);
                string token = response.token;
                messageText.text = "Login successful!";
                // Store the token or proceed to the next scene
            }
        }
    }

    [System.Serializable]
    private class LoginResponse
    {
        public string token;
    }
}
