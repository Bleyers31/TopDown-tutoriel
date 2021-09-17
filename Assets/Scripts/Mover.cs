using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract : ne peut pas être utilisé tel quel -> Il faut qu'une classe fille en hérite
public abstract class Mover : Fighter
{
    private Vector3 originalSize;

    protected BoxCollider2D boxCollider2D;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;


    protected virtual void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();    
        originalSize = transform.localScale;
    }

    //Fonction qui gère tous les déplacements
    protected virtual void UpdateMotor(Vector3 input){
        //On applique la vitesse sur la direction où se déplace le component fille
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Changer la position du sprite selon la direction où on se déplace
        if(moveDelta.x > 0){
            //Vector3.one équivaut à new Vector3(1, 1, 1)
            transform.localScale = originalSize;
            //On sauvegarde de quel côté regarde le joueur
            if(gameObject.name == "Player"){
                GameManager.instance.player.lookAt = "right";
                GameManager.instance.player.anim.SetBool("isMoving", true);
            }
        }else if(moveDelta.x < 0){
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
            if(gameObject.name == "Player"){
                GameManager.instance.player.lookAt = "left";
                GameManager.instance.player.anim.SetBool("isMoving", true);
            }
        }

        if(moveDelta.x == 0){
            GameManager.instance.player.anim.SetBool("isMoving", false);
        }

        //On ajoute une force (si il y en a une) afin de pousser l'entité dans une direction
        //via l'attribut pushDirection héritée de Fighter
        moveDelta += pushDirection;

        //On réduit la force de poussée chaque frame jusqu'a ce que la poussée soit terminée (sinon poussée infinie)
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);


        //La vérif sur hit permet de vérifier si le joueur peut se déplacer dans la direction souhaitée. 
        //Si un élément avec un des masques listés à la fin de la fonction est présent, on bloque le mouvement

        //Vérification sur l'axe Y
        hit = Physics2D.BoxCast(transform.position, boxCollider2D.size, 0, new Vector2(0, moveDelta.y), 
                Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null){
            //Gestion du déplacement
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //Vérification sur l'axe X
        hit = Physics2D.BoxCast(transform.position, boxCollider2D.size, 0, new Vector2(moveDelta.x, 0), 
                Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null){
            //Gestion du déplacement
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
