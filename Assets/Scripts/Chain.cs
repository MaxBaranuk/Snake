using System;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public static Action<SceneObject.ActionType> OnSnakeAction;
    public bool Rotate;
    public Target CurrTarget;
    public bool IsLast;
    public bool IsFirst;
    public float Speed;

    private void Awake()
    {
        Snake.OnSpeedChange += speed =>
        {
            Speed = speed;
        };
    }

    private void FixedUpdate()
    {        
        CheckTargetReached();
        transform.LookAt(CurrTarget.Position);
        transform.Translate(transform.forward * Speed, Space.World);             
    }

    private void CheckTargetReached()
    {
        if (!(Vector3.Distance(transform.position, CurrTarget.Position) < 0.01f)) return;
        transform.position = CurrTarget.Position;
        if (Math.Abs(transform.position.x) >= 10
            || Math.Abs(transform.position.z) >= 10)
        {
            transform.Translate(-transform.forward * 20, Space.World);
        }
        if (CurrTarget.Next == null)
        {
            var target = new Target()
            {
                Previous = CurrTarget,
                Position = transform.position + (Rotate ? - transform.forward: transform.forward)
            };
            CurrTarget.Next = target;
            Snake.AddTarget(target);
        }

        Rotate = false;
        var t = CurrTarget;
        CurrTarget = CurrTarget.Next;
        if (!IsLast) return;
        Snake.Targets.Remove(t);
        CurrTarget.Previous = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        var fruit = other.gameObject.GetComponent<SceneObject>();
        if (fruit == null || !IsFirst) return;
        OnSnakeAction?.Invoke(fruit.Action);
        Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsFirst || other.gameObject.GetComponent<Chain>() != null)
            GameManager.Instance.FinishGame();
    }
}
