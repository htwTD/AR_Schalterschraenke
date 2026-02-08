using UnityEngine;
using System.Collections;
using UnityEngine.Android; // Crucial for Permissions

public class QuestCameraAccessor : MonoBehaviour
{
    public static QuestCameraAccessor Instance;
    private WebCamTexture questCamera;
    public string statusMessage = "Initializing..."; // Read this in your HUD

    void Awake()
    {
        Instance = this;
        StartCoroutine(StartCameraSequence());
    }

    IEnumerator StartCameraSequence()
    {
        // 1. Check Android Permissions (The "Hard" Check)
        statusMessage = "Checking Permissions...";
        yield return new WaitForSeconds(0.5f);

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            statusMessage = "Requesting Permission... Look for Popup!";
            yield return new WaitForSeconds(2.0f); // Wait for user to click "Allow"
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            statusMessage = "ERROR: Permission Denied by User.";
            yield break; // Stop here
        }

        // 2. Find Camera Devices
        statusMessage = "Searching for Cameras...";
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            statusMessage = "ERROR: No Cameras Found. (Check Manifest?)";
            yield break;
        }

        // 3. Start the Camera
        string camName = devices[0].name;
        statusMessage = "Starting: " + camName;

        questCamera = new WebCamTexture(camName, 1280, 720, 30);
        questCamera.Play();

        // 4. Wait for it to actually play
        float timeout = 0f;
        while (!questCamera.isPlaying && timeout < 5f)
        {
            timeout += Time.deltaTime;
            yield return null;
        }

        if (questCamera.isPlaying)
            statusMessage = "SUCCESS: Camera Running!";
        else
            statusMessage = "ERROR: Camera stuck (isPlaying = false)";
    }

    public Color GetColorAtScreenPoint(Vector2 screenPoint)
    {
        if (questCamera == null || !questCamera.isPlaying) return Color.black;

        // Map screen to camera UVs
        float u = screenPoint.x / Screen.width;
        float v = screenPoint.y / Screen.height;

        int x = Mathf.FloorToInt(u * questCamera.width);
        int y = Mathf.FloorToInt(v * questCamera.height);

        return questCamera.GetPixel(x, y);
    }
}