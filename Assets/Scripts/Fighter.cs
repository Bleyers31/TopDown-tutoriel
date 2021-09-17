using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitPoint = 10;
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f;

    //Durée invincibilité après un coup
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //Direction dans laquelle regarde le personnage (droite par défaut)
    public string lookAt = "right";

    //Poussée
    protected Vector3 pushDirection;

    //Tous les combattant peuvent recevoir des dégats et mourir
    protected virtual void ReceiveDamage(Damage dmg){
        if(Time.time - lastImmune > immuneTime){
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            //Pour savoir où on sera poussé, on compare notre position à celle de l'attaquant
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 15, Color.red, transform.position, Vector3.up * 75, 0.5f);

            if(hitPoint <= 0){
                hitPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death(){

    }

}
