using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic I;
    public Bot bot;

    private void Awake()
    {
        I = this;
    }
    private void Update()
    {
        RayCastOnClik();
    }
    void RayCastOnClik()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 200f);

            if (!hit.collider)
            {
                if (bot)
                {
                    bot.SwitchIndicator();
                    UIManager.I.unitInfoPlate.SetActive(false);
                }
                bot = null;
                UIManager.I.unitInfoPlate.SetActive(false);
            }
            if (hit.collider.CompareTag("Bot"))  
            {
                if (bot) bot.SwitchIndicator();
                print("Find bot");
                bot = hit.collider.gameObject.GetComponent<Bot>();
                bot.SwitchIndicator();
                UIManager.I.unitInfoPlate.SetActive(true);
                UIManager.I.UpdateUnitInfo(bot);
            }
            else if (bot)
            {
                bot.SwitchIndicator();
                bot = null;
                UIManager.I.unitInfoPlate.SetActive(false);
            }
        }
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift) && bot.enabled)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 200f);

            if (!hit.collider)
            {
                return;
            }
            if (hit.collider.CompareTag("Ground") && bot != null)
            {
                if (bot.tasks[0].task == Bot.Task.TaskType.waiting) bot.CancelCurrentTask();
                bot.AddTask(Bot.Task.TaskType.movement, hit.point);
                return;
            }
            if (hit.collider.CompareTag("Storage") && bot != null)
            {
                if (bot.tasks[0].task == Bot.Task.TaskType.waiting) bot.CancelCurrentTask();
                bot.AddTask(hit.collider.GetComponent<Storage>());
                return;
            }
            if (hit.collider.CompareTag("Factory") && bot != null)
            {
                if (bot.tasks[0].task == Bot.Task.TaskType.waiting) bot.CancelCurrentTask();
                bot.AddTask(hit.collider.GetComponent<Factory>());
                return;
            }
            if (bot)
            {
                bot.CancelAllTask();
            }
        }
        else if (Input.GetMouseButtonDown(1) && bot.enabled)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 200f);

            if (!hit.collider)
            {
                return;
            }
            if (hit.collider.CompareTag("Ground") && bot != null)
            {
                bot.CancelAllTask();
                bot.AddTask(Bot.Task.TaskType.movement, hit.point);
                return;
            }
            if (hit.collider.CompareTag("Storage") && bot != null)
            {
                if (bot.tasks[0].building && bot.tasks[0].building.GetComponent<Storage>() == hit.collider.GetComponent<Storage>())
                {
                    return;
                }
                bot.CancelAllTask();
                bot.AddTask(hit.collider.GetComponent<Storage>());
                return;
            }
            if (hit.collider.CompareTag("Factory") && bot != null)
            {
                if (bot.tasks[0].building && bot.tasks[0].building.GetComponent<Factory>() == hit.collider.GetComponent<Factory>())
                {
                    return;
                }
                bot.CancelAllTask();
                bot.AddTask(hit.collider.GetComponent<Factory>());
                return;
            }
            if (bot)
            {
                bot.CancelAllTask();
            }
        }
    }
}
