using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    public enum ActionType
    {
        AddPoints, Increase, Decrease, Invert, Slow, Fast
    }

    public ActionType Action;
}
