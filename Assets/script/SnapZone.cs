using UnityEngine;
using UnityEngine.Events;

public class SnapZone : MonoBehaviour
{
    [Header("Which component should go here?")]
    public string expectedComponentId;      

    [Header("Settings")]
    public float maxSnapDistance = 0.5f;

    [Header("Events")]
    public UnityEvent onCorrectPlaced;
    public UnityEvent onWrongPlaced;

    [Header("Debug")]
    public bool logDebug = false;

    [HideInInspector] 
    public bool isFilled = false;

    private void OnTriggerStay(Collider other)
    {
        if (isFilled)
        {
            if (logDebug) Debug.Log($"[SnapZone {name}] already filled.");
            return;
        }

        // Look for ComponentId on the thing inside the zone
        ComponentId comp = other.GetComponentInParent<ComponentId>();
        if (comp == null)
        {
            if (logDebug) Debug.Log($"[SnapZone {name}] {other.name} has no ComponentId.");
            return;
        }

        if (logDebug) Debug.Log($"[SnapZone {name}] Found ComponentId '{comp.id}' on {comp.name}");

        // Check distance
        float distance = Vector3.Distance(comp.transform.position, transform.position);
        if (distance > maxSnapDistance)
        {
            if (logDebug) Debug.Log($"[SnapZone {name}] Too far to snap. dist = {distance}");
            return;
        }

        // Wrong piece?
        if (comp.id != expectedComponentId)
        {
            if (logDebug) Debug.Log($"[SnapZone {name}] WRONG piece. Expected '{expectedComponentId}', got '{comp.id}'");
            onWrongPlaced?.Invoke();
            return;
        }

        // ---- CORRECT PIECE -> SNAP & LOCK ----
        if (logDebug) Debug.Log($"[SnapZone {name}] CORRECT piece, snapping {comp.name}");

        Transform t = comp.transform;

        // Put it exactly at the snap zone
        t.position = transform.position;
        t.rotation = transform.rotation;

        // Stop physics
        Rigidbody rb = comp.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Disable all colliders so it cannot be grabbed again
        foreach (var col in comp.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        isFilled = true;
        onCorrectPlaced?.Invoke();
    }
}
