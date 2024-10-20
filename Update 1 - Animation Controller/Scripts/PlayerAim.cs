using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    private Vector2 aimDirection;
    private Vector2 mousePosition;
    private Vector3 worldDirection;

    private void Start()
    {
        mousePosition = Vector2.zero;
        worldDirection = Vector3.zero; 
    }

    private void Update()
    {
        Aim();   
    }

    private void Aim()
    {
        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.y;

        aimDirection = mousePosition - screenCenter;

        worldDirection = new Vector3(aimDirection.x, 0, aimDirection.y);

        if (worldDirection.magnitude > 0.01f)
        {
            transform.forward = worldDirection.normalized;
        }
    }
}
