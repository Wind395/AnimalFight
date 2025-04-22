using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class SpectatorMode : MonoBehaviour
{

    public static SpectatorMode Instance;

    // Sử dụng CinemachineVirtualCamera để điều khiển camera
    private GameObject[] targets;
    private CinemachineCamera spectatorCamera;
    private AnimalProperties animalProperties;
    public GameObject exitBtn; // Tham chiếu đến nút thoát chế độ spectator

    private bool canSwitch = false;
    public bool IsSpectatorModeActive = false;

    void Start()
    {
        spectatorCamera = GetComponent<CinemachineCamera>();
        if (animalProperties == null)
        {
            Debug.Log("Không tìm thấy AnimalProperties trên đối tượng có tag Player!");
        }
        if (spectatorCamera == null)
        {
            Debug.Log("Không tìm thấy CinemachineVirtualCamera trên SpectatorMode object!");
        }

        Instance = this;
    }

    void Update()
    {
        // Nếu chưa ở chế độ spectator, thử tìm đối tượng có tag "Player" và lấy component AnimalProperties
        if (!canSwitch)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                animalProperties = playerObj.GetComponent<AnimalProperties>();
                if (animalProperties != null && animalProperties.isDead)
                {
                    Debug.Log("Player đã chết, kích hoạt chế độ spectator!");
                    canSwitch = true;
                    ActivateSpectatorMode();
                }
            }
            else
            {
                // Không tìm thấy đối tượng Player, có thể đã bị destroy
                // Trong trường hợp này, không làm gì thêm
            }
        }

        if (IsSpectatorModeActive)
        {
            // Cập nhật danh sách các target (đối tượng có tag "Target")
            targets = GameObject.FindGameObjectsWithTag("Target");

            exitBtn.gameObject.SetActive(true);
            // Nếu đã ở chế độ spectator, cho phép chuyển target bằng phím V
            if (canSwitch && Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("Chuyển target!");
                CycleSpectatorTarget();
            }
        }
    }

    public void SetSpectatorMode(Transform target)
    {
        spectatorCamera.Follow = target;
        spectatorCamera.LookAt = target;
    }

    public void ActivateSpectatorMode()
    {
        // Tìm target ban đầu (người chơi khác) để theo dõi
        Transform target = FindInitialSpectatorTarget();

        // Kích hoạt chế độ spectator trên SpectatorMode component

        SetSpectatorMode(target);


        // Nếu muốn chuyển giao quyền điều khiển hoàn toàn sang chế độ spectator,
        // bạn có thể vô hiệu các component điều khiển hoặc ẩn UI của player này.
        // Ví dụ: GetComponent<PlayerController>().enabled = false;
    }

    private Transform FindInitialSpectatorTarget()
    {
        // Tìm tất cả đối tượng có tag "Player" và chọn đối tượng khác chính nó
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            if (target != this.gameObject)
            {
                return target.transform;
            }
        }
        Debug.Log("Không tìm thấy target nào để theo dõi trong chế độ spectator!");
        return null; // Trả về null nếu không tìm thấy target nào
    }

    private void CycleSpectatorTarget()
    {
        // Lấy danh sách tất cả các đối tượng có tag "Player"
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Target");
        if (targets.Length == 0)
        {
            Debug.Log("Không có target nào để chuyển.");
            return;
        }

        Debug.Log("Có " + targets.Length + " target để chuyển.");
        // Xác định target hiện tại mà camera đang theo dõi
        Transform currentTarget = spectatorCamera.Follow;
        int currentIndex = -1;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform == currentTarget)
            {
                currentIndex = i;
                break;
            }
        }

        // Chọn target tiếp theo theo vòng tròn
        int nextIndex = (currentIndex + 1) % targets.Length;
        Transform nextTarget = targets[nextIndex].transform;
        SetSpectatorMode(nextTarget);
        Debug.Log("Chuyển sang theo dõi target: " + targets[nextIndex].name);
    }
}
