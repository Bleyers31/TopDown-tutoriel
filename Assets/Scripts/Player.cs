using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Le joueur peut se déplacer et est un combattant -> il peut infliger, recevoir des dégats et mourir
public class Player : Mover 
{
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Si on change de scène, le joueur persiste afin que toutes ses références reste associés au GameManager
        //Comme l'arme est un enfant du Player (dans la hierarchie des objets Unity), elle ne sera pas détruite non plus
        DontDestroyOnLoad(gameObject);
    }

    //Permet au joueur de se déplacer. La classe Mover prend tout en charge
    private void FixedUpdate() {
        //Renvoie -1 ou 1 en fonction de si on va a gauche ou à droite
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinId){
       spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    //Quand on level up, augmente hp max et remet full life
    public void OnLevelUp(){
        maxHitPoint++;
        hitPoint = maxHitPoint;
    }

    //Applique le niveau en paramètre au joueur
    public void SetLevel(int level){
        for (var i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }
}
