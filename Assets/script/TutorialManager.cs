using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TutorialStep[] steps;
    public TextMeshProUGUI instructionLabel;

    int currentStepIndex = -1;

    void Start()
    {
        if (steps != null && steps.Length > 0)
            GoToStep(0);
    }

    void GoToStep(int index)
    {
        if (index < 0 || index >= steps.Length)
            return;

        currentStepIndex = index;

        // update UI text
        if (instructionLabel != null)
            instructionLabel.text = steps[index].instructionText;

        // only enable snap zones for current step
        for (int i = 0; i < steps.Length; i++)
        {
            bool active = (i == index);
            if (steps[i].zonesToComplete == null) continue;

            foreach (var zone in steps[i].zonesToComplete)
            {
                if (zone != null)
                    zone.gameObject.SetActive(active);
            }
        }
    }

    // THIS is TutorialManager.OnZoneCompleted
    public void OnZoneCompleted()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length)
            return;

        var step = steps[currentStepIndex];

        // are all zones in this step filled?
        bool allFilled = true;
        if (step.zonesToComplete != null)
        {
            foreach (var zone in step.zonesToComplete)
            {
                if (zone != null && !zone.isFilled)
                {
                    allFilled = false;
                    break;
                }
            }
        }

        if (!allFilled)
            return;

        int next = currentStepIndex + 1;
        if (next < steps.Length)
        {
            GoToStep(next);
        }
        else
        {
            // finished tutorial
            if (instructionLabel != null)
                instructionLabel.text = "Cabinet assembly complete!";
        }
    }
}
