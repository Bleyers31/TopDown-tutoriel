using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update() {
        foreach(FloatingText txt in floatingTexts){
            txt.UpdateFloatingText();
        }
    }

    private FloatingText GetFloatingText(){
        //Retourne le texte dans la liste qui n'est pas actif
        //Equivalent d'un foreach sur la liste où on remonte le txt si active = false
        FloatingText txt = floatingTexts.Find(t => t.active);

        //Si le texte n'est pas actif, on l'instancie et prépare l'affichage
        if(txt == null){
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration){
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;
        //On se base sur la position de la Main Camera pour définir où placer le texte (sinon caméra globale et pb de scalling)
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();

    }


}
