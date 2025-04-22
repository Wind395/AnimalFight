using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TestUI
{
    // Test 1: Load Scene
    [UnityTest]
    public IEnumerator TestLoadScene()
    {
        // Giả sử tên scene cần test là "MainScene" và đã được thêm vào Build Settings
        SceneManager.LoadScene("SceneTest_1");
        // Chờ cho scene load xong (có thể chờ thêm vài frame nếu cần)
        yield return null;
        // Lấy scene hiện tại và kiểm tra tên
        Scene currentScene = SceneManager.GetActiveScene();
        Assert.AreEqual("SceneTest_1", currentScene.name, "Scene không được load đúng");
    }

    // Test 2: Trừ máu (Health Slider)
    [UnityTest]
    public IEnumerator TestHealthSliderReduction()
    {
        // Giả sử scene game chứa health slider là "GameScene"
        SceneManager.LoadScene("SceneTest_1");
        yield return null; // chờ scene load

        // Tìm GameObject chứa health slider. Đảm bảo GameObject có tên "HealthSlider"
        GameObject sliderObj = GameObject.Find("HealthSlider");
        Assert.IsNotNull(sliderObj, "Không tìm thấy HealthSlider");

        Slider healthSlider = sliderObj.GetComponent<Slider>();
        Assert.IsNotNull(healthSlider, "Component Slider không được tìm thấy trên HealthSlider");

        // Lưu lại giá trị ban đầu
        float initialValue = healthSlider.value;

        // Giả lập việc trừ máu (ví dụ: trừ 10 đơn vị)
        healthSlider.value -= 10;
        yield return null; // chờ cập nhật UI

        // Kiểm tra giá trị slider đã giảm đúng
        Assert.AreEqual(initialValue - 10, healthSlider.value, "Giá trị HealthSlider không giảm đúng sau khi trừ máu");
    }

    // Test 3: Setting - Âm thanh
    [UnityTest]
    public IEnumerator TestAudioSettings()
    {
        // Giả sử scene chứa menu setting là "SettingsScene"
        SceneManager.LoadScene("SceneTest_1");
        yield return null; // chờ scene load

        // Tìm GameObject chứa slider âm thanh, giả sử tên là "AudioSlider"
        GameObject audioSliderObj = GameObject.Find("AudioSlider");
        Assert.IsNotNull(audioSliderObj, "Không tìm thấy AudioSlider");

        Slider audioSlider = audioSliderObj.GetComponent<Slider>();
        Assert.IsNotNull(audioSlider, "Component Slider không được tìm thấy trên AudioSlider");

        // Giả lập thay đổi âm lượng (ví dụ: chuyển slider đến 50%)
        audioSlider.value = 0.5f;
        yield return null; // chờ cập nhật UI

        // Kiểm tra giá trị của slider và cập nhật AudioListener (nếu có)
        Assert.AreEqual(0.5f, audioSlider.value, "Giá trị AudioSlider không được cập nhật chính xác");

        // Giả sử hệ thống âm thanh được cập nhật theo AudioListener.volume
        AudioListener.volume = audioSlider.value;
        Assert.AreEqual(0.5f, AudioListener.volume, "AudioListener.volume không được cập nhật đúng");
    }

    
    
}
