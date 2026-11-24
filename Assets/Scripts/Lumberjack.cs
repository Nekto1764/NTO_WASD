using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Lumberjack : MonoBehaviour
{
    public Transform assignedTree;
    public Transform sawmill;
    public float harvestAmountPerSecond = 5f;

    public float maxCapacity = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDepositing = false;
    private bool treesExist = true;

    public Slider capacitySlider;

    private float _currentCapacity;

    public ResourceType currentResourceType;

    public float CurrentCapacity
    {
        get => _currentCapacity;
        set
        {
            _currentCapacity = Mathf.Clamp(value, 0f, maxCapacity);
            UpdateSlider();
        }
    }

    private void UpdateSlider()
    {
        if (capacitySlider != null)
        {
            float normalized = CurrentCapacity / maxCapacity;
            float scaled = normalized * 10f;
            capacitySlider.value = Mathf.Round(scaled);
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentResourceType = ResourceType.Wood;
    }

    public void AppointAssignedTree()
    {
        Transform treesFinder = sawmill.GetChild(0);
        Debug.Log(treesFinder);
        FindTrees findTrees = treesFinder.GetComponent<FindTrees>();

        List<GameObject> gameObjects = findTrees.GetObjectsInRadius();

        if (gameObjects.Count > 0)
        {
            assignedTree = gameObjects[0].transform;
        }
        else
        {
            treesExist = false;
        }
    }

    void Update()
    {
        animator.SetBool("hasAssignedTree", assignedTree != null);
        animator.SetBool("isFull", CurrentCapacity >= maxCapacity);
        animator.SetBool("isNotEmpty", CurrentCapacity > 0);

        if (assignedTree != null && assignedTree.GetComponent<ResourceNode>().IsDepleted)
        {
            assignedTree = null;
            animator.SetBool("hasAssignedTree", false);
        }
        else if (assignedTree == null && treesExist)
        {
            AppointAssignedTree();
        }
    }

    public void MoveTo(Transform target)
    {
        if (target == null) return;
        
        UnitMovement unitMovement = GetComponent<UnitMovement>();

        if (unitMovement != null)
        {
            unitMovement.MoveTo(target.position, false);
        }
    }

    public void CancelHarvesting()
    {
        assignedTree = null;
        animator.SetBool("hasAssignedTree", false);
        animator.SetBool("atTree", false);
    }

    public void Harvest()
    {
        if (assignedTree == null) return;

        if (assignedTree != null && !assignedTree.GetComponent<ResourceNode>().IsDepleted)
        {
            ResourceNode node = assignedTree.GetComponent<ResourceNode>();
            currentResourceType = node.resourceType;

            node.Harvest(harvestAmountPerSecond * Time.deltaTime);
            CurrentCapacity += harvestAmountPerSecond * Time.deltaTime;

            if (CurrentCapacity > maxCapacity)
            {
                CurrentCapacity = maxCapacity;
            }
        }
    }

    public void DepositResources()
    {
        if (isDepositing) return;

        isDepositing = true;
   
        StartCoroutine(WaitInDepositingState());
    }

    private System.Collections.IEnumerator WaitInDepositingState()
    {
        float tempCapacity = CurrentCapacity;
       
        while (CurrentCapacity > 0f)
        {
            CurrentCapacity -= 1 * Time.deltaTime;

            if (CurrentCapacity < 0f)
            {
                CurrentCapacity = 0f;
            }

            yield return null;
        }

        HandleResourcesDeposit(tempCapacity, currentResourceType);

        isDepositing = false;

        if (assignedTree != null)
        {
             animator.SetTrigger("doneDepositing");
        }

        currentResourceType = ResourceType.None;

    }

    private void HandleResourcesDeposit(float tempCapacity, ResourceType currentResourceType)
    {
        switch (currentResourceType)
        {
            case ResourceType.Wood:
                ResourceManager.Instance.IncreaseResource(ResourceType.Wood, (int)tempCapacity);
                return;
        }
    }

    private void OnTriggerEnter(Collider other) // Use onTriggerStay instead to check if the agent stopped moving.
    {
        if (assignedTree != null && other.transform == assignedTree)
        {
            animator.SetBool("atTree", true);
        }
        else if (sawmill != null && other.transform == sawmill)
        {
            animator.SetBool("atSupply", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (assignedTree != null && other.transform == assignedTree)
        {
            animator.SetBool("atTree", false);
        }
        else if (sawmill != null && other.transform == sawmill)
        {
            animator.SetBool("atSupply", false);
        }
    }
}