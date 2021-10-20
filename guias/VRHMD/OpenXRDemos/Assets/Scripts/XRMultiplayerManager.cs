using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRMultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private string _prefabname = "";

    private Realtime _realtime;
   

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();

        _realtime.didConnectToRoom += Realtime_didConnectToRoom;

    }

    private void Realtime_didConnectToRoom(Realtime realtime)
    {
        int prefabcount = _realtime.room.datastore.prefabViewModels.Count;
            
        var _localMultiplayerCube = Realtime.Instantiate(_prefabname,
            position: new Vector3(transform.position.x + (prefabcount * 0.7f),transform.position.y, transform.position.z),
            rotation: transform.rotation, 
            ownedByClient: true,
            preventOwnershipTakeover: false,
            destroyWhenOwnerOrLastClientLeaves: true,
            useInstance: _realtime);
    

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.05f);

    }
}
