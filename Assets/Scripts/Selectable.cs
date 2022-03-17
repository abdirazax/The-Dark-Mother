using UnityEngine;
public abstract class Selectable : MonoBehaviour
{
    public virtual void Select()
    {
        Debug.Log("This is Selectable Object");
    }
}

