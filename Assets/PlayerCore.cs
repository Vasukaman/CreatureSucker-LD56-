using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{

    [SerializeField] private PlayerDataSO _playerDataSO;
    // Start is called before the first frame update
    void Start()
    {
        _playerDataSO.playerObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
