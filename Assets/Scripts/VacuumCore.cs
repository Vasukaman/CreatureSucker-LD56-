using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCore : MonoBehaviour
{
    [SerializeField]
    private Transform _suckZoneCentre;

    [SerializeField]
    private float _suckZoneRadius;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindAllSuckablesInZone();
    }

    iSuckable[] FindAllSuckablesInZone()
    {
        Collider[] _hitColliders = Physics.OverlapSphere(_suckZoneCentre.position, _suckZoneRadius);

        foreach (Collider col in _hitColliders)
        {
            //Debug.Log("Object Found");
            iSuckable iSucker = col.gameObject.GetComponent<iSuckable>();
            if (iSucker != null)
            {
                Debug.Log("Sucker found");
            }
        }

        return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_suckZoneCentre.position, _suckZoneRadius);
    }
}
