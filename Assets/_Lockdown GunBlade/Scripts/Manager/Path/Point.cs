using UnityEngine;

public class Point : BaseMonoBehaviour
{
    [SerializeField] protected Point nextPoint;
    public Point NextPoint => nextPoint;
    int randomIndex;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRandomNextPoint();
    }

    public virtual void LoadRandomNextPoint()
    {
        if (this.nextPoint != null) return;

        do
        {
            randomIndex = Random.Range(0, transform.parent.childCount);
        } while (transform.parent.GetChild(randomIndex) == transform);

        Transform nextSibling = transform.parent.GetChild(randomIndex);
        this.nextPoint = nextSibling.GetComponent<Point>();
    }
}
