using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Box : Product
{
    BoxCollider2D boxCollider;
    public Sprite openSprite;
    public Vector2 openScale;
    public bool isOpened;
    
    public List<Product> productsInBox = new List<Product>();
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        CachedSprite = render.sprite;
        render.sprite = openSprite;
        transform.DOScale(openScale, 0.2f).SetEase(Ease.InBounce);

        productsInBox.ForEach((v) =>
        {
            v.transform.SetParent(null);
            v.transform.localScale = Vector2.one;
            v.gameObject.SetActive(true);
        });

        isOpened = true;
        spriteForced = true;
    }

    public void Close()
    {
        render.sprite = CachedSprite;
        transform.DOScale(defScale, 0.2f).SetEase(Ease.InOutExpo);

        productsInBox.ForEach((v) =>
        {
            v.transform.SetParent(transform);
            v.gameObject.SetActive(false);
        });

        isOpened = false;
        spriteForced = false;
    }

    void FixedUpdate()
    {
        if (isOpened && isHolding)
        {
            Close();
        }

        if (isOpened)
        {
            productsInBox.Clear();

            Vector2 center = transform.TransformPoint(boxCollider.offset);
            Vector2 halfExtents = Vector2.Scale(boxCollider.size, transform.lossyScale) * 0.5f;

            // 충돌 감지
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, halfExtents, 0);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("product"))
                {
                    Product p = hit.GetComponent<Product>();
                    if (p != null && !productsInBox.Contains(p))
                    {
                        productsInBox.Add(p);
                    }
                }
            }
        }
    }
}
