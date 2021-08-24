using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Le joueur peut se déplacer et est un combattant -> il peut infliger, recevoir des dégats et mourir
public class Player : Mover 
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //On met à jour le ratio de la barre d'hp du joueur après la perte d'hp
    protected override void ReceiveDamage(Damage dmg)
    {
        if(!isAlive){
            return;
        }
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitPointChange();
    }

    //Gestion de la mort du joueur
    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
    }

    //Permet au joueur de se déplacer. La classe Mover prend tout en charge
    private void FixedUpdate() {
        //Renvoie -1 ou 1 en fonction de si on va a gauche ou à droite
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //Si le joueur est mort, bloque ses mouvements
        if(isAlive){
            UpdateMotor(new Vector3(x, y, 0));
        }
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

    public void Heal(int healingAmount){

        if(hitPoint == maxHitPoint){
            return;
        }else{
            hitPoint += healingAmount;
            if(hitPoint > maxHitPoint){
                hitPoint = maxHitPoint;
            }
        }

        GameManager.instance.ShowText(healingAmount.ToString(), 30, Color.green, transform.position, Vector3.up * 40, 1.0f);

        //Mise à jour du ratio de la barre de points de vie après le soin
        GameManager.instance.OnHitPointChange();
    }
}
