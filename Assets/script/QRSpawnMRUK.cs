using Meta.XR.MRUtilityKit;
using UnityEngine;

public class QRSpawnMRUK : MonoBehaviour
{
    [SerializeField] private MRUK mruk;
    [SerializeField] private GameObject prefabToSpawn;

    // NEW
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffsetEuler;

    private void Start()
    {
        if (mruk == null)
            mruk = MRUK.Instance;

        if (mruk == null)
        {
            Debug.LogError("MRUK reference missing!");
            return;
        }

        mruk.SceneSettings.TrackableAdded.AddListener(OnTrackableAdded);
        mruk.SceneSettings.TrackableRemoved.AddListener(OnTrackableRemoved);
    }

    private void OnDestroy()
    {
        if (mruk == null) return;
        mruk.SceneSettings.TrackableAdded.RemoveListener(OnTrackableAdded);
        mruk.SceneSettings.TrackableRemoved.RemoveListener(OnTrackableRemoved);
    }

    private void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            return;

        var obj = Instantiate(prefabToSpawn, trackable.transform);

        // Apply offsets RELATIVE to the QR pose
        obj.transform.localPosition = positionOffset;
        obj.transform.localRotation = Quaternion.Euler(rotationOffsetEuler);
    }

    private void OnTrackableRemoved(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            return;

        foreach (Transform child in trackable.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
