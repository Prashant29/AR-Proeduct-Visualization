using DissolveExample;
using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public GameObject trainer, canvas;

    public GameObject placedObject;

    public bool isObjectPlaced;

    public Animator animator;

    // Add the names of your animations here in the order you want to cycle through
    public string[] animationNames;

    private int currentAnimationIndex = 0;
    private bool isReversed = false;

    [SerializeField]
    public bool ObjectPlace
    {
        get { return isObjectPlaced; }
        set
        {
            isObjectPlaced = value;
            trainer.SetActive(value);
            canvas.SetActive(value);
        }
    }

    private void Start()
    {
        TextToSpeech.Instance.StartSpeak("Welcome to Coffee Machine Training Tutorial! In this tutorial We will learn about Coffee Machine");

        animator = placedObject.GetComponent<Animator>();

        Debug.Log("Animation length = "+animationNames.Length);
        for (int i = 0; i < animationNames.Length; i++)
        {
            Debug.Log("Animation "+i+ " : "+animationNames[i]);            
        }
    }

    public void EnableTrainer(Button btn)
    {
        if (btn.GetComponentInChildren<TMPro.TMP_Text>().text.Contains("Hide"))
        {
            trainer.GetComponent<DissolveChilds>().Disappear();
            btn.GetComponentInChildren<TMPro.TMP_Text>().text = "Show";
        }
        else
        {

            trainer.GetComponent<DissolveChilds>().Appear();
            btn.GetComponentInChildren<TMPro.TMP_Text>().text = "Hide";

        }

    }

    private void Update()
    {
        if (Input.touchCount == 2 && isObjectPlaced)
        {
            ScaleUpDownObject();
        }
    }

    float initialDistance;
    Vector3 initialScale;
    void ScaleUpDownObject()
    {
        var touchZero = Input.GetTouch(0);
        var touchOne = Input.GetTouch(1);

        if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled) return;

        if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
        {
            initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
            initialScale = placedObject.transform.localScale;

        }
        else
        {
            var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
            if (Mathf.Approximately(initialDistance, 0))
            {
                return;
            }

            var factor = currentDistance / initialDistance;
            placedObject.transform.localScale = initialScale * factor;
        }

    }

    public void PlayNextAnimation()
    {
        isReversed = false;
        if(currentAnimationIndex != animationNames.Length-1){
            currentAnimationIndex ++;
            // currentAnimationIndex = (currentAnimationIndex + 1) % animationNames.Length;
            PlayAnimation(animationNames[currentAnimationIndex]);
        }
    }

    public void PlayPreviousAnimation()
    {
        isReversed = true;
        if(currentAnimationIndex != 0){
            // currentAnimationIndex = (currentAnimationIndex - 1 + animationNames.Length) % animationNames.Length;
            PlayAnimation(animationNames[currentAnimationIndex]);
            currentAnimationIndex --;
        }
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(isReversed && currentAnimationIndex!=0 ? animationName + "Reverse" : animationName);
    }
}
