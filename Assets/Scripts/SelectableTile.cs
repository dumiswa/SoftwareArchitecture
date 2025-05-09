using UnityEngine;

//This class is attached to each selectable tile to set/get the "isTaken" value
//"isTaken = true" value means that the tower has been placed on that tile
public class SelectableTile : MonoBehaviour
{
    public bool isTaken = false;
}
