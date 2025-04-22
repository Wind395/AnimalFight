using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;

public class ObjectPooling : NetworkBehaviour
{
    public static ObjectPooling Instance;

    public GameObject prefab;
    public int poolSize = 10;
    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private WeaponSpawn weaponSpawn;


    void Start()
    {
        // weaponSpawn = GetComponent<WeaponSpawn>();
        // for (int i = 0; i < poolSize; i++)
        // {
        //     GameObject obj = Instantiate(prefab, weaponSpawn.transform.position, Quaternion.identity);
        //     //RPC_ReturnObject(obj.GetComponent<NetworkObject>().Id);
        //     //RPC_SetObjectInactive(obj.GetComponent<NetworkObject>().Id);
        //     objectPool.Enqueue(obj);
        // }
        Instance = this;
    }

    public GameObject GetObject()
    {
        var getObj = objectPool.FirstOrDefault(x => !x.activeInHierarchy);
        if (getObj != null)
        {
            getObj.SetActive(true);
            return getObj;
        }
        else
        {
            //Debug.Log("Spawned new object: " + prefab.name);
            return Runner.Spawn(prefab, transform.position, Quaternion.identity).gameObject;
        }
            //RPC_SetObjectInactive(newObj.GetComponent<NetworkObject>().Id);
            //objectPool.Enqueue(newObj);
    }

    public void ReturnObject(GameObject obj)
    {
        // if (Object.HasStateAuthority)
        // {
        //     // Gọi RPC để đồng bộ hóa trả đối tượng với tất cả client
        //     RPC_ReturnObject(obj.GetComponent<NetworkObject>().Id);
        // }

        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ReturnObject(NetworkId objectId)
    {
        var networkObject = Runner.FindObject(objectId);
        if (networkObject != null)
        {
            GameObject obj = networkObject.gameObject;
            obj.SetActive(false);
            if (!objectPool.Contains(obj))
            {
                objectPool.Enqueue(obj);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SetObjectInactive(NetworkId objectId)
    {
        var networkObject = Runner.FindObject(objectId);
        if (networkObject != null)
        {
            networkObject.gameObject.SetActive(false);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SetObjectActive(NetworkId objectId)
    {
        var networkObject = Runner.FindObject(objectId);
        if (networkObject != null)
        {
            networkObject.gameObject.SetActive(true);
        }
    }
}