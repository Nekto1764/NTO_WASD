using UnityEngine;

public class Initialization : MonoBehaviour
{
    public PlacementSystem placementSystem;
    public Vector3Int townHallPosition;
    private void Start()
    {
        placementSystem.PlaceStructureTo(townHallPosition);
    }
}
