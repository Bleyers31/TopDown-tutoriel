using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    //Filtre de collission
    public ContactFilter2D filter2D;

    private BoxCollider2D boxCollider2D;
    //Nombre max d'items avec lesquels le joueur peut être en collision en même temps (10 est énorme)
    private Collider2D[] hits = new Collider2D[10];


    protected virtual void Start(){
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() {
        //Système de collision
        boxCollider2D.OverlapCollider(filter2D, hits);
        for (var i = 0; i < hits.Length; i++)
        {
            if(hits[i] == null){
                continue;
            }

            //Permet de savoir qui est au contact du propriétaire du script
            //Debug.Log(collider2D.name);
            OnCollide(hits[i]);

            //On vide les collisions trouvées
            hits[i] = null;
        }
    } 


    protected virtual void OnCollide(Collider2D collider2D){
        Debug.Log("Collision pas encore implémentée sur " + collider2D.name);
    }
}
