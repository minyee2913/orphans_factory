using UnityEngine;

public class Product : MonoBehaviour
{
    public string Id;
    protected SpriteRenderer render;
    [HideInInspector]
    public Vector2 defScale;
    const float nearDist = 2.5f;
    public Vector2 offset;
    public Sprite dig0, dig45;
    public bool onConvey, isHolding;
    protected bool spriteForced;
    protected Sprite CachedSprite;

    void Start()
    {
        render = GetComponent<SpriteRenderer>();

        defScale = transform.localScale;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, PlayerController.Local.unit.transform.position);
        if (dist <= nearDist)
        {
            if (PlayerController.Local.holding != this)
            {
                PlayerController.Local.nearest[this] = dist;
            }
        }
        else
        {
            if (PlayerController.Local.nearest.ContainsKey(this))
            {
                PlayerController.Local.nearest.Remove(this);
            }
        }
    }

    public void Set45(bool val)
    {
        if (spriteForced)
        {
            if (val)
                CachedSprite = dig45;
            else
                CachedSprite = dig0;
            return;
        }

        if (val)
            render.sprite = dig45;
        else
            render.sprite = dig0;
    }
}
