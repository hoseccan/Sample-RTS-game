using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bot.Task;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitsData", order = 1)]
public class UnitsData : ScriptableObject
{
    [System.Serializable]
    public struct description
    {
        public TaskType type;
        public string _description;
    }
    public List<description> descriptions;
}
