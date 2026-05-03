using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Key";

    public void Pickup()
    {
        Debug.Log(itemName + " aufgehoben!");
        Destroy(gameObject);
    }
}
