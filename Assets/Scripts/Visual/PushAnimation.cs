using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushAnimation", menuName = "Piekoszek/Animation/Push")]
public class PushAnimation : Animation
{
    [SerializeField] private float depth = 1;

    private Dictionary<GameObject, PushAnimationData> animationDataByGameObject = new();

    public override void StartAnimation(GameObject gameObject)
    {
        if (animationDataByGameObject.ContainsKey(gameObject))
        {
            var previousAnimation = animationDataByGameObject[gameObject];
            GetEmpty(gameObject).StopCoroutine(previousAnimation.runningAnimation);
            SetInitialPosition(gameObject);
        }

        var initialPosition = gameObject.transform.position;
        var targetPosition = initialPosition - new Vector3(0, 1, 0) * depth;
        var runningAnimation = GetEmpty(gameObject).StartCoroutine(Animation(gameObject));
        animationDataByGameObject[gameObject] =
            new PushAnimationData(initialPosition, targetPosition, runningAnimation);
    }

    private IEnumerator Animation(GameObject gameObject)
    {
        yield return null;

        var initialPosition = animationDataByGameObject[gameObject].initialPosition;
        var targetPosition = animationDataByGameObject[gameObject].targetPosition;

        for (float step = 0; step < 1; step += 10 * Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, step);
            yield return null;
        }


        for (float step = 0; step < 1; step += 10 * Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(targetPosition, initialPosition, step);
            yield return null;
        }

        SetInitialPosition(gameObject);
    }

    public override void StopAnimation(GameObject gameObject)
    {
        GetEmpty(gameObject).StopCoroutine(animationDataByGameObject[gameObject].runningAnimation);
        SetInitialPosition(gameObject);
    }

    private Empty GetEmpty(GameObject gameObject)
    {
        var empty = gameObject.GetComponent<Empty>();

        if (empty == null)
        {
            empty = gameObject.AddComponent<Empty>();
        }

        return empty;
    }

    private void SetInitialPosition(GameObject gameObject)
    {
        gameObject.transform.position = animationDataByGameObject[gameObject].initialPosition;
    }
}

public class PushAnimationData
{
    public Vector3 initialPosition;
    public Vector3 targetPosition;
    public Coroutine runningAnimation;

    public PushAnimationData(Vector3 initialPosition, Vector3 targetPosition, Coroutine runningAnimation)
    {
        this.initialPosition = initialPosition;
        this.targetPosition = targetPosition;
        this.runningAnimation = runningAnimation;
    }
}