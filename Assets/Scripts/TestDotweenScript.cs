using DG.Tweening;
using UnityEngine;

public class TestDotweenScript : MonoBehaviour
{
    public float Duration;
    public Transform GameObjectForMove;

    public Transform from;

    public Transform to;
    
    void Start()
    {
        var seq = DOTween.Sequence()
            .Append(GameObjectForMove.DOMove(to.position, Duration))
            .OnComplete(() => GameObjectForMove.DOMove(from.position, Duration));
        
        seq.Play();
    }
}
