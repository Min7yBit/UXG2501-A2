using UnityEngine;
using LineworkLite.FreeOutline;

public class CrosshairOutline : MonoBehaviour
{
    public float maxDistance = 100f;
    public LayerMask interactLayer;

    private Outline currentOutline;

    private GameObject currentObject;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance))
        {
            if (hit.collider.gameObject != currentObject)
            {
                ResetCurrent();
                currentObject = hit.collider.gameObject;
                currentObject.layer = LayerMask.NameToLayer("Outline");
            }
            return;
        }

        ResetCurrent();
    }

    void ResetCurrent()
    {
        if (currentObject != null)
        {
            currentObject.layer = LayerMask.NameToLayer("Default");
            currentObject = null;
        }
    }
}