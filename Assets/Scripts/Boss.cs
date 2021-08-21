using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //2 boules de feu gravitent autour du boss; chacune dans un sens horaire différent
    public float[] fireballSpeed = {2.5f, -2.5f};
    public float distance = 0.25f;
    public Transform[] fireballs;

    private void Update() {

        for (var i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(
                -Mathf.Cos(Time.time * fireballSpeed[i]) * distance, //Axe x
                Mathf.Sin(Time.time * fireballSpeed[i]) * distance,  //Axe y
                0);                                                  //Axe z
        }

                                                     
    }
}
