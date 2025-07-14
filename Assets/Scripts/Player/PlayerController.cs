using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Local { get; private set; }
    public Unit unit;
    public Product holding;
    public Dictionary<Product, float> nearest = new();
    void Awake()
    {
        Local = this;
    }

    Product Nearest()
    {
        Product product = null;
        float dist = 0;

        foreach (var pair in nearest)
        {
            if (product == null)
            {
                product = pair.Key;
                dist = pair.Value;
            }
            else
            {
                if (dist < pair.Value)
                {
                    product = pair.Key;
                    dist = pair.Value;
                }
            }
        }

        return product;
    }

    Product NearestBox()
    {
        Product product = null;
        float dist = 0;

        foreach (var pair in nearest)
        {
            if (pair.Key?.tag != "box")
            {
                continue;
            }

            if (product == null)
            {
                product = pair.Key;
                dist = pair.Value;
            }
            else
            {
                if (dist < pair.Value)
                {
                    product = pair.Key;
                    dist = pair.Value;
                }
            }
        }

        return product;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (holding == null)
            {
                Product n = NearestBox();
                if (n != null)
                {
                    if (n.tag == "box")
                    {
                        Box box = n.GetComponent<Box>();
                        if (box != null)
                        {
                            if (box.isOpened)
                            {
                                box.Close();
                            }
                            else
                            {
                                box.Open();
                            }
                        }
                        return;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (holding != null)
            {
                holding.transform.DOMove(unit.transform.position + new Vector3(unit.dirX, unit.dirY), 0.2f).SetEase(Ease.OutExpo);
                holding.isHolding = false;
                holding = null;
            }
            else
            {
                Product n = Nearest();
                if (n != null)
                {
                    holding = n;
                    n.isHolding = true;
                    nearest.Remove(n);
                }
            }
        }

        if (holding != null)
        {
            holding.transform.position = Vector2.Lerp(holding.transform.position, (Vector2)unit.transform.position + new Vector2(0, 1), 6 * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        bool isMoving = Mathf.Abs(dir.x) + Mathf.Abs(dir.y) != 0;

        if (unit != null)
        {
            unit.SetMoving(isMoving);

            if (isMoving)
                unit.Move(dir * Time.fixedDeltaTime);
        }
    }
}
