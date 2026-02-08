using UnityEngine;

public class CableChecker : MonoBehaviour
{
    public enum ExpectedColor { Blue, YellowGreen, Black }
    public ExpectedColor wireColor;

    // Assign a green transparent cube prefab here in the Inspector
    public GameObject successPrefab;
    // Assign a red wireframe cube prefab here in the Inspector
    public GameObject errorPrefab;

    private bool isCompleted = false;

    // Call this via a button or a timer
    public void CheckConnection()
    {
        if (isCompleted) return;

        // 1. Get Screen Position of this Anchor
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        // 2. Check Logic
        bool isSuccess = false;

        // Get the color from our Camera Manager
        Color detectedColor = QuestCameraAccessor.Instance.GetColorAtScreenPoint(screenPos);

        if (wireColor == ExpectedColor.Black)
        {
            // Use the Special "White Label" finder for black wires
            if (CheckForWhiteTagNearby(screenPos)) isSuccess = true;
        }
        else
        {
            // Simple Color Check for Blue/Yellow
            if (IsColorMatch(detectedColor, wireColor)) isSuccess = true;
        }

        // 3. Feedback
        if (isSuccess)
        {
            Debug.Log("Success! Wire Connected.");
            if (successPrefab) Instantiate(successPrefab, transform.position, Quaternion.identity);
            isCompleted = true;
        }
        else
        {
            Debug.Log("Wrong connection detected.");
            if (errorPrefab) Instantiate(errorPrefab, transform.position, Quaternion.identity);
        }
    }

    // --- HELPER FUNCTIONS ---

    bool IsColorMatch(Color c, ExpectedColor expected)
    {
        if (expected == ExpectedColor.Blue)
            return (c.b > c.r + 0.1f && c.b > c.g + 0.1f);

        if (expected == ExpectedColor.YellowGreen)
            return (c.r > 0.5f && c.g > 0.5f && c.b < 0.4f);

        return false;
    }

    bool CheckForWhiteTagNearby(Vector3 screenPos)
    {
        int range = 20;
        bool foundDarkWire = false;
        bool foundWhiteLabel = false;

        for (int x = -range; x <= range; x += 5)
        {
            for (int y = -range; y <= range; y += 5)
            {
                Vector2 checkPos = new Vector2(screenPos.x + x, screenPos.y + y);
                Color c = QuestCameraAccessor.Instance.GetColorAtScreenPoint(checkPos);

                if (c.r < 0.2f && c.g < 0.2f && c.b < 0.2f) foundDarkWire = true;

                float brightness = (c.r + c.g + c.b) / 3.0f;
                if (brightness > 0.85f && (Mathf.Abs(c.r - c.b) < 0.1f)) foundWhiteLabel = true;
            }
        }
        return foundDarkWire && foundWhiteLabel;
    }
}