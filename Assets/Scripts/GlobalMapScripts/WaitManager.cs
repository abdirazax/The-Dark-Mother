/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitManager : MonoBehaviour
{
    
    [SerializeField]
    SerializableDictionary<ISendProcessesToWaitForEvents, int> processesToWaitFor = new SerializableDictionary<ISendProcessesToWaitForEvents, int>();

    private void OnEnable()
    {
        Navigates.OnProcessToWaitForEventAdded += AddProcessToWaitList;
        Navigates.OnProcessToWaitForEventRemoved += RemoveProcessFromWaitList;
    }
    private void OnDisable()
    {
        Navigates.OnProcessToWaitForEventAdded -= AddProcessToWaitList;
        Navigates.OnProcessToWaitForEventRemoved -= RemoveProcessFromWaitList;
    }
    public bool IsThereNoProcessToWaitFor { get { return processesToWaitFor.Count == 0; } }

    public void AddProcessToWaitList(ISendProcessesToWaitForEvents eventSender)
    {
        if (!processesToWaitFor.ContainsKey(eventSender))
        {
            processesToWaitFor.Add(eventSender, 1);
            Debug.Log("Start " + DebugProcessesToWaitFor());
            return;
        }
        processesToWaitFor[eventSender]++;
        //Debug.Log("Start " + DebugProcessesToWaitFor());
    }
    public void RemoveProcessFromWaitList(ISendProcessesToWaitForEvents eventSender)
    {
        if (!processesToWaitFor.ContainsKey(eventSender))
        {
            Debug.Log("No Such Sender");
            return;
        }
        processesToWaitFor[eventSender]--;
        if (processesToWaitFor[eventSender] == 0)
            processesToWaitFor.Remove(eventSender);
        //Debug.Log("Fin " + DebugProcessesToWaitFor());
    }
    string  DebugProcessesToWaitFor()
    {
        string s = "";
        foreach(KeyValuePair<ISendProcessesToWaitForEvents, int> pair in processesToWaitFor)
        {
            s += pair+"; ";
        }
        return s;
    }
}


public interface ISendProcessesToWaitForEvents
{
    public static event Action<ISendProcessesToWaitForEvents> OnProcessToWaitForEventAdded;
    public static event Action<ISendProcessesToWaitForEvents> OnProcessToWaitForEventRemoved;
}*/