using Fusion;
using TMPro;
using UnityEngine;

public class SyncTime : NetworkBehaviour
{
    public WeaponSpawn weaponSpawn; // Tham chiếu đến WeaponSpawn để lấy vị trí spawn
    [Networked] public TickTimer Timer { get; set; } // Thời gian đồng bộ
    public TextMeshProUGUI TimerText; // UI Text để hiển thị thời gian
    public float CountdownDuration = 60f; // Thời gian đếm ngược (giây)
    private bool _timerStarted; // Theo dõi trạng thái timer đã khởi động

    public override void Spawned()
    {
        // Khởi tạo trạng thái ban đầu
        _timerStarted = false;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            // Chỉ khởi động timer một lần khi game bắt đầu
            if (LobbyManager.Instance.isGameStarted && !_timerStarted)
            {
                Timer = TickTimer.CreateFromSeconds(Runner, CountdownDuration);
                _timerStarted = true;
                Debug.Log("Timer started with duration: " + CountdownDuration);
            }

            // Kiểm tra nếu timer đã hết
            if (Timer.Expired(Runner) && _timerStarted)
            {
                Debug.Log("Time's up! Ending game...");
                Timer = TickTimer.None; // Reset timer
                _timerStarted = false; // Ngăn lặp lại logic kết thúc
                LobbyManager.Instance.isGameEnded = true; // Đặt trạng thái game kết thúc
                LobbyManager.Instance.OutOfTime(); // Gọi để hiển thị bảng xếp hạng
            }
        }
    }

    public override void Render()
    {

        // Cập nhật UI cho tất cả client
        if (!Timer.IsRunning && _timerStarted)
        {
            TimerText.text = "Time's up!";
            return;
        }

        if (!LobbyManager.Instance.isGameStarted)
        {
            TimerText.text = "Waiting to start...";
            return;
        }

        // Tính thời gian còn lại
        float remainingTime = Timer.RemainingTime(Runner) ?? 0f;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        TimerText.text = $"{minutes:00}:{seconds:00}";
        
    }
}