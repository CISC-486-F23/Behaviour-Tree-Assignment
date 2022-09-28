using UnityEngine;

public class PlayerAim : MonoBehaviour {
    private GameObject aimPoint;
    private Rigidbody2D aimRb2D;

    void Start() {
        aimPoint = GameObject.Find("AimPoint");
        aimRb2D = aimPoint.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        MoveAimPoint();
    }

    void MoveAimPoint() {
        // Get angle of the aim vector
        AngleOfAim = Mathf.Atan2(aimPoint.transform.localPosition.y, aimPoint.transform.localPosition.x) * Mathf.Rad2Deg;
    }

    public void Aim(Vector3 target)
    {
        // Get vector relative to player and mouse position in camera
        Vector2 aim = transform.position - target;
        aim *= -1f;
        aimPoint.transform.localPosition = aim;
    }

    public float AngleOfAim { get; private set; } = 0;
}
