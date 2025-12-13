using UnityEngine;

public class SimpleSnapZone : MonoBehaviour
{
    [Header("Which component should go here?")]
    public string expectedId = "1";     // must match ComponentId.id

    [Header("Snap settings")]
    public float snapDistance = 0.3f;   // meters
    public bool logDebug = true;

    private void OnTriggerStay(Collider other)
    {
        // Find a ComponentId on the object (or its parents)
        var comp = other.GetComponentInParent<ComponentId>();
        if (comp == null) return;

        if (logDebug)
            Debug.Log($"[SimpleSnapZone {name}] hit {comp.name} with id='{comp.id}'");

        // Wrong part? ignore
        if (comp.id != expectedId) return;

        // Too far away from the snap center? ignore
        float dist = Vector3.Distance(comp.transform.position, transform.position);
        if (dist > snapDistance)
        {
            if (logDebug)
                Debug.Log($"[SimpleSnapZone {name}] too far to snap: {dist}");
            return;
        }

        // ----- SNAP IT -----
        if (logDebug)
            Debug.Log($"[SimpleSnapZone {name}] SNAPPING {comp.name}");

        Transform t = comp.transform;
        t.position = transform.position;
        t.rotation = transform.rotation;

        var rb = comp.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
