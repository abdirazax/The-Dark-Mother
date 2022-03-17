using System;
using UnityEngine;

public class PlayerSelectionGlobalMap : PlayerSelection
{
    [SerializeField]
    private string selectableInWorldMapTag = "SelectableInWorldMap";
    [SerializeField]
    private Transform selectionCircle;

    GameObject selectedGameObject;

    public override void SelectObjectStart(Vector2 mousePosition)
    {
        //Debug.Log("Mouse start " + mousePosition);
    }
    public override void SelectObjectEnd(Vector2 mousePosition)
    {
        Debug.Log("Mouse end " + mousePosition + " Object: " + selectedGameObject);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(selectableInWorldMapTag))
            {
                SelectableInWorldMap selection = hit.transform.GetComponentInParent<SelectableInWorldMap>();
                selection.Select();
            }
        }
        //selectionCircle = selectedGameObject.transform;
    }
}