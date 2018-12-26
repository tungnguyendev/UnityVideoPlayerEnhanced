using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public RawImage image;
    public VideoPlayer videoPlayer;

    public GameObject videoControl;
    public GameObject controlPanel;
    public GameObject loading;
    public GameObject btnPause;

    public Text totalTime;
    public Text currentTime;

    public Image lifeTime;
    public Image imagePause;

    public Image PrevBGColor;
    public Image NextBGColor;

    private bool isControlPanel = false;

    public Sprite[] sprites;

    private bool isNextFrame = true;
    private bool isBackFrame = false;

    private void Awake()
    {
        IsLoading();

        isControlPanel = false;
        videoControl.SetActive(false);

        imagePause.sprite = sprites[0];
    }

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(playVideo());
    }

    private void IsLoading(bool isLoading = true)
    {
        if (isLoading)
        {
            btnPause.SetActive(false);
            loading.SetActive(true);
        }
        else
        {
            btnPause.SetActive(true);
            loading.SetActive(false);
        }
    }

    IEnumerator playVideo()
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        IsLoading(false);
        isControlPanel = false;
        videoControl.SetActive(false);
        controlPanel.SetActive(false);

        image.texture = videoPlayer.texture;

        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Done/Pause Video");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        TimeSpan ts = TimeSpan.FromSeconds(videoPlayer.time);
        currentTime.text = ts.ToString().Substring(0, ts.ToString().IndexOf('.'));

        lifeTime.fillAmount = 1 - (float)videoPlayer.frame / videoPlayer.frameCount;

        if (videoPlayer.isPlaying)
        {
            imagePause.sprite = sprites[0];
        }
        else
        {
            imagePause.sprite = sprites[1];
        }

        if ((videoPlayer.frameCount / videoPlayer.frameRate) - videoPlayer.time < 10)
        {
            isNextFrame = false;
            NextBGColor.color = new Color(140f / 255, 140f / 255, 140f / 255);
        }
        else
        {
            isNextFrame = true;
            NextBGColor.color = new Color(1f, 1f, 1f);
        }


        if ((videoPlayer.time - 10) < 0)
        {
            isBackFrame = false;
            PrevBGColor.color = new Color(140f / 255, 140f / 255, 140f / 255);
        }
        else
        {
            isBackFrame = true;
            PrevBGColor.color = new Color(255f, 255f, 255f);
        }
    }

    public void OnTouch()
    {
        SetTotalTime();
        if (videoPlayer.isPlaying)
        {
            videoControl.SetActive(true);

            if (!isControlPanel)
            {
                isControlPanel = true;
                controlPanel.SetActive(true);
            }
            else
            {
                isControlPanel = false;
                controlPanel.SetActive(false);
            }
        }
        else
        {
            if (!isControlPanel)
            {
                isControlPanel = true;
                videoControl.SetActive(true);
            }
            else
            {
                isControlPanel = false;
                videoControl.SetActive(false);
            }
        }
    }

    public void OnPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }

    public void OnNextFrame()
    {
        if (isNextFrame)
        {
            //videoPlayer.frame = videoPlayer.frame + (int)(videoPlayer.frameRate * 10);
            videoPlayer.time = videoPlayer.time + 10;

            if (videoPlayer.time + 10 > (videoPlayer.frameCount / videoPlayer.frameRate))
            {
                isNextFrame = false;
                NextBGColor.color = new Color(140f / 255, 140f / 255, 140f / 255);
            }
            else
            {
                isNextFrame = true;
                NextBGColor.color = new Color(1f, 1f, 1f);
            }
        }
        else
        {
            Debug.Log("This function is disabled.");
        }
    }

    public void OnBackFrame()
    {
        if (isBackFrame)
        {
            //videoPlayer.frame = videoPlayer.frame - (int)(videoPlayer.frameRate * 10);
            videoPlayer.time = videoPlayer.time - 10;

            if ((videoPlayer.time - 10) < 0)
            {
                isBackFrame = false;
                PrevBGColor.color = new Color(140f / 255, 140f / 255, 140f / 255);
            }
            else
            {
                isBackFrame = true;
                PrevBGColor.color = new Color(255f, 255f, 255f);
            }
        }
        else
        {
            Debug.Log("This function is disabled.");
        }
    }

    public void SetTotalTime()
    {
        TimeSpan ts = TimeSpan.FromSeconds(videoPlayer.frameCount / videoPlayer.frameRate);
        totalTime.text = ts.ToString().Substring(0, ts.ToString().IndexOf('.'));
    }
}
