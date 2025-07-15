using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MoveOptionHolder : MonoBehaviour
{
    private Animation anim;
    public string[] clips;
    public int jankCallQTB;
    public CombatManager combatManager;
    private void OnEnable()
    {
        anim = gameObject.GetComponent<Animation>();
        int toPlay = Random.Range(0, clips.Length);
        string clip = clips[toPlay];
        anim.Play(clip);
    }

    private void OnDisable()
    {
        combatManager.StartPointerTargeting(jankCallQTB); 
    }

}
