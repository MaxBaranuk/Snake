using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public static Action<float> OnSpeedChange;
    public static readonly List<Target> Targets = new List<Target>();
    public Transform CameraHolder;
    private readonly List<Chain> _chains = new List<Chain>();
    private float _currentSpeed = 0.025f;

    private void Awake()
    {
        var first =  AssetProvider.GetChain();
        _chains.Add(first);
        first.transform.parent = transform;
        first.IsFirst = true;
        var last = AssetProvider.GetChain();
        _chains.Add(last);
        last.transform.parent = transform;
        var target1 = new Target()
        {
            Position = new Vector3((int) (transform.position.x + transform.forward.x),
                transform.position.y,
                (int) (transform.position.z + transform.forward.z))
        };
        Targets.Add(target1);
        var target2 = new Target()
        {
            Position = transform.position
        };
        Targets.Add(target2);

        target1.Previous = target2;
        target2.Next = target1;
        first.CurrTarget = target1; 
        last.CurrTarget = target2; 
        last.transform.position = transform.position - transform.forward;
        last.IsLast = true;
        InputController.DirectionChange += Rotate;
        Chain.OnSnakeAction += SnakeAction;
    }

    private void Start()
    {
        OnSpeedChange?.Invoke(_currentSpeed);
    }

    private void Update()
    {
        CameraHolder.position = _chains[0].transform.position;
    }

    public static void AddTarget(Target target)
    {
        Targets.Add(target);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction == _chains[0].transform.forward
        || direction == -_chains[0].transform.forward
        || _chains[0].CurrTarget.Next != null)
            return;
        
        var nextTarget = new Target()
        {
            Position = _chains[0].CurrTarget.Position + direction,
            Previous = _chains[0].CurrTarget
        };
        Targets.Add(nextTarget);
        _chains[0].CurrTarget.Next = nextTarget;
    }

    private void AddChain()
    {
        StartCoroutine(CreateChain(_chains[0].transform.position));
    }

    private IEnumerator CreateChain(Vector3 position)
    {
        yield return new WaitUntil(() => Vector3.Distance(position, _chains.Last().transform.position) < 0.01f);
        yield return new WaitUntil(() => Vector3.Distance(position, _chains.Last().transform.position) > 0.98f);
        var chain = AssetProvider.GetChain();
        chain.transform.parent = transform;
        chain.CurrTarget = _chains.Last().CurrTarget;
        chain.transform.position = _chains.Last().transform.position - _chains.Last().transform.forward;
        chain.Speed = _chains.Last().Speed;
        _chains.Last().IsLast = false;
        _chains.Add(chain);
        _chains.Last().IsLast = true;
    }
    
    public void SnakeAction(SceneObject.ActionType action)
    {
        switch (action)
        {
            case SceneObject.ActionType.AddPoints:
                GameManager.Instance.AddPoints(20);
                AddChain();
                break;
            case SceneObject.ActionType.Increase:
                AddChain();
                break;
            case SceneObject.ActionType.Decrease:
                RemoveHalf();
                break;
            case SceneObject.ActionType.Invert:
                InvertSnake();
                break;
            case SceneObject.ActionType.Slow:
                StartCoroutine(ChangeSpeed(.5f));
                break;
            case SceneObject.ActionType.Fast:
                StartCoroutine(ChangeSpeed(2f));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    private void InvertSnake()
    {
        _chains[0].IsFirst = false;
        _chains.Last().IsLast = false;
        _chains.Reverse();
        _chains[0].IsFirst = true;
        _chains.Last().IsLast = true;
       
        Targets.ForEach(target =>
        {
            var t = target.Next;
            target.Next = target.Previous;
            target.Previous = t;
        });
        _chains.ForEach(chain => chain.Rotate = true);
    }

    private IEnumerator ChangeSpeed(float speedFactor)
    {
        _currentSpeed *= speedFactor;
        OnSpeedChange?.Invoke(_currentSpeed);
        yield return new WaitForSeconds(20);
        _currentSpeed /= speedFactor;
        OnSpeedChange?.Invoke(_currentSpeed);
    }

    private void RemoveHalf()
    {
        if (_chains.Count < 4)
            return;
        var count = _chains.Count / 2;
        for (var i = _chains.Count - 1; i > count; i--)
        {
            Destroy(_chains[i]);
            _chains.RemoveAt(i);
        }
        _chains.Last().IsLast = true;
    }

    private void OnDestroy()
    {
        if (InputController.DirectionChange != null) 
            InputController.DirectionChange -= Rotate;
        if (Chain.OnSnakeAction != null) 
            Chain.OnSnakeAction -= SnakeAction;
        _chains.ForEach(chain => Destroy(chain.gameObject));
        _chains.Clear();
        Targets.Clear();
    }
}
