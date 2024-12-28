using UnityEngine;
using DG.Tween;

public class MagicBookIntroController : MonoBehaviour
{
    [SerializeField] private RectTransform bookTransform;
    [SerializeField] private float animationDuration = 1.5f;
    [SerializeField] private float rotationAngle = 360f;
    [SerializeField] private Vector2 startPosition = new Vector2(-1000f, 0f);
    [SerializeField] private Vector2 endPosition = Vector2.zero;
    
    private void Awake()
    {
        if (bookTransform == null)
            bookTransform = GetComponent<RectTransform>();
    }

    public void PlayIntroAnimation()
    {
        // Reset position and rotation
        bookTransform.anchoredPosition = startPosition;
        bookTransform.localRotation = Quaternion.identity;
        
        // Create animation sequence
        Sequence sequence = DOTween.Sequence();
        
        // Move from off-screen to center while rotating
        sequence.Append(bookTransform.DOAnchorPos(endPosition, animationDuration)
            .SetEase(Ease.OutBack));
        sequence.Join(bookTransform.DORotate(new Vector3(0, 0, rotationAngle), animationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad));
            
        sequence.Play();
    }

    public void PlayOutroAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        
        // Rotate and move off-screen
        sequence.Append(bookTransform.DOAnchorPos(startPosition, animationDuration)
            .SetEase(Ease.InBack));
        sequence.Join(bookTransform.DORotate(new Vector3(0, 0, -rotationAngle), animationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InQuad));
            
        sequence.Play();
    }
}
