using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] GameObject titleUI;
    [SerializeField] GameUI gameUI;
    [SerializeField] CinemachineVirtualCamera titleCam, gameCam;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] NPCInitializer npcInitializer;

    void Awake()
    {
        titleUI.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

    public void OnStartButton()
    {
        StartCoroutine(StartGame());
    }

    public void OnOptionButtons()
    {

    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    IEnumerator StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;

        titleUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        titleCam.Priority = 0;
        gameCam.Priority = 10;

        npcInitializer.Initialize(playerController);

        Volume titleVolume = titleCam.GetComponent<Volume>();
        for(int i = 0; i < 10; i++)
        {
            titleVolume.weight -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        playerController.Initialize();
        playerDataManager.Initialize();
        gameUI.Initialize(playerDataManager, playerDataManager.PlayerData);
        gameUI.gameObject.SetActive(true);
    }
}
