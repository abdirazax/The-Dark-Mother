using UnityEngine;

public abstract class SelectableInWorldMap : MonoBehaviour
{
    public virtual void Select()
    {
        Debug.Log("This is Selectable Object In World Map");
    }
}

