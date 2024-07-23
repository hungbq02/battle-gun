using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expandable;

    private List<GameObject> freeList;
    private List<GameObject> usedList;

    private void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for (int i = 0; i < poolSize; ++i)
        {
            GenerateNewObject();
        }
    }

    public GameObject GetObject()
    {
        int totalFree = freeList.Count;
        /*if (freeList.Count == 0 && !expandable) return null;
        else if (freeList.Count == 0) GenerateNewObject();*/

        if (freeList.Count == 0)
        {
            if (!expandable) return null;
            GenerateNewObject();
        }

        GameObject g = freeList[totalFree - 1];
        freeList.RemoveAt(totalFree - 1);
        usedList.Add(g);
        return g;
    }

    public void ReturnObject(GameObject obj)
    {
        if (!usedList.Contains(obj))
        {
            Debug.LogWarning("Attempting to return an object that is not in the used list.");
        }
    //    Debug.Assert(usedList.Contains(obj));
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }
    private void GenerateNewObject()
    {
        GameObject g = Instantiate(prefab);
        g.transform.parent = transform;
        g.SetActive(false);
        freeList.Add(g);
    }

}

