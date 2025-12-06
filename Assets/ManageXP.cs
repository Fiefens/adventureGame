using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageXP : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Slider accuracySlider;
    [SerializeField] private Slider communicationSlider;

    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI communicationText;

    private bool alreadyConfirmed = false;

    private GameManager gameManager;

    private float initialXP;
    private float initialPower, initialAccuracy, initialCommunication;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found.");
            enabled = false;
            return;
        }

        var player = gameManager.player;

        initialPower = player.power;
        initialAccuracy = player.accuracy;
        initialCommunication = player.communication;
        initialXP = player.XP;

        SetupSlider(powerSlider, initialPower);
        SetupSlider(accuracySlider, initialAccuracy);
        SetupSlider(communicationSlider, initialCommunication);

        UpdateXPText(initialXP);
        UpdateAttributeTexts();

        // Subscribe to slider events
        powerSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        accuracySlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        communicationSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
    }

    private void SetupSlider(Slider slider, float baseValue)
    {
        slider.minValue = baseValue;
        slider.maxValue = baseValue + initialXP;
        slider.value = baseValue;
    }

    private void OnSliderChanged()
    {
        float totalSpent = GetTotalSpentXP();

        if (totalSpent > initialXP)
        {
            // Revert the last changed slider to prevent overflow
            float allowed = initialXP - (totalSpent - GetLastChangedDelta());
            ClampLastSlider(allowed);
        }

        float remainingXP = Mathf.Clamp(initialXP - GetTotalSpentXP(), 0, initialXP);
        UpdateXPText(remainingXP);
        UpdateAttributeTexts();
    }

    private float GetTotalSpentXP()
    {
        return (powerSlider.value - initialPower) +
               (accuracySlider.value - initialAccuracy) +
               (communicationSlider.value - initialCommunication);
    }

    private void UpdateXPText(float xp)
    {
        if (xpText != null)
            xpText.text = xp.ToString("0");
    }

    private void UpdateAttributeTexts()
    {
        if (powerText != null)
            powerText.text = $"Power: {(int)powerSlider.value}";
        if (accuracyText != null)
            accuracyText.text = $"Accuracy: {(int)accuracySlider.value}";
        if (communicationText != null)
            communicationText.text = $"Communication: {(int)communicationSlider.value}";
    }

    public void ConfirmAndProceed()
    {
        if (alreadyConfirmed)
            return;

        float totalSpent = GetTotalSpentXP();
        float remainingXP = Mathf.Clamp(initialXP - totalSpent, 0, initialXP);

        if (remainingXP > 0)
            return;

        alreadyConfirmed = true;

        var player = gameManager.player;

        player.power = (int)powerSlider.value;
        player.accuracy = (int)accuracySlider.value;
        player.communication = (int)communicationSlider.value;
        player.XP = 0;

        gameManager.IncreaseStage(1);
        gameManager.LoadNewScene();
    }



    private float GetLastChangedDelta()
    {
        float pDelta = powerSlider.value - initialPower;
        float aDelta = accuracySlider.value - initialAccuracy;
        float cDelta = communicationSlider.value - initialCommunication;

        float max = Mathf.Max(pDelta, aDelta, cDelta);
        return max;
    }

    private void ClampLastSlider(float allowedXP)
    {
        float pDelta = powerSlider.value - initialPower;
        float aDelta = accuracySlider.value - initialAccuracy;
        float cDelta = communicationSlider.value - initialCommunication;

        if (pDelta >= aDelta && pDelta >= cDelta)
            powerSlider.value = initialPower + allowedXP;
        else if (aDelta >= pDelta && aDelta >= cDelta)
            accuracySlider.value = initialAccuracy + allowedXP;
        else
            communicationSlider.value = initialCommunication + allowedXP;
    }
}
