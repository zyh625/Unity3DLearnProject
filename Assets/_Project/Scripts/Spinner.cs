
using UnityEngine;
public class Spinner : MonoBehaviour
{
    public enum Mode { Idle,SpinSelf,SpinWorld,LookAtTarget,RotateAroundTarget,NormalAroundTarget};
    public float spinSpeed = 90f;
    public Transform target;
    public float turnSpeed = 120f;
    public Mode mode = Mode.Idle;
    void Update()
    {
        switch (mode)
        {
            case Mode.Idle:
                break;
            case Mode.SpinSelf:
                transform.Rotate(spinSpeed * Vector3.up * Time.deltaTime,
                    Space.Self);
                break;
            case Mode.SpinWorld:
                transform.Rotate(spinSpeed * Vector3.up * Time.deltaTime,
                    Space.World);
                break;
            case Mode.LookAtTarget:
                Vector3 dir = target.position - transform.position;
                Vector3 r = transform.eulerAngles;
                Quaternion q1 = Quaternion.Euler(r.x, r.y, r.z);
                Quaternion q2 = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards
                    (q1, q2, turnSpeed * Time.deltaTime);
                break;
            case Mode.RotateAroundTarget:
                transform.RotateAround(target.position, Vector3.up,
                    Time.deltaTime * spinSpeed);
                break;
            case Mode.NormalAroundTarget:
                Vector3 offset = transform.position - target.position;
                transform.position = Quaternion.AngleAxis(spinSpeed * Time.deltaTime,
                    Vector3.up) * offset + target.position;
                break;
        }
    }
}   
