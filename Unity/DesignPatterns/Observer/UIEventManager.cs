using System;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    public static UIEventManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public event Action<bool> OnInventoryToggled;
    public event Action<string> OnShowTooltip;
    public event Action OnHideTooltip;
    public event Action<int, int> OnHealthChanged;             
    public event Action<int, int> OnManaChanged;               
    public event Action<int> OnLevelUp;                        
    public event Action<string> OnDialogueLineShown;           
    public event Action OnDialogueClosed;
    public event Action<int> OnQuestTrackedChanged;            

    public void ToggleInventory(bool isOpen)
    {
        OnInventoryToggled?.Invoke(isOpen);
    }

    public void ShowTooltip(string text)
    {
        OnShowTooltip?.Invoke(text);
    }

    public void HideTooltip()
    {
        OnHideTooltip?.Invoke();
    }

    public void HealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    public void ManaChanged(int current, int max)
    {
        OnManaChanged?.Invoke(current, max);
    }

    public void LevelUp(int newLevel)
    {
        OnLevelUp?.Invoke(newLevel);
    }

    public void DialogueLineShown(string line)
    {
        OnDialogueLineShown?.Invoke(line);
    }

    public void DialogueClosed()
    {
        OnDialogueClosed?.Invoke();
    }

    public void QuestTrackedChanged(int questIdOrIndex)
    {
        OnQuestTrackedChanged?.Invoke(questIdOrIndex);
    }
}
