using UnityEngine;

public class IKArm2D : MonoBehaviour
{
    [Header("Bones")]
    public Transform upperArm;
    public Transform foreArm;
    public Transform armTip;   // empty child at the end of foreArm, never moves externally

    [Header("Target")]
    public Transform hand;     // the physics object — read only, never parented under foreArm

    [Header("Options")]
    public bool elbowFlip = false;

    private float upperArmLength;
    private float foreArmLength;

    void Start()
    {
        upperArmLength = Vector2.Distance(upperArm.position, foreArm.position);
        foreArmLength  = Vector2.Distance(foreArm.position, armTip.position);
    }

    void LateUpdate()
    {
        SolveIK();
    }

    void SolveIK()
    {
        Vector2 origin    = upperArm.position;
        Vector2 targetPos = hand.position;    // reads position only, hand is never moved here
        Vector2 toTarget  = targetPos - origin;
        float   dist      = toTarget.magnitude;

        float maxReach = upperArmLength + foreArmLength;
        float minReach = Mathf.Abs(upperArmLength - foreArmLength);
        dist = Mathf.Clamp(dist, minReach + 0.001f, maxReach - 0.001f);

        Vector2 dir           = toTarget.normalized;
        Vector2 clampedTarget = origin + dir * dist;

        float cosAngle = (upperArmLength * upperArmLength + dist * dist - foreArmLength * foreArmLength)
                         / (2f * upperArmLength * dist);
        float angle     = Mathf.Acos(Mathf.Clamp(cosAngle, -1f, 1f)) * Mathf.Rad2Deg;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float sign          = elbowFlip ? -1f : 1f;
        float upperArmAngle = baseAngle - sign * angle;

        upperArm.rotation = Quaternion.Euler(0f, 0f, upperArmAngle);

        Vector2 elbowPos = (Vector2)upperArm.position
                         + new Vector2(Mathf.Cos(upperArmAngle * Mathf.Deg2Rad),
                                       Mathf.Sin(upperArmAngle * Mathf.Deg2Rad)) * upperArmLength;

        Vector2 toHand     = clampedTarget - elbowPos;
        float foreArmAngle = Mathf.Atan2(toHand.y, toHand.x) * Mathf.Rad2Deg;

        foreArm.rotation = Quaternion.Euler(0f, 0f, foreArmAngle);
    }
}