using System.Collections.Generic;
using UnityEngine;

public class EntitiesRandomFactory : EntityFactory<ListData>
{
    public override void CreateFactory(ListData EntitiesData, int amountEntities, 
                                        List<GameObject> ListToContainer, Transform _Spawn)
    {
        List<GameObject> Entities = EntitiesData.ReturnData();
        List<GameObject> ListTmp = new List<GameObject>();

        if(Entities == null){
            throw new KeyNotFoundException($"Enemies list not found in factory.");
        }else
        {
            
            //Create a list of random enemies based on the scriptable object EnemiesListData
            for (int i = 0; i < amountEntities; i++)
            {
                ListTmp.Add(Entities[Random.Range(0,Entities.Count)]);
            }
            _ObjectPool.CreatePoolFromList(ListTmp,ListToContainer,_Spawn);
        }
    }
}
