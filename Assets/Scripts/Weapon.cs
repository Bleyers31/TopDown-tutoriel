using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Structure des dégats
    public int[] damagePoint = {2,4,6,8,10,12,14,16,18,20};
    public float[] pushForce = {2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 4.5f, 5.0f, 5.5f, 6.0f, 6.5f};

    //Upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    //Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        //On garde le contenu du Start et on y ajoute la récupération du SpriteRenderer
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space)){
            //On check si le cooldown pour refaire un swing est revenu
            if(Time.time - lastSwing > cooldown){
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    private void Swing(){
        //On lance l'animation du Swing via son trigger dans le menu d'animation
        anim.SetTrigger("Swing");
    }

    //On redéfinit le comportement d'une collision dans le cas où c'est l'arme qui touche quelque chose
    protected override void OnCollide(Collider2D collider2D)
    {
        if(collider2D.tag == "Fighter"){
            if(collider2D.name != "Player"){

                //On crée un nouvel objet de dégats que l'on va envoyer à celui qui est rentré en collision avec l'arme 
                Damage dmg = new Damage(){
                    damageAmount = damagePoint[weaponLevel],
                    origin = transform.position,
                    pushForce = pushForce[weaponLevel]
                };

                //On envoie l'objet de dégats vers celui qui va les recevoir
                collider2D.SendMessage("ReceiveDamage", dmg);

                Debug.Log("Collision avec " + collider2D.name);
            }
        }
    }

    //On améliore l'arme -> on augente de 1 son niveau et change le sprite en conséquence
    public void UpgradeWeapon(){
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    //Attribue un niveau de l'arme et actualise le sprite dans les mains du joueur
    public void SetWeaponLevel(int level){
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
