using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Action<Vector3> DirectionChange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            DirectionChange?.Invoke(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            DirectionChange?.Invoke(Vector3.back);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            DirectionChange?.Invoke(Vector3.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            DirectionChange?.Invoke(Vector3.right);
    }
}
