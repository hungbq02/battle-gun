using UnityEngine;

public class LineFade : MonoBehaviour
{
    [SerializeField] private Color color;

    [SerializeField] private float speed = 10f;

    LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        color.a = 1;
        lr.startColor = color;
        lr.endColor = color;
    }

    void Update()
    {
        // move towards zero
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);

        // update color
        //lr.SetColors (color, color);
        lr.startColor = color;
        lr.endColor = color;
    }

}
