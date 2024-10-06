using System.Collections.Generic;
using UnityEngine;

public class VacuumCleaner : MonoBehaviour
{
    public float suctionRange = 5f;
    public float suctionForce = 3f;
    public LayerMask creatureLayer;

    private List<GameObject> suckedCreatures = new List<GameObject>();

    [SerializeField]
    private Transform _suckZoneCentre;

    [SerializeField]
    private Transform _suckInPoint;

    [SerializeField]
    private float _suckZoneRadius;

    [SerializeField]
    private float _distanceToStore;

    void Update()
    {
       
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            Debug.Log("LMB");
            SuckIn();
        }
    }

    private void SuckIn()
    {
        iSuckable[] suckablesInZone = FindAllSuckablesInZone();
        Debug.Log(suckablesInZone.Length);
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
                Debug.Log("SUCK");
                suckable.OnSuck();
                ApplySuctionForce(rb);
            }

            if (Vector3.Distance(_suckInPoint.transform.position, creature.transform.position) < _distanceToStore)
            {
                StoreCreature(creature);
                suckable.OnSuck();
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
            suckedCreatures.Add(creature);
            creature.SetActive(false);
        }
    }

    public List<GameObject> GetStoredCreatures()
    {
        return suckedCreatures;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_suckZoneCentre.position, _suckZoneRadius);
    }
}
