using System.Collections.Generic;
using UnityEngine;

public abstract class EntityFactory<T> : MonoBehaviour
{
    [SerializeField] protected ObjectPool _ObjectPool;
    public abstract void CreateFactory(T entity, int amountEntities, 
                                        List<GameObject> EntitiesList, Transform container);
}
