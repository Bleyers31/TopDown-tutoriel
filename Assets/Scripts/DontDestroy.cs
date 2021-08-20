using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ce script est à attribuer à tous les objets que l'on veut faire suivre d'une scène à l'autre
//Il permet d'éviter de faire des OnDestroyOnLoad dans chaque script et d'en avoir partout à la fin
public class DontDestroy : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
