using DG.Tweening;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Start()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }
}

