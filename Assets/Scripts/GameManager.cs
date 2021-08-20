using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Permet de rendre ce GameManager accessible depuis nimporte quel autre script 
    //Pour l'appeller, faire GameManager.instance
    public static GameManager instance;

    private void Awake(){
        //Si il y a déjà une instance de GameManager, on détruit les autres références car cela
        //veut dire qu'elles sont déjà présentes et que ça les duplierait.
        //C'est du au DontDestroyOnLoad, qui cherche à les faire suivre d'une scène à l'autre
        if(GameManager.instance != null){
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        //Si on souhaite supprimer les données sauvegardées : PlayerPrefs.DeleteAll();
        instance = this;

        //On ne passe pas les paramètres des fonctions car le SceneManager 
        //retrouve tous les paramètres et fait les liens automatiquement
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    //Référence à la barre de points de vie pour gérer l'affichage
    public RectTransform hitPointBar;
    public GameObject hud;
    public GameObject menu;

    //Références
    public Player player;
    public Weapon weapon;

    //Boite de dialogue
    public FloatingTextManager floatingTextManager;

    //Logic -> les attributs du joueur
    public int moula;
    public int experience;


    public void ShowText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration){
        floatingTextManager.Show(message, fontSize, color, position, motion, duration);
    }


    //Gestion de l'affichage de la barre d'hp et de sa mise à jour
    public void OnHitPointChange(){
        //On récupère le ratio entre hp courants et hp max
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;

        //On applique le ratio (0 à 1) pour savoir quel % de la barre doit être rempli
        hitPointBar.localScale = new Vector3(1, ratio, 1);
    }


    //On upgrade l'arme si les conditions sont remplies
    public bool TryUpgradeWeapon(){
        //Est-ce que l'arme est à son niveau maximum?
        if(weaponPrices.Count <= weapon.weaponLevel){
            return false;
        }

        //Est-ce qu'on a assez de moula pour améliorer l'arme?
        if(moula >= weaponPrices[weapon.weaponLevel]){
            //On déduit la quantitée de moula demandée et upgrade l'arme
            moula -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }


    //Récupération du level actuel du joueur
    public int GetCurrentLevel(){
        int level = 0;
        int experienceRequiredNextLevel = 0;

        while(experience >= experienceRequiredNextLevel){
            //on ajoute l'xp necessaire pour le prochain niveau
            experienceRequiredNextLevel += xpTable[level];
            //on ajoute 1 au niveau du joueur
            level++;
        
            //Est-ce que l'on est au niveau max?
            if(level == xpTable.Count){
                return level;
            }
        }

        return level;
    }


    //Retourne l'xp necessaire pour atteindre le niveau en paramètre
    public int GetXpToLevel(int levelAsked){
        int level = 0;
        int xp = 0;

        while(level < levelAsked){
            xp += xpTable[level];
            level++;
        }

        return xp;
    }
    
    //Quand on gagne de l'xp, on vérifie si on a level up et en applique les effets si c'est le cas 
    public void GrantXp(int xp){
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if(currentLevel < GetCurrentLevel()){
            OnLevelUp();
        }
    }

    public void OnLevelUp(){
        Debug.Log("LEVEL UP");
        player.OnLevelUp();
        OnHitPointChange();
    }

    //Fonctions de sauvegarde
    /*
    * INT preferedSkin
    * INT moula
    * INT experience
    * INT weaponLevel
    */
    public void SaveState(){
        string s = "";

        s += "0" + "|";
        s += moula.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode){

        //Permet de s'assurer que l'on ne charge qu'une fois une même scène si l'on y revient
        //plusieurs fois. Sinon les valeurs (moulla, xp, ...) s'additionnent
        SceneManager.sceneLoaded -= LoadState;

        //Si aucune sauvegarde trouvée, pas la peine de charger les données
        if(!PlayerPrefs.HasKey("SaveState")){
            return;
        }
            
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //attribue le skin choisi -> todo

        //attribue la moula sauvegardée
        moula = int.Parse(data[1]);

        //attribue l'xp sauvegardée
        experience = int.Parse(data[2]);

        //attribue le niveau du joueur et applique les bonus de niveau en fonction de l'xp.
        //Si niveau 1, on ne fait rien pour ne pas donner le bonus de level up (niveau 0 -> 1)
        if(GetCurrentLevel() != 1){
            player.SetLevel(GetCurrentLevel());
        }

        //attribue l'arme sauvegardée
        weapon.SetWeaponLevel(int.Parse(data[3]));
    }


    public void OnSceneLoaded(Scene s, LoadSceneMode mode){
        //On téléporte le joueur au point de spawn de la carte
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
