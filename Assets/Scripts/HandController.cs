using UnityEngine;

public class HandController : MonoBehaviour
{
    public TargetJoint2D joint;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        joint.enabled = false;
    }

    void OnMouseDrag()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        joint.enabled = true;
        joint.target = mouseWorld;
    }

    void OnMouseUp()
    {
        joint.enabled = false;
    }
}
