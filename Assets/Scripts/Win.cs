using UnityEngine;

public class Win : MonoBehaviour
{
    public GameObject toMenu;
    public GameObject house;

    private void Update()
    {
        if (house == null)
        {
            toMenu.SetActive(true);
        }
    }
}
