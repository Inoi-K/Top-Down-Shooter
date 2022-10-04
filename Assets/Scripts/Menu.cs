using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToggle;
    public int[] screenWidths;

    int activeScreenIndex;

    private void Start() {
        activeScreenIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;

        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;

        for (int i = 0; i < resolutionToggles.Length; ++i)
            resolutionToggles[i].isOn = i == activeScreenIndex;

        fullscreenToggle.isOn = isFullscreen;
    }

    public void Play() {
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        Application.Quit();
    }

    public void OptionsMenu() {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu() {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetScreenResolution(int i) {
        if (resolutionToggles[i].isOn) {
            activeScreenIndex = i;
            float aspectRation = 16 / 9;
            Screen.SetResolution(screenWidths[i], (int) (screenWidths[i] / aspectRation), false);
            PlayerPrefs.SetInt("screen res index", activeScreenIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen) {
        for (int i = 0; i < resolutionToggles.Length; ++i) {
            resolutionToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen) {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        } else {
            SetScreenResolution(activeScreenIndex);
        }

        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value) {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value) {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSfxVolume(float value) {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }
}
