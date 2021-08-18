using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; // Position du joueur
    public float boundX = 0.15f; // Définition de la zone (axe X) où la caméra est fixe avant qu'elle ne suive le joueur
    public float boundY = 0.05f; // Définition de la zone (axe Y) où la caméra est fixe avant qu'elle ne suive le joueur


    private void Start() {
        lookAt = GameObject.Find("Player").transform;
    }


    //LateUpdate est appelé après tous les autes Update -> la caméra doit être calculée en dernier après les mouvements
    private void LateUpdate() {
        Vector3 delta = Vector3.zero;

        //On regarde si le joueur est toujours dans la zone de la caméra fixe
        //Vérifiation sur l'axe X
        float deltaX = lookAt.position.x - transform.position.x;
        if(deltaX > boundX || deltaX < -boundX){
            if(transform.position.x < lookAt.position.x){
                delta.x = deltaX - boundX;
            }else{
                delta.x = deltaX + boundX;
            }
        }

        //Vérifiation sur l'axe Y
        float deltaY = lookAt.position.y - transform.position.y;
        if(deltaY > boundY || deltaY < -boundY){
            if(transform.position.y < lookAt.position.y){
                delta.y = deltaY - boundY;
            }else{
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
