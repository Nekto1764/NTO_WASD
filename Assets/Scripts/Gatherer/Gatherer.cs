using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Gatherer : MonoBehaviour
{
    public Transform assignedNode;
    public Transform gathererStructure;
    public float harvestAmountPerSecond = 5f;

    public float maxCapacity = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDepositing = false;
    private bool nodeExist = true;

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

        currentResourceType = ResourceType.None;
    }

    public void AppointAssignedNode()
    {
        Transform finderTransform = gathererStructure.GetChild(0);
        Finder finder = finderTransform.GetComponent<Finder>();

        List<GameObject> gameObjects = finder.GetObjectsInRadius();

        if (gameObjects.Count > 0)
        {
            assignedNode = gameObjects[0].transform;
        }
        else
        {
            nodeExist = false;
        }
    }

    void Update()
    {
        animator.SetBool("hasAssignedNode", assignedNode != null);
        animator.SetBool("isFull", CurrentCapacity >= maxCapacity);
        animator.SetBool("isNotEmpty", CurrentCapacity > 0);

        if (assignedNode != null && assignedNode.GetComponent<ResourceNode>().IsDepleted)
        {
            assignedNode = null;
            animator.SetBool("hasAssignedNode", false);
        }
        else if (assignedNode == null && nodeExist && !animator.GetBool("isNotEmpty"))
        {
            AppointAssignedNode();
        }
        else if (assignedNode == null && !animator.GetBool("atSupply"))
        {
            MoveTo(gathererStructure);
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
        assignedNode = null;
        animator.SetBool("hasAssignedNode", false);
        animator.SetBool("atNode", false);
    }

    public void Harvest()
    {
        if (assignedNode == null) return;

        if (assignedNode != null && !assignedNode.GetComponent<ResourceNode>().IsDepleted)
        {
            ResourceNode node = assignedNode.GetComponent<ResourceNode>();
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
            CurrentCapacity -= 2 * Time.deltaTime;

            if (CurrentCapacity < 0f)
            {
                CurrentCapacity = 0f;
            }

            yield return null;
        }

        HandleResourcesDeposit(tempCapacity, currentResourceType);

        isDepositing = false;

        if (assignedNode != null)
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
                ResourceManager.Instance.IncreaseResource(ResourceType.Wood, (int)tempCapacity*ResourceManager.Instance.woodCoefficient);
                return;
            case ResourceType.Stone:
                ResourceManager.Instance.IncreaseResource(ResourceType.Stone, (int)tempCapacity*ResourceManager.Instance.stoneCoefficient);
                return;
        }
    }

    private void OnTriggerEnter(Collider other) // Use onTriggerStay instead to check if the agent stopped moving.
    {
        if (assignedNode != null && other.transform == assignedNode)
        {
            animator.SetBool("atNode", true);
        }
        else if (gathererStructure != null && other.transform == gathererStructure)
        {
            animator.SetBool("atSupply", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (assignedNode != null && other.transform == assignedNode)
        {
            animator.SetBool("atNode", false);
        }
        else if (gathererStructure != null && other.transform == gathererStructure)
        {
            animator.SetBool("atSupply", false);
        }
    }
}