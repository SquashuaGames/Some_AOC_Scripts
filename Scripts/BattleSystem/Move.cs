using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : ScriptableObject
{
    public string firstText;
    public string secondText;
    public string altText;
    public int setDamage;
    public int power = 1, hitAdjustment = 0;
    public GameObject combatFX;
    public bool specialFX;
    public GameObject[] bonusFX;
    public bool needsAccuracy = true;

    public AnimationClip moveClip;
    public int[] spriteIndices;
}
