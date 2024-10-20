using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager0 : MonoBehaviour
{
    [SerializeField] private string task1;
    [SerializeField] private string task2;

    [SerializeField] private TMP_Text taskText;

    public Inventory inventory;
    void Start()
    {
        inventory.OnItemAddTask += ObjectCheck;
        UpdateTaskText();
    }

    private void ObjectCheck(ScriptableObject task_item)
    {
        if (task_item.name == "Cube")
        {
            TaskClear(1);
        }
        else if (task_item.name == "Sphere")
        {
            TaskClear(2);
        }
    }
    private void UpdateTaskText()
    {
        taskText.text = $"<size=40>{task1}</size>\n<size=40>{task2}</size>";
    }
    public void UpdateTask(int taskNumber, string newTask)
    {
        if (taskNumber == 1)
        {
            task1 = newTask;
        }
        else if (taskNumber == 2)
        {
            task2 = newTask;
        }
        UpdateTaskText();
    }
    public void TaskClear(int taskNumber)
    {

        if (taskNumber == 1)
        {
            task1 = null;
        }
        else if(taskNumber == 2) 
        {
            task2 = null;
        }
        UpdateTaskText();
    }
}
