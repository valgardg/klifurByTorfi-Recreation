using UnityEngine;

public class SimpleDragController : MonoBehaviour
{
    public TargetJoint2D joint;
    // public Transform handTarget;

    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
        joint.enabled = false;
    }

    void Update()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            joint.enabled = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            joint.enabled = false;
        }

        if (isDragging)
        {
            // handTarget.position = mouseWorld;
            joint.target = mouseWorld;
        }
    }
}
