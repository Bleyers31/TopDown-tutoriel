using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Champs de texte mis à jour
    public Text levelText;
    public Text hitpointText;
    public Text moulaText;
    public Text upgradeCostText;
    public Text xpText;


    //Logique
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;


    //Sélection du personnage. True si droite, false si gauche
    public void OnArrowClick(bool right){
        if(right){
            //On passe au personnage suivant
            currentCharacterSelection++;

            //Limite atteinte -> on repart à 0
            if(currentCharacterSelection == GameManager.instance.playerSprites.Count){
                currentCharacterSelection = 0;
            }
        }else{
             //On passe au personnage précédent
            currentCharacterSelection--;

            //On est à 0 -> on repart au dernier existant
            if(currentCharacterSelection < 0){
                //-1 car on commence à 0
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }
        }

        OnSelectionChanged();
    }


    //On met à jour le sprite du joueur dans le menu et en jeu
    private void OnSelectionChanged(){
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }
    

    //On clique sur le bouton pour améliorer l'arme
    //Si toutes les conditions sont remplies, upgrade et maj de l'interface avec nouvelle arme
    public void OnUpgradeClick(){
        if(GameManager.instance.TryUpgradeWeapon()){
            UpdateMenu();
        }
    }


    //Update des informations affichées du joueur
    public void UpdateMenu(){
        //Arme
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if(GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count){
            upgradeCostText.text = "MAX";
        }else{
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }
       
        //Textes
        hitpointText.text = GameManager.instance.player.hitPoint.ToString() +  " / " + GameManager.instance.player.maxHitPoint.ToString();
        moulaText.text = GameManager.instance.moula.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //Barre d'xp -> affichage différent si level max
        int currentLevel = GameManager.instance.GetCurrentLevel();

        if(currentLevel == GameManager.instance.xpTable.Count){
            xpText.text = GameManager.instance.experience.ToString() + " xp";
            xpBar.localScale = Vector3.one; //rempli à 100%
        }else{
            //On récupère l'xp du level précédent et suivant afin de faire un ration pour remplir la barre en %
            int previousLevelXp = GameManager.instance.GetXpToLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.GetXpToLevel(currentLevel);

            int diff = currentLevelXp - previousLevelXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLevelXp;

            float completitionRatio = (float)currentXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completitionRatio, 1, 1);
            xpText.text = currentXpIntoLevel.ToString() + " / " + diff;
        }
    }
}
