using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TiltAnimation", menuName = "Piekoszek/Animation/Tilt")]
public class TiltAnimation : Animation
{
    [SerializeField] private float tilt = 0;

    private Dictionary<GameObject, TiltAnimationData> animationDataByGameObject = new();

    public override void StartAnimation(GameObject gameObject)
    {
        if (animationDataByGameObject.ContainsKey(gameObject))
        {
            var previousAnimation = animationDataByGameObject[gameObject];
            GetEmpty(gameObject).StopCoroutine(previousAnimation.runningAnimation);
        }

        var runningAnimation = GetEmpty(gameObject).StartCoroutine(Animation(gameObject, tilt));
        animationDataByGameObject[gameObject] =
            new TiltAnimationData(runningAnimation);
    }

    private IEnumerator Animation(GameObject gameObject, float targetTilt)
    {
        yield return null;


        var rotation = gameObject.transform.localRotation.eulerAngles;
        var initialTilt = gameObject.transform.localRotation.eulerAngles.x;

        for (float step = 0; step < 1; step += 10 * Time.deltaTime)
        {
            rotation.x = Mathf.Lerp(initialTilt, targetTilt, step);
            gameObject.transform.localRotation = Quaternion.Euler(rotation);
            yield return null;
        }
        
    }

    public override void StopAnimation(GameObject gameObject)
    {
        GetEmpty(gameObject).StopCoroutine(animationDataByGameObject[gameObject].runningAnimation);
    }
    
}

public class TiltAnimationData
{
    public Coroutine runningAnimation;

    public TiltAnimationData(Coroutine runningAnimation)
    {
        this.runningAnimation = runningAnimation;
    }
}