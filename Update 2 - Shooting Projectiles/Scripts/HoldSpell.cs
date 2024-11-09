using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSpell : MonoBehaviour
{
    public ParticleSystem holdSpell;

    private PlayerShoot attacks;

    private bool spellPause;

    void Start()
    {
        attacks = GetComponent<PlayerShoot>();
        spellPause = false;
    }   
    void Update()
    {
        // if the player is charging the next attack play the particle system
        if(attacks.isCharging && attacks.isAttacking && holdSpell.isStopped)
        {
            holdSpell.Play();
        }
        else if((!attacks.isCharging || !attacks.isAttacking) && holdSpell.isPlaying)
        {
            holdSpell.Stop(true);
            holdSpell.Clear();
        }
    }
}
