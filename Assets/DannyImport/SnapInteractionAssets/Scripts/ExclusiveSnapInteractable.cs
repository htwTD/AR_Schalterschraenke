using Oculus.Interaction;
using UnityEngine;

public class ExclusiveSnapInteractable : MonoBehaviour
{
    [Header("Einziger erlaubter Interactor")]
    [SerializeField] private SnapInteractor allowedInteractor;

    private SnapInteractable snapInteractable;

    private void Awake()
    {
        snapInteractable = GetComponentInChildren<SnapInteractable>();

        if (snapInteractable == null)
        {
            Debug.LogError("SnapInteractable nicht gefunden!");
            return;
        }

        if (allowedInteractor == null)
        {
            Debug.LogError("Erlaubter SnapInteractor nicht zugewiesen!");
            return;
        }
    }

    private void Start()
    {
        // Max Selecting Interactors auf 1 setzen
        snapInteractable.MaxSelectingInteractors = 1;
    }
}
