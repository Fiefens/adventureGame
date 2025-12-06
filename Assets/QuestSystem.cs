using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    int currentStage;
    bool timerStarted;
    float timer;

    List<string> actions, targets, xps;
    List<bool> objectiveAchieved;

    string stageTitle, stageDescription, stageObjectives, startingPointForPlayer;
    bool panelDisplayed = true;

    int nbObjectivesAchieved = 0;
    int nbObjectivesToAchieve;
    int XPAchieved;

    public TextMeshProUGUI userMessageText;

    public enum possibleActions { do_nothing = 0, talk_to = 1, acquire_a = 2, destroy_one = 3, enter_place_called = 4 }
    List<possibleActions> actionsForQuest;

    public GameObject stagePanel;
    public TextMeshProUGUI stageTitleText;
    public TextMeshProUGUI stageDescriptionText;
    public TextMeshProUGUI stageObjectivesText;

    // XP Display Message Components
    float displayTimer;
    bool startDisplayTimer;
    [SerializeField] GameObject Canvas;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
        MovePlayerToStartingPoint();
        DontDestroyOnLoad(Canvas);
    }

    void Display(bool display)
    {
        stagePanel.SetActive(display);
    }

    public void MovePlayerToStartingPoint()
    {
        GameObject p = GameObject.Find("Player");
        if (p == null)
        {
            Debug.LogWarning("Player not found in scene and not carried over.");
            return;
        }

        GameObject startingPoint = GameObject.Find("startingPoint");
        if (startingPoint != null)
        {
            p.transform.position = startingPoint.transform.position;
            p.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Starting point not found in scene.");
        }
    }





    void DisplayQuestInfo()
    {
        stageTitleText.text = stageTitle;
        stageDescriptionText.text = stageDescription;
        stageObjectivesText.text = stageObjectives + "\n Press H to Hide/Display this information";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            panelDisplayed = !panelDisplayed;
            Display(panelDisplayed);
        }

        // Handle display timer for XP message
        if (startDisplayTimer)
        {
            displayTimer += Time.deltaTime;
            if (displayTimer >= 2f)
            {
                displayTimer = 0f;
                startDisplayTimer = false;
                if (userMessageText != null)
                    userMessageText.text = "";
            }
        }
    }

    public void Init()
    {
        currentStage = GetComponent<GameManager>().GetStage();
        nbObjectivesAchieved = 0;
        actions = new List<string>();
        targets = new List<string>();
        xps = new List<string>();
        objectiveAchieved = new List<bool>();
        actionsForQuest = new List<possibleActions>();

        LoadQuest2();
        DisplayQuestInfo();
    }

    void DisplayMessage(string message)
    {
        if (userMessageText != null)
        {
            userMessageText.text = message;
            startDisplayTimer = true;
        }
    }

    public void Notify(possibleActions action, string target)
    {
        Debug.Log("Notified: Action=" + action + " Target:" + target);

        for (int i = 0; i < actionsForQuest.Count; i++)
        {
            if (action == actionsForQuest[i] &&
                target == targets[i] &&
                !objectiveAchieved[i])
            {
                string xpMessage = "+" + xps[i] + " XP";
                Debug.Log(xpMessage);
                DisplayMessage(xpMessage);

                nbObjectivesAchieved++;
                XPAchieved += Int32.Parse(xps[i]);
                objectiveAchieved[i] = true;
            }
        }

        if (nbObjectivesAchieved >= objectiveAchieved.Count)
        {
            DisplayMessage("Stage Complete");

            GetComponent<GameManager>().player.XP = CalculateTotalXPForLevel();

            Invoke("StageComplete", 2);
        }

    }

    void StageComplete()
    {
        if (SceneManager.GetActiveScene().name != "level3")
            SceneManager.LoadScene("levelComplete");
        else
            SceneManager.LoadScene("endScreen");
    }

    int CalculateTotalXPForLevel()
    {
        int totalXP = 0;

        for (int i = 0; i < actionsForQuest.Count; i++)
        {
            totalXP += Int32.Parse(xps[i]);
        }

        return totalXP;
    }

    public void LoadQuest2()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("quest");
        if (textAsset == null)
        {
            Debug.LogError("Quest XML not found in Resources.");
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);

        stageObjectives = "For this stage, you need to:\n";

        foreach (XmlNode stage in doc.SelectNodes("quest/stage"))
        {
            if (stage.Attributes.GetNamedItem("id").Value == currentStage.ToString())
            {
                stageTitle = stage.Attributes.GetNamedItem("name").Value;
                stageDescription = stage.Attributes.GetNamedItem("description").Value;

                foreach (XmlNode results in stage)
                {
                    foreach (XmlNode result in results)
                    {
                        string action = result.Attributes.GetNamedItem("action").Value;
                        string target = result.Attributes.GetNamedItem("target").Value;
                        string xp = result.Attributes.GetNamedItem("xp").Value;

                        possibleActions actionForQuest = possibleActions.do_nothing;

                        if (action.Contains("Acquire"))
                            actionForQuest = possibleActions.acquire_a;
                        else if (action.Contains("Talk"))
                            actionForQuest = possibleActions.talk_to;
                        else if (action.Contains("Destroy") && action.Contains("one"))
                            actionForQuest = possibleActions.destroy_one;
                        else if (action.Contains("Enter") && action.Contains("place"))
                            actionForQuest = possibleActions.enter_place_called;

                        actionsForQuest.Add(actionForQuest);
                        actions.Add(action);
                        targets.Add(target);
                        xps.Add(xp);
                        objectiveAchieved.Add(false);

                        Debug.Log($"{action} {target} [{xp} XP]");
                        stageObjectives += $"\n -> {action} {target} [ {xp} XP]";
                        nbObjectivesToAchieve++;
                    }
                }

                break;
            }
        }
    }
}
