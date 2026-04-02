using UnityEngine;
using UnityEngine.Video;

public class EndCutSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject creditImage;

    void Start()
    {
        creditImage.SetActive(false);
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        creditImage.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
        Application.Quit();
    }
}

