using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Collidable
{
    public float fireBallSpeed;
    private new Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    public int damagePoint;
    public float pushForce;
    public float cooldown;
    private float lastCast;

   protected override void Start() {
        base.Start();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //Selon que le joueur regarde à droite ou à gauche, on lance dans le bon sens
        if(GameManager.instance.player.lookAt == "right"){
            rigidbody2D.velocity = transform.right * fireBallSpeed;
        }else{
            rigidbody2D.velocity = -transform.right * fireBallSpeed;
        }
        
    }

    //On redéfinit le comportement d'une collision dans le cas où c'est l'arme qui touche quelque chose
    protected override void OnCollide(Collider2D collider2D)
    {
        if(collider2D.tag == "Fighter"){
            if(collider2D.name != "Player"){

                //On crée un nouvel objet de dégats que l'on va envoyer à celui qui est rentré en collision avec la boule de feu
                Damage dmg = new Damage(){
                    damageAmount = damagePoint,
                    origin = transform.position,
                    pushForce = pushForce
                };

                //On envoie l'objet de dégats vers celui qui va les recevoir
                collider2D.SendMessage("ReceiveDamage", dmg);

                Debug.Log("Collision de la boule de feu avec " + collider2D.tag);
            }
        }else{
            Debug.Log("Collision avec une entité non vivante " + collider2D.tag);
        }

        //Autodestriction
        Destroy(gameObject);
    }

}
