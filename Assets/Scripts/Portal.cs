using UnityEngine;

public class Portal : Collidable
{
public string[] sceneNames;

    protected override void OnCollide(Collider2D collider2D)
    {
        if(collider2D.name == "Player"){

            //On sauvegarde le statut du joueur
            GameManager.instance.SaveState();

            //Téléporte le joueur vers une scène aléatoire parmis la liste
            //Si on veut aller sur une scène précise, passer uniquement la scène dans la liste
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];

            Debug.Log("chargement de la scène : " + sceneName);

            //Charge la nouvelle scène
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
