using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCleaner : MonoBehaviour
{
    public float suctionRange = 5f;
    public float suctionForce = 3f;
    public LayerMask creatureLayer;

    // List to store sucked creatures
    private List<GameObject> suckedCreatures = new List<GameObject>();

    [SerializeField]
    private Transform _suckZoneCentre;

    [SerializeField]
    private float _suckZoneRadius;

    void Update()
    {
        // Call functions from here
        iSuckable[] suckablesInZone = FindAllSuckablesInZone();
        SuckCreatures(suckablesInZone);
    }

    iSuckable[] FindAllSuckablesInZone()
    {
        Collider[] _hitColliders = Physics.OverlapSphere(_suckZoneCentre.position, _suckZoneRadius, creatureLayer);
        List<iSuckable> suckables = new List<iSuckable>();

        foreach (Collider col in _hitColliders)
        {
            iSuckable iSucker = col.gameObject.GetComponent<iSuckable>();
            if (iSucker != null)
            {
                Debug.Log("Sucker found");
                suckables.Add(iSucker); // Add the suckable to the list
            }
        }

        return suckables.ToArray(); // Return as an array
    }

    private void SuckCreatures(iSuckable[] suckablesInZone)
    {
        foreach (iSuckable suckable in suckablesInZone)
        {
            GameObject creature = (suckable as MonoBehaviour).gameObject; // Cast to MonoBehaviour to access the GameObject

            // Suck the creature in
            Vector3 direction = (transform.position - creature.transform.position).normalized;
            creature.GetComponent<Rigidbody>().AddForce(direction * suctionForce);

            if (Vector3.Distance(transform.position, creature.transform.position) < 1f)
            {
                StoreCreature(creature);
                suckable.OnSuck();
            }
        }
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
}
