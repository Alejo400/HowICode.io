using System.Collections.Generic;
using UnityEngine;

public class EntityIDFactory : EntityFactory<string>
{
    [SerializeField] EntitiesData entitiesData;
    void Awake(){
        entitiesData.LoadEntities();
    }
    public override void CreateFactory(string IDName, int amountEntities, 
                                        List<GameObject> EntitiesList, Transform container)
    {
        if(!entitiesData.EntitiesDictionary.ContainsKey(IDName)){
            throw new KeyNotFoundException($"Entity ID '{IDName}' not found in factory.");
        }else
        {
            _ObjectPool.createObjectList(entitiesData.EntitiesDictionary[IDName],
            amountEntities,EntitiesList,container);
        }
    }
}
