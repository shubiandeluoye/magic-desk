using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonColorAnimation : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private float gradientDuration = 1.5f;
    
    // Our color palette
    private readonly Color redColor = new Color(1f, 0.2f, 0.2f, 1f);      // #FF3333
    private readonly Color lightBlueColor = new Color(0.4f, 0.8f, 1f, 1f); // #66CCFF
    private readonly Color blueColor = new Color(0.2f, 0.4f, 1f, 1f);     // #3366FF
    
    private void Awake()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
    }
    
    public void PlayFlashAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        
        // Flash from current color to white and back
        sequence.Append(buttonImage.DOColor(Color.white, flashDuration * 0.5f));
        sequence.Append(buttonImage.DOColor(buttonImage.color, flashDuration * 0.5f));
        
        sequence.Play();
    }
    
    public void PlayGradientAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        
        // Cycle through our color palette
        sequence.Append(buttonImage.DOColor(redColor, gradientDuration * 0.33f));
        sequence.Append(buttonImage.DOColor(lightBlueColor, gradientDuration * 0.33f));
        sequence.Append(buttonImage.DOColor(blueColor, gradientDuration * 0.33f));
        sequence.SetLoops(-1, LoopType.Yoyo);
        
        sequence.Play();
    }
    
    public void StopAnimations()
    {
        DOTween.Kill(buttonImage);
    }
}
