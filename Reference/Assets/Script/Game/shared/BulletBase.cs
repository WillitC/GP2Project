using UnityEngine;
using UnityEngine.Events;


public abstract class BulletBase : MonoBehaviour
{
    public GameObject Player { get; private set; }
    /*public Vector3 InitialPosition { get; private set; }
    public Vector3 InitialDirection { get; private set; }
    public Vector3 InheritedMuzzleVelocity { get; private set; }
    public float InitialCharge { get; private set; }*/

    public UnityAction OnFire;

    public void Fire(WeaponController controller)
    {
        Player = controller.Player;
        /*InitialPosition = transform.position;
        InitialDirection = transform.forward;
        InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
        InitialCharge = controller.CurrentCharge;*/

        OnFire?.Invoke();
    }
}
