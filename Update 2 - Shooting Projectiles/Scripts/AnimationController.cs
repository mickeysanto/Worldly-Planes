using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerMove movement;
    private PlayerShoot attacks;

    private Vector3 crossProduct;  
    private Vector3 moveDirection3D; // 3D vector representation of the movement input vector
    private Vector2 forward2D; // 2D vector representation of the direction the playe is facing

    private float dotProduct;
    private float forwardWalkVal; // the Y axis for the player movement blend tree
    private float sideWalkVal;// the X axis for the player movement blend tree
    private float acceleration = 8; // speed of transitions between animations
    private float smoothTime = 0.1f; // smooths down transitions between animations

    private int forwardWalkValHash;
    private int sideWalkValHash;
    private int shootHash;
    private int holdShootHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = gameObject.GetComponentInParent<PlayerMove>();
        attacks = gameObject.GetComponentInParent<PlayerShoot>();
    }

    private void Start()
    {
        forward2D = Vector2.zero;

        forwardWalkVal = 0;
        sideWalkVal = 0;

        forwardWalkValHash = Animator.StringToHash("ForwardWalk");
        sideWalkValHash = Animator.StringToHash("SideWalk");
        shootHash = Animator.StringToHash("ShotProjectile");
        holdShootHash = Animator.StringToHash("ShootPressed");
    }

    private void Update()
    {
        Walk();
        Attack(attacks.isAttacking, attacks.shotProjectile);
    }

    private void Attack(bool keyDown, bool shotProj)
    {
        // if a projectile was just shot
        if (shotProj)
        {
            animator.SetBool(shootHash, true);
        }
        else
        {
            animator.SetBool(shootHash, false);
        }

        // if the player is holding down the attack button
        if (keyDown)
        {
            animator.SetBool(holdShootHash, true);
        }
        else
        {
            animator.SetBool(holdShootHash, false);
        }
    }

    private void Walk()
    {
        // if the player is in motion
        if(movement.isMoving)
        {
            forward2D.x = movement.forward.x;
            forward2D.y = movement.forward.z;

            moveDirection3D.x = movement.moveDirection.x;
            moveDirection3D.y = 0;
            moveDirection3D.z = movement.moveDirection.y;

            dotProduct = Vector2.Dot(forward2D, movement.moveDirection);
            crossProduct = Vector3.Cross(movement.forward, moveDirection3D);

            // if dotProduct > 0 player is walking forward else backwards
            if (dotProduct > 0)
            {
                if (forwardWalkVal < 1)
                {
                    forwardWalkVal += Time.deltaTime * acceleration;
                }

                // if the cross porduct is positive then the player is looking left else right relative to movement direction
                if (crossProduct.normalized.y > 0 && sideWalkVal > dotProduct - 1)
                {
                    sideWalkVal -= Time.deltaTime * acceleration;
                }
                else if(crossProduct.normalized.y < 0 && sideWalkVal < 1 - dotProduct)
                {
                    sideWalkVal += Time.deltaTime * acceleration;
                }
                else
                {
                    sideWalkVal = Mathf.Lerp(sideWalkVal, 0, Time.deltaTime / smoothTime);
                }
            }
            else
            {
                if (forwardWalkVal > -1)
                {
                    forwardWalkVal -= Time.deltaTime * acceleration;
                }

                if (crossProduct.normalized.y > 0 && sideWalkVal < 1 - (-1 * dotProduct))
                {
                    sideWalkVal += Time.deltaTime * acceleration;
                }
                else if (crossProduct.normalized.y < 0 && sideWalkVal > (-1 * dotProduct) - 1)
                {
                    sideWalkVal -= Time.deltaTime * acceleration;
                }
                else
                {
                    sideWalkVal = Mathf.Lerp(sideWalkVal, 0, Time.deltaTime / smoothTime);
                }
            }
        }
        else if(forwardWalkVal != 0 || sideWalkVal != 0)
        {
            transitionIdle(ref forwardWalkVal);
            transitionIdle(ref sideWalkVal);
        }

        animator.SetFloat(forwardWalkValHash, forwardWalkVal);
        animator.SetFloat(sideWalkValHash, sideWalkVal);
    }

    // transitions the character back to the Idle animation state 
    private void transitionIdle(ref float value)
    {
        if (value != 0)
        {
            value += ((value > 0 ? -1 : 1) * Time.deltaTime * acceleration);

            // accounts for overshooting 0
            if (Mathf.Abs(value) < Time.deltaTime * acceleration)
            {
                value = 0;
            }
        }
    }
}
