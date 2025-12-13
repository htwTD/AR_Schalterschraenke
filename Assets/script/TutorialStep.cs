using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public string stepName;
    [TextArea] public string instructionText;
    public SnapZone[] zonesToComplete; // usually 1, but can be >1
}
