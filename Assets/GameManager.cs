using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static int currentStage = 0;

    public GameObject gameUI;

    GameObject gameCanvas;

    public PlayerInfo player = new PlayerInfo(PlayerInfo.playerType.Akai);

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadNewScene()
    {
        StartCoroutine(LoadSceneSafely());
    }

    private IEnumerator LoadSceneSafely()
    {
        var quest = GetComponent<QuestSystem>();

        // Safely interact with the UI before scene unload
        if (quest != null && quest.stagePanel != null)
            quest.stagePanel.SetActive(true);

        if (GetStage() > 0 && quest != null)
            quest.Init();

        yield return null; // Wait one frame to let Unity finish current operations

        string nextScene = "level" + (GetStage() + 1);
        SceneManager.LoadScene(nextScene);
    }



    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // GameManager itself

        GameObject player = GameObject.Find("Player");
        if (player != null)
            DontDestroyOnLoad(player); // Persist the Player
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject p = GameObject.Find("Player");
        GameObject spawn = GameObject.Find("startingPoint");

        if (p != null && spawn != null)
        {
            p.transform.position = spawn.transform.position;
            p.transform.rotation = Quaternion.identity;
        }
    }


    public void SetStage(int newStage) { currentStage = newStage; }
    public void IncreaseStage(int increment) { currentStage += increment; }
    public int GetStage() { return (currentStage); }

}
