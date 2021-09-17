using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
   public Transform spellPosition;
   public GameObject spell;

   private void Update() {
        if(Input.GetKeyDown(KeyCode.A)){
            Instantiate(spell, spellPosition.position, spellPosition.rotation);
        }
   }
}
