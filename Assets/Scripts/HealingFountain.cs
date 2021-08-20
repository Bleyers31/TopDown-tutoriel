using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable
{
    public int healingAmount = 2;

    private float healCooldown = 0.5f;
    private float lastHeal;

    protected override void OnCollide(Collider2D collider2D)
    {
        //Le soin ne doit s'appliquer que au joueur
        if(collider2D.name != "Player"){
            return;
        }

        if(Time.time - lastHeal > healCooldown){
            lastHeal = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }
        
    }
}
