using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BoardSpawner : EventListener
{
    private ARTrackedImageManager manager;
    [SerializeField] private GameObject board;

    private bool firstTime = true;

    private void Awake()
    {
        manager = GetComponent<ARTrackedImageManager>();
        manager.trackedImagesChanged += OnChanged;
        board.transform.position = new Vector3(-01231231312, -231321312, -12313123131);
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        ARTrackedImage img = null;
        foreach (var newImage in eventArgs.added)
        {
            img = newImage;
            // Handle added event
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            img = updatedImage;
        }

        if (img == null)
        {
            return;
        }

        if (firstTime)
        {
            EventSystem.Instance.Fire(EventName.ARSpawn, new EventData());
            firstTime = false;
        }

        board.transform.position = img.transform.position;
        board.transform.rotation = img.transform.rotation;
    }
}