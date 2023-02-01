using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FactionResourceStore
{
    [SerializeField]
    public SerializableDictionary<ResourceType, int> FactionResources { get; private set; }
    [SerializeField]
    public SerializableDictionary<ResourceType, int> FactionCaps { get; private set; }
    [SerializeField]
    public SerializableDictionary<ResourceType, bool> IsBankrupt { get; private set; }
    public FactionResourceStore(SerializableDictionary<string, ResourceType> allResourceTypes)
    {
        CreateAllPossibleResources(allResourceTypes);
    }
    /// <summary>
    /// It will look up any existing enum Resource Type names and create a dictionary of ResourceType and integers
    /// </summary>
    void CreateAllPossibleResources(SerializableDictionary<string, ResourceType> allResourceTypes)
    {
        FactionResources = new SerializableDictionary<ResourceType, int>();
        FactionCaps = new SerializableDictionary<ResourceType, int>();
        IsBankrupt = new SerializableDictionary<ResourceType, bool>();
        foreach (KeyValuePair<string,ResourceType> pair in allResourceTypes)
        {
            FactionResources.Add(pair.Value, 0);
            FactionCaps.Add(pair.Value, 0);
            IsBankrupt.Add(pair.Value, false);
        }
    }
    public void AddResource(Resource resource)
    {
        FactionResources[resource.resourceType] += resource.amount;
        if (WillAddedResourceBePastMaxAmount(resource))
        {
            if (resource.resourceType.havePenaltyIfPastMaxCap)
            {
                IsBankrupt[resource.resourceType] = true;
            }
            if(!resource.resourceType.canBePastMaxCap)
            {
                FactionResources[resource.resourceType] = FactionCaps[resource.resourceType];
            }
        }
        if (WillAddedResourceBePastMinAmount(resource))
        {
            if (resource.resourceType.havePenaltyIfPastMinCap)
            {
                IsBankrupt[resource.resourceType] = true;
            }
            if (!resource.resourceType.canBePastMinCap)
            {
                FactionResources[resource.resourceType] = 0;
            }
        }
    }
    public void AddResource(List<Resource> resources)
    {
        foreach (Resource resource in resources)
            AddResource(resource);
    }


    public void AddOnlyCapResource(Resource resource)
    {
        if (!resource.resourceType.isDesignedToBeCapResource)
            return;
        AddResource(resource);
    }
    public void AddOnlyCapResource(List<Resource> resources)
    {
        foreach (Resource resource in resources)
            AddOnlyCapResource(resource);
    }
    public void ResetCapResources()
    {
        for (int i = 0; i < FactionResources.Count; i++)
        {
            if (FactionResources.ElementAt(i).Key.isDesignedToBeCapResource)
            {
                FactionResources[FactionResources.ElementAt(i).Key] = 0;
            }
        }
    }


    public void AddCap(Resource resource)
    {
        FactionCaps[resource.resourceType] += resource.amount;
    }
    public void AddCap(List<Resource> resources)
    {
        foreach (Resource resource in resources)
            AddCap(resource);
    }
    public void ResetCaps(SerializableDictionary<ResourceType, int> factionInitialCaps)
    {
        for (int i = 0; i < FactionResources.Count; i++)
        {
            if (factionInitialCaps.ContainsKey(FactionCaps.ElementAt(i).Key))
                FactionCaps[FactionCaps.ElementAt(i).Key] = factionInitialCaps[FactionCaps.ElementAt(i).Key];
            else
                FactionCaps[FactionCaps.ElementAt(i).Key] = 0;
        }
    }



    public bool IsResourceAddPossible(Resource resource)
    {
        if (WillAddedResourceBePastMaxAmount(resource) || WillAddedResourceBePastMinAmount(resource))
            return false;
        return true;
    }
    public bool IsResourceAddPossible(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            if (!IsResourceAddPossible(resource))
                return false;
        }
        return true;
    }
    public bool WillAddedResourceBePastMaxAmount(Resource resource)
    {
        if (FactionResources[resource.resourceType] > FactionCaps[resource.resourceType])
            return true;
        return false;
    }
    public bool WillAddedResourceBePastMinAmount(Resource resource)
    {
        if (FactionResources[resource.resourceType] < 0)
            return true;
        return false;
    }

    public string GetResourceInfo()
    {
        string s = "";
        foreach (KeyValuePair<ResourceType, int> resourceInfo in FactionResources)
        {
            s += resourceInfo.Value + " " + resourceInfo.Key.name + "; ";
        }
        return s;
    }
}
