using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickRipple : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ParticleSystem rippleEffect;
    [SerializeField] private float rippleSize = 1f;
    [SerializeField] private float rippleDuration = 0.5f;
    [SerializeField] private Color rippleColor = new Color(1f, 1f, 1f, 0.5f);

    private void Awake()
    {
        if (rippleEffect == null)
        {
            // Create particle system if not assigned
            rippleEffect = GetComponentInChildren<ParticleSystem>();
            if (rippleEffect == null)
            {
                GameObject rippleObj = new GameObject("RippleEffect");
                rippleObj.transform.SetParent(transform);
                rippleEffect = rippleObj.AddComponent<ParticleSystem>();
                SetupParticleSystem();
            }
        }
    }

    private void SetupParticleSystem()
    {
        var main = rippleEffect.main;
        main.duration = rippleDuration;
        main.startLifetime = rippleDuration;
        main.startSize = rippleSize;
        main.startColor = rippleColor;
        
        var shape = rippleEffect.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;
        
        var emission = rippleEffect.emission;
        emission.rateOverTime = 0;
        emission.SetBurst(0, new ParticleSystem.Burst(0f, 1));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rippleEffect != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            rippleEffect.transform.localPosition = localPoint;
            rippleEffect.Play();
        }
    }
}
