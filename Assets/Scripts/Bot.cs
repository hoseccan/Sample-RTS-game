using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Bot.Task;

public class Bot : MonoBehaviour
{
    public Transform parking;
    public GameObject dust;
    public GameObject indicator;
    Vector3 lastLocation = Vector3.zero;
    NavMeshAgent agent;
    Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    [System.Serializable]
    public class Task
    {
        public enum TaskType { waiting, movement, loading, unloading, alert };
        public TaskType task;

        public Vector3 destination;
        public Building building;
        public Transform slot;
    }
    public List<Task> tasks = new List<Task>();

    public void SwitchIndicator()
    {
        if (indicator.activeSelf) indicator.SetActive(false);
        else indicator.SetActive(true);
    }
    private void Update()
    {
        CheckBehaviour();
    }
    private void FixedUpdate()
    {
        CheckVelocity();
    }
    void CheckVelocity()
    {
        if (Vector3.Distance(lastLocation, transform.position) > 0.05f)
        {
            animator.SetFloat("Velocity", 1f);
            dust.SetActive(true);
        }
        else
        {
            animator.SetFloat("Velocity", 0f);
            dust.SetActive(false);
        }
        lastLocation = transform.position;
    }
    private void CheckBehaviour()
    {
        if (tasks.Count != 0)
        {
            switch (tasks[0].task)
            {
                case TaskType.waiting:
                    WaitingBehaviour();
                    break;
                case TaskType.movement:
                    MovementsBehaviour();
                    break;
                case TaskType.loading:
                    ProcessBehaviour();
                    break;
                case TaskType.unloading:
                    ProcessBehaviour();
                    break;
                case TaskType.alert:
                    AlertBehaviour();
                    break;
            }
        }
        else
        {
            AddTask(TaskType.waiting, transform.position);
        }
    }
    public void AddTask(TaskType type, Vector3 destination)                // Movement, waiting
    {
        Task _task = new Task();
        _task.task = type;
        _task.destination = destination;
        tasks.Add(_task);
        if (GameLogic.I.bot == this) UIManager.I.UpdateUnitInfo(this);
    }
    public void AddTask(Factory factory)                                   // Unload on factory 
    {
        Task _task = new Task();
        _task.task = TaskType.unloading;
        _task.slot = factory.slot;
        _task.destination = factory.slot.position;
        _task.building = factory;
        tasks.Add(_task);
        if (GameLogic.I.bot == this) UIManager.I.UpdateUnitInfo(this);
    }
    public void AddTask(Storage storage)                                   // Upload on storage 
    {
        Task _task = new Task();
        _task.task = TaskType.loading;
        _task.slot = storage.slot;
        _task.destination = storage.slot.position;
        _task.building = storage;
        tasks.Add(_task);
        if (GameLogic.I.bot == this) UIManager.I.UpdateUnitInfo(this);
    }
    public void CancelCurrentTask()
    {
        tasks.RemoveAt(0);
        animator.SetBool("Open", false);
        if (GameLogic.I.bot == this && tasks.Count > 0) UIManager.I.UpdateUnitInfo(this);
    }
    public void CancelAllTask()
    {
        if (tasks[0].task == TaskType.loading || tasks[0].task == TaskType.unloading)
        {
            tasks[0].building.StopProcess();
        }
        animator.SetBool("Open", false);
        tasks.Clear();
    }
    void WaitingBehaviour()
    {

    }
    void MovementsBehaviour()
    {
        agent.SetDestination(tasks[0].destination);
        if (Vector3.Distance(transform.position, tasks[0].destination) < 0.5f)
        {
            CancelCurrentTask();
        }
    }
    void ProcessBehaviour()
    {
        if (tasks[0].building.IsFree()) agent.SetDestination(tasks[0].destination);

        if (Vector3.Distance(transform.position, tasks[0].destination) < 0.2f)
        {
            animator.SetBool("Open", true);
            transform.rotation = tasks[0].slot.rotation;
            tasks[0].building.bot = this;
            tasks[0].building.StarProcess();
        }
    }
    void AlertBehaviour()
    {
        agent.SetDestination(parking.position);
        if (Vector3.Distance(transform.position, parking.position) < 0.5f)
        {
            tasks[0].building.GetComponent<Parking>().CheckBotInside();
            animator.SetFloat("Velocity", 0f);
            dust.SetActive(false);
            enabled = false;
        }
    }

    #region Animation events
    public void EndProcess()
    {
        CancelCurrentTask();
    }
    #endregion
}
