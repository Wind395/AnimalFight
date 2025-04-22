using UnityEngine;
using Fusion;
using Unity.Cinemachine;

public class AnimalCamera : NetworkBehaviour
{
    private CinemachineCamera _cameraTopDowm;

    AnimalProperties animalProperties;
    // private CinemachineCamera _cameraFirstPerson;
    // private CinemachineCamera _cameraThirdPerson;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraTopDowm = GameObject.Find("CinemachineCamera (LookAt)").GetComponent<CinemachineCamera>();
        animalProperties = GetComponent<AnimalProperties>();
        // _cameraFirstPerson = GameObject.Find("CinemachineCamera (FirstPerson)").GetComponent<CinemachineCamera>();
        // _cameraThirdPerson = GameObject.Find("CinemachineCamera (3rd)").GetComponent<CinemachineCamera>();
        // _cameraFirstPerson.gameObject.SetActive(false);
        // _cameraThirdPerson.gameObject.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        LookAtCamera(player.transform);

        //_cameraTopDowm.gameObject.SetActive(true);
        
        // SwitchToFirstPerson(transform);
        // SwitchToThirdPerson(transform);
    }

    // public override void Spawned()
    // {
    //     _cameraTopDowm.enabled = Object.HasStateAuthority;
    //     // _cameraFirstPerson.enabled = Object.HasStateAuthority;
    //     // _cameraThirdPerson.enabled = Object.HasStateAuthority;
    //     LookAtCamera(transform);
    //     // SwitchToFirstPerson(transform);
    //     // SwitchToThirdPerson(transform);
    // }

    void Update()
    {

    }

    // public void SwitchCamera(Transform target)
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         _cameraFirstPerson.gameObject.SetActive(false);
    //         _cameraThirdPerson.gameObject.SetActive(false);
    //         _cameraTopDowm.gameObject.SetActive(true);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         _cameraFirstPerson.gameObject.SetActive(true);
    //         _cameraThirdPerson.gameObject.SetActive(false);
    //         _cameraTopDowm.gameObject.SetActive(false);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         _cameraFirstPerson.gameObject.SetActive(false);
    //         _cameraThirdPerson.gameObject.SetActive(true);
    //         _cameraTopDowm.gameObject.SetActive(false);
    //     }
    // }

    public void LookAtCamera(Transform target)
    {
        _cameraTopDowm.Follow = target;
        _cameraTopDowm.LookAt = target;
    }

    // public void SwitchToFirstPerson(Transform target)
    // {
    //     _cameraFirstPerson.Follow = target;
    //     _cameraFirstPerson.LookAt = target;
    // }

    // public void SwitchToThirdPerson(Transform target)
    // {
    //     _cameraThirdPerson.Follow = target;
    //     _cameraThirdPerson.LookAt = target;
    // }

    
}