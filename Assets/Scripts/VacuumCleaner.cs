using System.Collections.Generic;
using UnityEngine;

public class VacuumCleaner : MonoBehaviour
{
    public float suctionRange = 5f;
    public float suctionForce = 3f;
    public LayerMask creatureLayer;

    [SerializeField]
    private List<GameObject> suckedCreatures = new List<GameObject>(); // Added list to store sucked creatures

    [SerializeField]
    private Transform _suckZoneCentre;
    [SerializeField]
    private Transform _suckInPoint;
    [SerializeField]
    private float _suckZoneRadius;
    [SerializeField]
    private float _distanceToStore;

    private StorageCapacityManager storageManager;

    void Start()
    {
        storageManager = FindObjectOfType<StorageCapacityManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SuckIn();
        }
    }

    private void SuckIn()
    {
        iSuckable[] suckablesInZone = FindAllSuckablesInZone();
        SuckCreatures(suckablesInZone);
    }

    iSuckable[] FindAllSuckablesInZone()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_suckZoneCentre.position, _suckZoneRadius, creatureLayer);
        List<iSuckable> suckables = new List<iSuckable>();

        foreach (Collider col in hitColliders)
        {
            iSuckable suckable = col.GetComponent<iSuckable>();
            if (suckable != null)
            {
                suckables.Add(suckable);
            }
        }
        return suckables.ToArray();
    }

    private void SuckCreatures(iSuckable[] suckablesInZone)
    {
        foreach (iSuckable suckable in suckablesInZone)
        {
            GameObject creature = (suckable as MonoBehaviour).gameObject;
            Rigidbody rb = creature.GetComponent<Rigidbody>();

            if (rb != null)
            {
                suckable.OnSuck();
                ApplySuctionForce(rb);
            }

            float distance = Vector3.Distance(_suckInPoint.position, creature.transform.position);

            if (distance < _distanceToStore && storageManager.CanStoreInVacuum())
            {
                StoreCreature(creature);
            }
        }
    }

    private void ApplySuctionForce(Rigidbody rb)
    {
        Vector3 direction = (_suckInPoint.position - rb.position).normalized;
        rb.AddForce(direction * suctionForce);
    }

    private void StoreCreature(GameObject creature)
    {
        if (!suckedCreatures.Contains(creature))
        {
            storageManager.StoreInVacuum(creature);
            suckedCreatures.Add(creature);
            creature.GetComponent<CreatureCore>().StoreCreature();
            creature.SetActive(false);
        }
    }

    public void RemoveCreature(GameObject creature)
    {
        if (suckedCreatures.Contains(creature))
        {
            suckedCreatures.Remove(creature);
        }
    }


    public List<GameObject> GetStoredCreatures()
    {
        return new List<GameObject>(suckedCreatures); // Ensure we return a copy of the list
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_suckZoneCentre.position, _suckZoneRadius);
    }
}
