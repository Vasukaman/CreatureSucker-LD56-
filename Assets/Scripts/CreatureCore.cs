using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCore : MonoBehaviour, iSuckable
{
    [SerializeField] NavMeshAgent _navAgent;

    [SerializeField] private PlayerDataSO _playerDataSO;

    private bool isBeingSucked=false;
    public bool GetIsBeingSucked()
    {
        return isBeingSucked;
    }

    public float GetDistanceFromTheZone()
    {
        Vector3 suctionZonePosition = _playerDataSO.playerObject.transform.position;
        return Vector3.Distance(transform.position, suctionZonePosition);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_navAgent)
        {
            _navAgent.destination = _playerDataSO.playerObject.transform.position;
        }

    }

    public void OnSuck()
    {
        _navAgent.enabled = false;
        isBeingSucked = true;
    }
}
