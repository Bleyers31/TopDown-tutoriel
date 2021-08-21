using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collidable
{
    public string message;
    private float cooldownText = 4.0f;
    private float lastText = -4.0f; //on initialise pour pas devoir attendre 4 secondes avant affichage du 1er message

    protected override void OnCollide(Collider2D collider2D)
    {
        Debug.Log("Collision avec NPC");
        //On vérifie si le dernier message affiché n'est pas trop récent
        if(Time.time - lastText > cooldownText){
            lastText = Time.time;
            //On affiche le message au dessus du npc (+16 pixels)
            GameManager.instance.ShowText(message, 18, Color.white, transform.position + new Vector3(0,0.16f,0), Vector3.zero, cooldownText);
        }
    }
}
