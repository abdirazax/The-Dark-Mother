using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Resource
{
    public int amount = 0;
    public ResourceType resourceType;
    public Resource(ResourceType resourceType)
    {
        this.resourceType = resourceType;
    }
    public Resource(int amount, ResourceType resourceType)
    {
        this.amount = amount;
        this.resourceType = resourceType;
    }
    public Resource (KeyValuePair<ResourceType,int> resourcePair)
    {
        amount = resourcePair.Value;
        resourceType = resourcePair.Key;
    }
    public void AddResource(Resource addedResource)
    {
        if (addedResource.resourceType != resourceType) return;
        amount += addedResource.amount;
    }

}
