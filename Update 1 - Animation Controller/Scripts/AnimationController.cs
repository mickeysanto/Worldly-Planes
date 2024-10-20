using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerMove movement;

    private Vector3 crossProduct;  
    private Vector3 moveDirection3D; // 3D vector representation of the movement input vector
    private Vector2 forward2D; // 2D vector representation of the direction the playe is facing

    private float dotProduct;
    private float walkVal; // the Y axis for the player movement blend tree
    private float sideWalkVal;// the X axis for the player movement blend tree
    private float acceleration = 8; // speed of transitions between animations

    private int walkValHash;
    private int sideWalkValHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = gameObject.GetComponentInParent<PlayerMove>();

        forward2D = Vector2.zero;

        walkVal = 0;
        sideWalkVal = 0;

        walkValHash = Animator.StringToHash("Walk");
        sideWalkValHash = Animator.StringToHash("SideWalk");
    }

    private void Update()
    {
        Walk();
    }

    private void Walk()
    {
        // if the player is in motion
        if(movement.moveDirection != Vector2.zero)
        {
            forward2D.x = movement.forward.x;
            forward2D.y = movement.forward.z;

            moveDirection3D.x = movement.moveDirection.x;
            moveDirection3D.y = 0;
            moveDirection3D.z = movement.moveDirection.y;

            dotProduct = Vector2.Dot(forward2D, movement.moveDirection);
            crossProduct = Vector3.Cross(movement.forward, moveDirection3D);

            walkVal = dotProduct;

            // if dotProduct > 0 player is walking forward else backwards
            if (dotProduct > 0)
            {
                // if the cross porduct is positive then the player is moving left else right
                if(crossProduct.normalized.y > 0 && sideWalkVal > -1)
                {
                    sideWalkVal -= Time.deltaTime * acceleration;
                }
                else if(crossProduct.normalized.y < 0 && sideWalkVal < 1)
                {
                    sideWalkVal += Time.deltaTime * acceleration;
                }
            }
            else
            {
                if (crossProduct.normalized.y > 0 && sideWalkVal < 2)
                {
                    sideWalkVal += Time.deltaTime * acceleration;
                }
                else if (crossProduct.normalized.y < 0 && sideWalkVal > -2)
                {
                    sideWalkVal -= Time.deltaTime * acceleration;
                }
            }
        }
        else if(walkVal != 0 || sideWalkVal != 0)
        {
            transitionIdle(ref walkVal);
            transitionIdle(ref sideWalkVal);
        }

        animator.SetFloat(walkValHash, walkVal);
        animator.SetFloat(sideWalkValHash, sideWalkVal);
    }

    //transitions the character back to the Idle animation state 
    private void transitionIdle(ref float value)
    {
        if (value != 0)
        {
            value += ((value > 0 ? -1 : 1) * Time.deltaTime * acceleration);

            if (Mathf.Abs(value) < Time.deltaTime * acceleration)
            {
                value = 0;
            }
        }
    }

    private void DebugLog()
    {
        Debug.Log("Dot: " + dotProduct);
        Debug.Log("Cross: " + crossProduct);
        Debug.Log("Aim Direction: " + forward2D);
        Debug.Log("Move Direction: " + movement.moveDirection);
    }
}
