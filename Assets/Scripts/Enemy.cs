using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //Expérience donnée par l'ennemi
    public int xpValue = 1;

    //IA de l'ennemi
    public float triggerLegnth = 1;    // Portée pour commencer à poursuivre le joueur
    public float chaseLength = 5;      // Portée pour arreter de poursuivre le joueur
    private bool chasing;              // En train de poursuivre le joueur ?
    private bool collidingWithPlayer;  // Collision avec le joueur?
    private Transform playerTransform; // Position du joueur
    private Vector3 startingPosition;  // Point de départ de l'ennemi

    //Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10]; 

    //Barre de vie de l'ennemi
    public EnemyHealthBar enemyHealthBar;


    protected override void Start() {
        base.Start();
        //Position du joueur
        playerTransform = GameManager.instance.player.transform;
        //Position de départ de l'ennemi
        startingPosition = transform.position;
        //On récupère la hitbox du BoxCollider enfant de l'ennemi
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        //On remplit la barre de vie
        enemyHealthBar.SetHealth(hitPoint, maxHitPoint);
    }

    private void FixedUpdate() {
        //Le joueur est-il dans le rayon de poursuite de l'ennemi ?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLength){
            //Le joueur est-il dans le rayon de début de poursuite ?
            if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLegnth){
                chasing = true;
            }

            if(chasing){
                if(!collidingWithPlayer){
                    //On va vers le joueur
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }else{
                //Le joueur n'est plus dans le rayon, retour au point de départ
                UpdateMotor(startingPosition - transform.position);
            }
        }else{
            //Le joueur n'est pas dans le rayon de poursuite, on retourne au point de départ
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        //On vérifie si l'ennemi est au contact du joueur
        collidingWithPlayer = false;

        //Système de collision
        boxCollider2D.OverlapCollider(filter, hits);
        for (var i = 0; i < hits.Length; i++)
        {
            if(hits[i] == null){
                continue;
            }

            if(hits[i].tag == "Fighter" && hits[i].name == "Player"){
                collidingWithPlayer = true;
            }

            //On vide les collisions trouvées
            hits[i] = null;
        }
    } 

    //Quand un ennemi meurt, on attribue l'xp au joueur
    protected override void Death(){
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        
        //On actualise la barre de vie
        enemyHealthBar.SetHealth(hitPoint, maxHitPoint);
    }

}
