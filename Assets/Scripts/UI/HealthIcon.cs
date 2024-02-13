using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour
{
    [SerializeField] private Image activeIcon;
   
    
    public void SetIcon(bool active, bool animate)
    {
        if(animate)
            AnimateIcon();
        else
            SetStatic(active);
    }

    private void SetStatic(bool active)
    {
        activeIcon.gameObject.SetActive(active);
    }

    private Sequence animationSequence;
    private void AnimateIcon()
    {
        float duration = 0.2f;
        if(animationSequence != null &&animationSequence.IsPlaying()) return;
        animationSequence = DOTween.Sequence();
        

        animationSequence.Append(activeIcon.transform.DOLocalMoveY(2, duration / 2).SetEase(Ease.OutQuad));
        animationSequence.Append(activeIcon.transform.DOMoveY(0, 0.1f).SetEase(Ease.InQuad));


        animationSequence.SetLoops(1, LoopType.Restart);
    
    }

}