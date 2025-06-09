using UnityEngine;
using DG.Tweening;
public class TestCannonRotator : MonoBehaviour
{
    [SerializeField] private Transform cannon;
    [SerializeField] private float angleLimit = 50f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private SpriteRenderer cannonRenderer;

    private float currentDirection;
    private Tween rotationTween;

    public enum PlayerType { Player1, Player2 }

    public void Init(PlayerType playerType, Material material, string tagName, string layerName)
    {
        this.tag = tagName;
        this.gameObject.layer = LayerMask.NameToLayer(layerName);
        if (cannonRenderer != null)
            cannonRenderer.material = material;
    }

    public void StartRotation()
    {
        float startAngle = UnityEngine.Random.Range(-angleLimit, angleLimit);
        cannon.localRotation = Quaternion.Euler(0, 0, startAngle);
        currentDirection = UnityEngine.Random.value > 0.5f ? 1 : -1;
        StartRotating();
    }

    private void StartRotating()
    {
        if (rotationTween != null && rotationTween.IsActive())
            rotationTween.Kill();

        float currentZ = cannon.localEulerAngles.z;
        if (currentZ > 180) currentZ -= 360;
        float targetZ = currentZ + angleLimit * currentDirection;

        rotationTween = cannon.DOLocalRotate(new Vector3(0, 0, targetZ), duration)
            .SetEase(Ease.Linear)
            .OnUpdate(CheckDirectionChange)
            .OnComplete(StartRotating);
    }

    private void CheckDirectionChange()
    {
        float currentZ = cannon.localEulerAngles.z;
        if (currentZ > 180) currentZ -= 360;
        if (Mathf.Abs(currentZ) >= angleLimit)
        {
            currentDirection *= -1;
            rotationTween.Kill();
            StartRotating();
        }
    }
}
