using System.Collections.Generic;
using UnityEngine;

public class StorageCapacityManager : MonoBehaviour
{
    [SerializeField]
    private int maxVacuumCapacity = 25;
    [SerializeField]
    private int maxFurnaceCapacity = 50;


    public bool CanStoreInVacuum(int currfentAmmount)
    {
       return currfentAmmount < maxVacuumCapacity;
    }

    public bool CanStoreInFurnace(int currfentAmmount)
    {
        return currfentAmmount < maxFurnaceCapacity;
    }



}
