using UnityEngine;

public class ItemDropScript : MonoBehaviour
{
    public GameObject[] listItemDrop;
    int valueRandom;
    public void DropItem(Vector3 posDrop)
    {
        posDrop = transform.position;
        Debug.Log("Drop");
        valueRandom = Random.Range(0, 2);
        Instantiate(listItemDrop[valueRandom], posDrop, Quaternion.identity);
    }
}
