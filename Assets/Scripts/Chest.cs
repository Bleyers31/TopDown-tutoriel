using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int moullaAmount = 5;


    protected override void OnCollect()
    {
        if(!collected){
            collected = true;
            //Le coffre est ramassé, on change le sprite du coffre
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.moula += moullaAmount;
        }
        
    }
}
