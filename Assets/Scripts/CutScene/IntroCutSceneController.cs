using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroCutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextScene = "MainGameScene";

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}