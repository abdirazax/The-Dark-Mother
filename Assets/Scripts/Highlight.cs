using UnityEngine;

public class Highlight:MonoBehaviour
{
    public void Follow(Transform target, float scaleHighlight)
    {
        transform.SetParent(target);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * scaleHighlight;
        gameObject.SetActive(true);
    }
    public void Remove()
    {
        gameObject.SetActive(false);
    }
}
