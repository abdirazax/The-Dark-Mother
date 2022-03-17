using UnityEngine;

public abstract class SelectableInSkirmish : MonoBehaviour
{
    public virtual void Select()
    {
        Debug.Log("This is Selectable Object In Skirmish");
    }
}

