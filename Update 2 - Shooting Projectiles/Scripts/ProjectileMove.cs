using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public ParticleSystem projectile = null; // the projectile particle system
    public ParticleSystem impact = null; // the particle system that plays on impact or max range

    public float speed = 2f; // projectile move speed
    public float sphereRadius = .28f; // radius of the sphere cast
    public float range = 10f; // how far the projectile moves before its deactivated

    private Vector3 direction;
    private Vector3 startingPoint;
    private RaycastHit hit;

    private bool projectileEnd;

    private void OnEnable()
    {
        startingPoint = transform.position;
        projectileEnd = false;
    }

    private void Update()
    {
        if(!projectileEnd)
        {
            direction = transform.forward;

            // if the projectile has gotten to its max range or hit something
            if (Physics.SphereCast(transform.position, sphereRadius, direction, out hit, speed * Time.deltaTime * 1.1f)
                || Vector3.Distance(startingPoint, transform.position) >= range)
            {
                projectileEnd = true;

                if (impact != null && projectile != null)
                {
                    projectile.Stop(true);
                    projectile.Clear();

                    StartCoroutine(playImpact());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                // moves projectile
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    // plays the impact particle system and then deactivates the projectile after some time
    private IEnumerator playImpact()
    {
        impact.Play();

        yield return StartCoroutine(Utils.Timer(.5f));
        gameObject.SetActive(false);
    }
}
