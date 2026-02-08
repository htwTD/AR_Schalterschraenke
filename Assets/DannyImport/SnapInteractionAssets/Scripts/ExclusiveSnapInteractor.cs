using Oculus.Interaction;
using UnityEngine;

public class ExclusiveSnapInteractor : MonoBehaviour
{
    [Header("Erlaubter Snap Interactable")]
    [SerializeField] private SnapInteractable allowedSnapInteractable;

    private SnapInteractor snapInteractor;

    private void Awake()
    {
        // SnapInteractor im gleichen Objekt oder Kind-Objekten suchen
        snapInteractor = GetComponentInChildren<SnapInteractor>();

        if (snapInteractor == null)
        {
            Debug.LogError("SnapInteractor nicht gefunden!");
            return;
        }

        if (allowedSnapInteractable == null)
        {
            Debug.LogError("Erlaubter SnapInteractable nicht zugewiesen!");
            return;
        }
    }

    private void Start()
    {
        // Nur der zugewiesene SnapInteractable darf gesnapped werden
        snapInteractor.SetComputeCandidateOverride(() => allowedSnapInteractable);
    }
}
