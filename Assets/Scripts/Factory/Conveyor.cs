using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
class Conveying
{
    public Product product;
    public List<Vector2> points;
    public int pointIndex;
}
public class Conveyor : MonoBehaviour
{
    public Vector2 startPoint, endPoint;
    public List<Vector2> middles = new();
    public Conveyor next;
    public bool is45;
    public List<Product> products = new();
    [SerializeField]
    List<Conveying> conveyings = new();
    List<Conveying> destroyedConvey = new();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Product product in products)
        {
            List<Vector2> points = new()
            {
                startPoint,
            };
            foreach (Vector2 mid in middles)
            {
                points.Add(mid);
            }
            points.Add(endPoint);

            product.Set45(is45);
            product.onConvey = true;

            conveyings.Add(new Conveying
            {
                product = product,
                pointIndex = 0,
                points = points,
            });
        }

        products.Clear();

        Vector2 target;

        foreach (Conveying conveying in conveyings)
        {
            if (conveying.product.isHolding)
            {
                destroyedConvey.Add(conveying);
                
                conveying.product.Set45(false);
                conveying.product.onConvey = false;

                break;
            }
            target = (Vector2)transform.position + conveying.points[conveying.pointIndex] + conveying.product.offset;
            conveying.product.transform.position = Vector2.MoveTowards(conveying.product.transform.position, target, 1.5f * Time.deltaTime);

            if (Vector2.Distance(conveying.product.transform.position, target) <= 0.05f)
            {
                conveying.pointIndex++;

                if (conveying.pointIndex >= conveying.points.Count)
                {
                    destroyedConvey.Add(conveying);

                    if (next != null)
                    {
                        next.products.Add(conveying.product);
                    }
                    else
                    {
                        conveying.product.Set45(false);
                        conveying.product.onConvey = false;
                    }
                }
            }
        }

        foreach (Conveying conveying in destroyedConvey)
        {
            conveyings.Remove(conveying);
        }
        destroyedConvey.Clear();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Product product = collision.transform.GetComponent<Product>();

        if (product != null)
        {
            if (!product.onConvey && !product.isHolding)
            {
                products.Add(product);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (middles.Count > 0)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(startPoint + (Vector2)transform.position, middles[0] + (Vector2)transform.position);

            Vector2 last = middles[0];

            foreach (Vector2 point in middles)
            {
                Gizmos.DrawLine(last + (Vector2)transform.position, point + (Vector2)transform.position);

                last = point;
            }

            Gizmos.DrawLine(last + (Vector2)transform.position, endPoint + (Vector2)transform.position);
        }
        else
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(startPoint + (Vector2)transform.position, endPoint + (Vector2)transform.position);
        }

        if (next != null)
        {
            Gizmos.DrawLine(endPoint + (Vector2)transform.position, next.startPoint + (Vector2)next.transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPoint + (Vector2)transform.position, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint + (Vector2)transform.position, 0.05f);

        if (middles.Count > 0)
        {
            foreach (Vector2 point in middles)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(endPoint + (Vector2)transform.position, 0.02f);
            }
        }

#if UNITY_EDITOR
        Handles.Label(startPoint + (Vector2)transform.position + new Vector2(0, 0.05f), "Start");
        Handles.Label(endPoint + (Vector2)transform.position + new Vector2(0, 0.05f), "End");
#endif
    }
}
