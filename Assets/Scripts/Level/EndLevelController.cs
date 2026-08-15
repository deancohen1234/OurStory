using UnityEngine;
using TMPro;
using EasyTextEffects;


public class EndLevelController : MonoBehaviour
{
    public TextEffect NarrationEffect;
    public Animator Animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnimationComplete()
    {
        NarrationEffect.StartManualEffect("text-entry");
    }
}
