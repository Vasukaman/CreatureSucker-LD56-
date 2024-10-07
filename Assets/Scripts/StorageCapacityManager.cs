using System.Collections.Generic;
using UnityEngine;

public class StorageCapacityManager : MonoBehaviour
{
    [SerializeField]
    private int maxVacuumCapacity = 25;
    [SerializeField]
    private int maxFurnaceCapacity = 50;

    private List<GameObject> vacuumStoredCreatures = new List<GameObject>();
    private List<GameObject> furnaceStoredCreatures = new List<GameObject>();

    public bool CanStoreInVacuum() => vacuumStoredCreatures.Count < maxVacuumCapacity;

    public bool CanStoreInFurnace() => furnaceStoredCreatures.Count < maxFurnaceCapacity;

    public void StoreInVacuum(GameObject creature)
    {
        if (CanStoreInVacuum())
            vacuumStoredCreatures.Add(creature);
    }

    public void StoreInFurnace(GameObject creature)
    {
        if (CanStoreInFurnace())
            furnaceStoredCreatures.Add(creature);
    }

    public List<GameObject> GetVacuumCreatures() => new List<GameObject>(vacuumStoredCreatures);

    public List<GameObject> GetFurnaceCreatures() => new List<GameObject>(furnaceStoredCreatures);

    public void ClearVacuum() => vacuumStoredCreatures.Clear();

    public void ClearFurnace() => furnaceStoredCreatures.Clear();
}
