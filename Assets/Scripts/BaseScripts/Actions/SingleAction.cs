using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Cinemachine;


public class SingleAction : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private DialogueSetup dialogueSetup;
    private ActionController actionController;
    private bool isPaused = false;
    private bool isFading = false;
    private SSPlayerController playerController;
    private CutsceneCameraShake cameraShake;
    private GameObject player;
    private LevelControl levelController;
    private SSPlayerHealth playerHealth;

    [HideInInspector]
    public bool isPlayingDialogue = false;

    private bool cameraHasReachedTargetDestination = true;
    private bool levelLoading;
    private FadeController fadeController;
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera playerCamera;
    
    public string actionName;
    public bool actionHasFinished = false;
    public float playerDelayBeforeMoving;
    public bool playerCanMove;
    public Transform playerStartPosition;
    public Transform playerDestination;
    public float timeBeforePlayerCanMove = 0f;
    public NpcController[] npcControllers;
    public Transform[] targetDestinations;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    public bool startScreenOnSolidColor;
    public bool shouldUseWhite;
    public bool shouldFadeOut;
    public bool shouldFadeIn;
    public float fadeTime;
    public float solidColorTime;
    public float pausedTime;
    public GameObject newSpawnPoint;
    public bool preventCameraSwitching;
    public bool allowCameraSwitching;
    public bool setCameraToFollowPlayer;
    public float cameraMoveTime = 0;
    public CinemachineVirtualCamera virtualCamera;
    public bool shakeCamera;
    public bool stopCameraShake;
    public float cameraShakeIntensity;
    public string musicName;
    public float musicPauseBeforePlayTime;
    public float musicFadeOutTime;
    public bool fadeMusic;
    public GameObject sfxOneShot;
    public bool playSoundEffect;
    public string soundEffectNameToPlay;
    public string specificActionToLoad;
    public string levelToLoad;
    public string setTaskComplete;
    public SteamAchievement steamAchievement;
    public bool makePlayerInvincible;
    public GameObject nextActionToLoad;

    private void OnEnable()
    {
        actionName = gameObject.name;
    }

    void Start()
    {
        print(actionName);
        actionController = FindObjectOfType<ActionController>();
        fadeController = GetComponent<FadeController>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueSetup = GetComponentInChildren<DialogueSetup>();
        playerController = FindObjectOfType<SSPlayerController>();
        cameraShake = GetComponent<CutsceneCameraShake>();
        levelController = FindObjectOfType<LevelControl>();
        playerHealth = FindObjectOfType<SSPlayerHealth>();
        if(playerController != null)
        {
            player = playerController.gameObject;
        }
        
        PlacePlayerAtStart();
        ActivateAllObjects();
        DeactivateAllObjects();
        HandleFading();
        HandleCameraSwitching();
        HandlePlayerAbilityToMove();
        MovePlayerToDestination();
        MoveNpcsToDestination();
        SetTaskAsComplete();
        

        if (pausedTime > 0)
        {
            isPaused = true;
            Invoke("UnPauseScene", pausedTime);
        }
        
        if (newSpawnPoint != null)
        {
            playerController.gameObject.transform.position = newSpawnPoint.transform.position;
        }

        if(!string.IsNullOrEmpty(musicName))
        {
            SoundManager.Instance.FadeAndPlayMusic(musicName, 1f);
        }

        if (fadeMusic)
        {
                
        }

        if (sfxOneShot != null)
        {
            GameObject sfxToPlay = Instantiate(sfxOneShot, transform.position, transform.rotation);
        }

        if (!string.IsNullOrEmpty(soundEffectNameToPlay))
        {
            if (playSoundEffect)
            {
                SoundManager.Instance.Play(soundEffectNameToPlay);
            } else
            {
                SoundManager.Instance.FadeOutSound(soundEffectNameToPlay, 0.5f);
            }
            
        }

        if (shakeCamera && cameraShake != null)
        {
            cameraShake.ShakeCamera(cameraShakeIntensity);
        }

        if (stopCameraShake && cameraShake != null)
        {
            cameraShake.StopShakingCamera();
        }

        if(dialogueSetup != null)
        {
            PlayDialogueImmediately();
        }

        if(playerHealth != null)
        {
            if (makePlayerInvincible)
            {
                playerHealth.isInvunerable = true;
            }
            else
            {
                playerHealth.isInvunerable = false;
            }

        }

        if (steamAchievement != null)
        {
            //steamAchievement.ActivateAchievement();
        }

        if (!string.IsNullOrEmpty(levelToLoad)){
            LoadNextLevel();
        }
    }

    void Update()
    {

        if (!isPaused && !isFading && !isPlayingDialogue && CheckHasPlayerReachedFinalDestination() && CheckIfAllNPCsHasReachedFinalDestination())
        {
            LoadNextAction();
        }
    }

    public void PlacePlayerAtStart()
    {
        if(playerStartPosition != null)
        {
            player.transform.position = playerStartPosition.position;
        }
    }

    public void AllowPlayerMovement()
    {
        playerController.ChangeStateToIdle();
    }

    public void MovePlayerToDestination()
    {
        if (playerDestination != null)
        {
            playerController.playerDestination = playerDestination;
            playerController.ChangeStateToSceneControlled();
        }
    }

    public bool CheckHasPlayerReachedFinalDestination()
    {
        if(playerController != null)
        {
            if (playerController.hasReachedDestination)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void MoveNpcsToDestination ()
    {
        if(npcControllers.Length > 0)
        {
            for (int i = 0; i < npcControllers.Length; i++)
            {
                npcControllers[i].targetDestination = targetDestinations[i];
                npcControllers[i].ChangeStateToSceneControlled();
            }
        }
    }

    public bool CheckIfAllNPCsHasReachedFinalDestination()
    {
        bool allNPCsHaveReachedFinalDestination = true;

        if (npcControllers.Length > 0)
        {
            for (int i = 0; i < npcControllers.Length; i++)
            {
                if (!npcControllers[i].hasReachedDestination)
                {
                    allNPCsHaveReachedFinalDestination = false;
                }
            }
        }

        return allNPCsHaveReachedFinalDestination;
    }

    public void ActivateAllObjects()
    {
        if(objectsToActivate == null)
        {
            return;
        }

        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(true);
        }
    }

    public void DeactivateAllObjects ()
    {
        if (objectsToDeactivate == null)
        {
            return;
        }

        for (int i = 0; i < objectsToDeactivate.Length; i++)
        {
            objectsToDeactivate[i].SetActive(false);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(this.levelToLoad);
    }

    public void UnPauseScene()
    {
        isPaused = false;
    }

    public void HandleFading()
    {
        if (startScreenOnSolidColor)
        {
            if (shouldUseWhite)
            {
                fadeController.SetScreenToWhite();
            } else
            {
                fadeController.SetScreenToBlack();
            }
        }

        if (shouldUseWhite)
        {
            fadeController.useWhiteOut = true;
        } else
        {
            fadeController.useWhiteOut = false;
        }

        if (shouldFadeIn)
        {
            isFading = true;
            Invoke("FadeIn", solidColorTime);
        }
        else if (shouldFadeOut)
        {
            isFading = true;
            FadeOut();
        }
    }

    public void FadeIn()
    {
        fadeController.FadeIn(fadeTime + 0.2f);
        Invoke("EndFade", fadeTime);
    }

    public void FadeOut()
    {
        fadeController.FadeOut(fadeTime);
        Invoke("EndFade", fadeTime + solidColorTime);
    }

    public void EndFade()
    {
        isFading = false;
    }

    public void HandleCameraSwitching()
    {

        if (cameraMoveTime != 0)
        {
            //cinemachineBrain.m_DefaultBlend.m_Time = cameraMoveTime;
        }

        if (setCameraToFollowPlayer)
        {
            SwitchToPlayerCamera();
        }

        if (virtualCamera != null)
        {
            SwitchVirtualCameras();
        }
    }

    public void SwitchVirtualCameras()
    {
        playerCamera.Priority = 0;
        virtualCamera.Priority = 10;
        actionController.currentCamera = virtualCamera;
    }

    public void SwitchToPlayerCamera()
    {
        actionController.currentCamera.Priority = 0;
        actionController.currentCamera = playerCamera;
        playerCamera.Priority = 10;
    }

    public void HandlePlayerAbilityToMove()
    {
        if(playerController == null)
        {
            return;
        }

        if (playerCanMove && !playerController.isDead)
        {
            Invoke("AllowPlayerMovement", timeBeforePlayerCanMove);
        }
        else
        {
            if (!playerController.isDead)
            {
                playerController.ChangeStateToFrozen();
            }  
        }
    }

    public void PlayDialogueImmediately()
    {
        isPlayingDialogue = true;
        dialogueManager.ClearAllDialogueFromList();
        dialogueSetup.ConstructDialogue();
        dialogueManager.currentDialogueSetup = dialogueSetup;
        dialogueManager.ActivateDialogue();
    }

    public void SetTaskAsComplete()
    {
        if (!string.IsNullOrEmpty(setTaskComplete))
        {
            QuestManager.Instance.SetTaskAsComplete(setTaskComplete);
            //levelController.UpdateAllStateUpdaters();
        }
    }

    public void LoadNextAction()
    {
        if(nextActionToLoad != null)
        {
            nextActionToLoad.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}


