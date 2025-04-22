using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;

public class ObjectAmmoPool : NetworkBehaviour
{
    public static ObjectAmmoPool Instance;

    public GameObject prefab;
    public int poolSize = 10;
    private Queue<GameObject> objectPool = new Queue<GameObject>();
    public GameObject firePosition;

    void Start()
    {
        //weaponSpawn = GetComponent<WeaponSpawn>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        Instance = this;
    }

    public GameObject GetObject(Vector3 position)
    {
        var getObj = objectPool.FirstOrDefault(x => !x.activeInHierarchy);
        if (getObj != null)
        {
            getObj.SetActive(true);
            getObj.transform.position = position;
            return getObj;
        }
        else
        {
            GameObject newObj = Runner.Spawn(prefab, position, Quaternion.identity).gameObject;
            newObj.SetActive(false);
            objectPool.Enqueue(newObj);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (Object.HasStateAuthority)
        {
            // Gọi RPC để đồng bộ hóa trả đối tượng với tất cả client
            RPC_ReturnObject(obj.GetComponent<NetworkObject>().Id);
        }
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
}