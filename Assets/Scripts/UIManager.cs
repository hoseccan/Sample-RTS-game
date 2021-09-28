using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static Bot.Task;

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    public UnitsData unitInfo;

    public GameObject unitInfoPlate;
    public Text botName;
    public Text taskDescription;

    private void Awake()
    {
        I = this;
    }
    public void UpdateUnitInfo(Bot bot)
    {
        unitInfoPlate.SetActive(true);
        botName.text = bot.name;
        taskDescription.text = FindingDescription(bot);
    }
    string FindingDescription(Bot bot)
    {
        for (int i = 0; i < unitInfo.descriptions.Count; i++)
        {
            if (bot.tasks[0].task == unitInfo.descriptions[i].type) return unitInfo.descriptions[i]._description;
        }
        return "nothing to say";
    }
}

