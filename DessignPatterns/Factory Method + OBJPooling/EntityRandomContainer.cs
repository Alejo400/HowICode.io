using System.Collections.Generic;
using UnityEngine;

public class EntityRandomContainer : EntityContainer
{
    [SerializeField] EntityFactory<ListData> _Factory;
    [SerializeField] ListData EntitiesListData;
    protected override void Initialize()
    {
        _Factory.CreateFactory(EntitiesListData, AmountEntities, EntitiesList, transform);
    }
    public void VerifyFactoryValues()
    {
        if (_Factory == null){ Debug.LogError("Factory is not assigned"); return; }
        if (EntitiesListData == null){ Debug.LogError("Entities List is not assigned"); return;}
    }
}
