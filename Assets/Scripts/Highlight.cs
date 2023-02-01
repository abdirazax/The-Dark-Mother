using UnityEngine;

public class Highlight:MonoBehaviour
{
    [SerializeField]
    Transform initialParent;
    Transform followObject;
    HighlightStateAbstract currentHighlightState;
    HighlightStateDisabled highlightStateDisabled = new HighlightStateDisabled();
    HighlightStateFollowing highlightStateFollowing = new HighlightStateFollowing();
    private void Awake()
    {
        currentHighlightState = highlightStateDisabled;
    }
    private void OnEnable()
    {
        GlobalMapSpawnManager.OnStartedDestroyingGameObjects += Disable;
    }
    private void OnDisable()
    {
        GlobalMapSpawnManager.OnStartedDestroyingGameObjects -= Disable;
    }
    private void LateUpdate()
    {
        currentHighlightState.LateUpdate(this);
    }
    public void Follow(Transform target, float scaleHighlight)
    {
        followObject = target;
        transform.localScale = Vector3.one * scaleHighlight;
        currentHighlightState = highlightStateFollowing;
        transform.position = followObject.position;
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
        currentHighlightState = highlightStateDisabled;
    }
    public class HighlightStateFollowing : HighlightStateAbstract
    {
        public override void LateUpdate(Highlight highlight)
        {
            highlight.transform.position = highlight.followObject.position;
        }
    }
    public class HighlightStateDisabled : HighlightStateAbstract
    {
        public override void LateUpdate(Highlight highlight) { }
    }
    public abstract class HighlightStateAbstract
    {
        public abstract void LateUpdate(Highlight highlight);
    }

}
