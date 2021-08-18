using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //Damage
    public int damage = 1;
    public float pushForce = 5;

    protected override void OnCollide(Collider2D collider2D)
    {
        //Si le joueur est au contact
        if(collider2D.tag == "Fighter" && collider2D.name == "Player"){
            //On crée un nouvel objet de dégats que l'on va envoyer au joueur 
            Damage dmg = new Damage(){
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            //On envoie l'objet de dégats vers celui qui va les recevoir
            collider2D.SendMessage("ReceiveDamage", dmg);

            Debug.Log("L'ennemi " + transform.parent.name + " inflige " + damage + " au joueur");
        }
    }
}
