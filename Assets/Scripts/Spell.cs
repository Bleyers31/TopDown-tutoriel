using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe simple permettant de définir la base de tous les sorts afin qu'ils en hérite
public class Spell : Collidable
{
    public Sprite spellIcon;
    public float projectileSpeed;
    private new Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    public int damagePoint;
    public int manaCost;
    public float pushForce;
    public float cooldown;
    private float lastCast;
    public float duration; //Durée de vie à l'écran du spell
    public float momentOfCast;

   protected override void Start() {
        base.Start();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //Selon que le joueur regarde à droite ou à gauche, on lance dans le bon sens
        if(GameManager.instance.player.lookAt == "right"){
            rigidbody2D.velocity = transform.right * projectileSpeed;
        }else{
            rigidbody2D.velocity = -transform.right * projectileSpeed;
        }

        momentOfCast = Time.time;
        
    }

    //On redéfinit le comportement d'une collision (effet pouvant être différent selon l'arme)
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

                Debug.Log("Collision et dégâts appliqué à l'entité : " + collider2D.tag);
            }
        }else{
            Debug.Log("Collision avec une entité non vivante : " + collider2D.tag);
        }

        //Autodestruction
        Destroy(gameObject);
    }

    private void FixedUpdate() {
        //Si le spell dépasse sa durée de vie max, on le détruit
        if(Time.time - momentOfCast > duration){
            Destroy(gameObject);
        }
    }
}
