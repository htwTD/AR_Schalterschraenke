using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorDebugHUD : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public Image colorPreview;

    void Update()
    {
        // 1. Get the Status from the Manager
        string status = QuestCameraAccessor.Instance.statusMessage;

        // 2. Get the Color
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Color c = QuestCameraAccessor.Instance.GetColorAtScreenPoint(center);

        // 3. Show it all
        if (colorPreview != null) colorPreview.color = c;
        if (debugText != null)
        {
            debugText.text = $"STATUS: {status}\n\n" +
                             $"R: {c.r:F2}\n" +
                             $"G: {c.g:F2}\n" +
                             $"B: {c.b:F2}";
        }
    }
}