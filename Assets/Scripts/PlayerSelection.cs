using System;
using UnityEngine;
using CodeMonkey.Utils;

public abstract class PlayerSelection : MonoBehaviour
{
    public abstract void SelectObjectStart(Vector2 mousePosition);
    public abstract void SelectObjectEnd(Vector2 mousePosition);
}
