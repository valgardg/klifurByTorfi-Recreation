using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HandGrab : MonoBehaviour
{
    [Header("Start hold")]
    public Collider2D startHold;

    [Header("Detection")]
    [Tooltip("Layer(s) that bouldering holds live on.")]
    public LayerMask holdMask = ~0; // default: everything; narrow this in the inspector
    [Tooltip("Include colliders marked as triggers when searching for holds.")]
    public bool holdsAreTriggers = true;

    private FixedJoint2D grabJoint;
    private Collider2D handCollider;
    private readonly Collider2D[] overlapBuffer = new Collider2D[8];

    void Awake()
    {
        handCollider = GetComponent<Collider2D>();
        if (startHold != null) GrabStartHold(startHold);
    }

    void OnMouseDown() => ReleaseHold();

    void OnMouseUp()
    {
        Collider2D hold = FindHoldUnderHand();
        if (hold != null) GrabHold(hold);
    }

    private Collider2D FindHoldUnderHand()
    {
        var filter = new ContactFilter2D
        {
            useTriggers = holdsAreTriggers,
            useLayerMask = true,
        };
        filter.SetLayerMask(holdMask);

        int count = handCollider.Overlap(filter, overlapBuffer);
        for (int i = 0; i < count; i++)
        {
            var c = overlapBuffer[i];
            if (c != null && c.CompareTag("Hold")) return c;
        }
        return null;
    }

    private void GrabHold(Collider2D hold)
    {
        if (hold == null || grabJoint != null) return;

        grabJoint = gameObject.AddComponent<FixedJoint2D>();
        grabJoint.connectedBody = hold.attachedRigidbody;
    }

    private void GrabStartHold(Collider2D hold)
    {
        if (hold == null || grabJoint != null) return;

        grabJoint = gameObject.AddComponent<FixedJoint2D>();
        grabJoint.connectedBody = hold.attachedRigidbody;
        grabJoint.autoConfigureConnectedAnchor = false;
        grabJoint.anchor = Vector2.zero;
        grabJoint.connectedAnchor = Vector2.zero;
    }

    private void ReleaseHold()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
        }
    }
}