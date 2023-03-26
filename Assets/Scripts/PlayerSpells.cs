using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
   public Transform spellPosition;
   public List<Spell> spells;
   // public pour les tests, a passer en private à terme
   public List<float> lastCasts;
   public Player player;

   public int naturalManaRegenAmount;
   public float lastTickManaRegen;
   public float manaRegenCooldown;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)){
            //On check si le cooldown pour refaire un swing est revenu
            if(Time.time - lastCasts[0] > spells[0].cooldown){
                //On vérifie si le joueur a assez de mana pour lancer le sort
                if(spells[0].manaCost <= player.manaPoint){
                    Instantiate(spells[0], spellPosition.position, spellPosition.rotation);
                    player.UseMana(spells[0].manaCost);
                    lastCasts[0] = Time.time;
                }
            }       
        }

        ApplyNaturalManaRegen();

        //Debug only : rend 10 pts de mana avec le btn M
        if(Input.GetKeyDown(KeyCode.M)){
            player.manaPoint += 10;
        }
    }

    //Rend du mana au joueur si il n'est pas déjà full
    //Si le montant rendu dépasse le max, on donne le max pour éviter d'overcap
    private void ApplyNaturalManaRegen(){
        if(player.manaPoint < player.maxManaPoint){
            if(Time.time - lastTickManaRegen > manaRegenCooldown){
                lastTickManaRegen = Time.time;
                if((player.manaPoint + naturalManaRegenAmount) > player.maxManaPoint){
                    player.RegenMana(player.maxManaPoint);
                } else{
                    player.RegenMana(naturalManaRegenAmount);       
                }
            }
        }
    }
}
