using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Sprite xSprite;
    [SerializeField] private SpriteRenderer markerSpriteRenderer;

    public enum MarkerType
    {
        None,
        O,
        X
    }

    public delegate void OnBlockClicked(int index);

    private OnBlockClicked _onBlockClicked;

    private int _blockIndex;
    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void InitMarker(int blockIndex, OnBlockClicked onBlockClicked)
    {
        _blockIndex = blockIndex;
        SetMarker(MarkerType.None);
        this._onBlockClicked = onBlockClicked;
        SetColor(_defaultColor);
    }

    public void SetMarker(MarkerType markerType)
    {
        switch (markerType)
        {
            case MarkerType.O:
                markerSpriteRenderer.sprite = oSprite;
                break;
            case MarkerType.X:
                markerSpriteRenderer.sprite = xSprite;
                break;
            case MarkerType.None:
                markerSpriteRenderer.sprite = null;
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        _onBlockClicked?.Invoke(_blockIndex);
    }
}
