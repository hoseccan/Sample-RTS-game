using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Bot bot;
    public GameObject goods;
    public Transform slot;
    public Animator animator;
    public void StarProcess()
    {
        animator.SetBool("Start", true);
    }
    public void EndProcess()
    {
        bot.CancelCurrentTask();
        animator.SetBool("Start", false);
    }
    public void StopProcess()
    {
        if (bot && bot.tasks[0].task == Bot.Task.TaskType.unloading) //Factory
        {
            AnimatorStateInfo clipInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (clipInfo.IsName("Process start"))
            {
                HideGoods();
                animator.SetBool("Start", false);
            }
        }
        if (bot && bot.tasks[0].task == Bot.Task.TaskType.loading) //Storage
        {
            AnimatorStateInfo clipInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (clipInfo.IsName("Process start") || clipInfo.IsName("Process end"))
            {
                animator.SetBool("Start", false);
                animator.SetBool("Declien",true);
            }
        }
    }
    public bool IsFree()
    {
        Collider[] hitColliders = Physics.OverlapSphere(slot.position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Bot") && Vector3.Distance(hitCollider.transform.position, slot.position) < 2f) return false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return true;
        else return false;
    }

    #region Animation events
    public void ShowGoods()
    {
        if (animator.GetBool("Start"))
        {
            goods.SetActive(true);
        }
    }
    public void HideGoods()
    {
        goods.SetActive(false);
    }
    public void ResetDeclien()
    {
        animator.SetBool("Declien", false);
    }
    #endregion
}
