using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parking : Building
{
    public List<Bot> bots;
    int parkedBots = 0;

    public void CheckBotInside()
    {
        parkedBots++;
        print(parkedBots);
        if (parkedBots == bots.Count)
        {
            animator.SetBool("Open", false);
        }
    }

    public void SetAllert()
    {
        animator.SetBool("Open", true);
        foreach (Bot bot in bots)
        {
            if (bot.tasks[0].task == Bot.Task.TaskType.waiting) bot.CancelCurrentTask();
            bot.AddTask(Bot.Task.TaskType.alert, Vector3.zero);
            bot.tasks[bot.tasks.Count - 1].building = this;
            if (bot.tasks.Count > 1)
            {
                Bot.Task alertTask = bot.tasks[bot.tasks.Count - 1];
                bot.tasks.Insert(0, alertTask);
                bot.tasks.RemoveAt(bot.tasks.Count - 1);
            }
        }
    }
    public void UnsetAllert()
    {
        animator.SetBool("Open", true);
        foreach (Bot bot in bots)
        {
            bot.enabled = true;
            bot.CancelCurrentTask();
        }
        parkedBots = 0;
    }
}
