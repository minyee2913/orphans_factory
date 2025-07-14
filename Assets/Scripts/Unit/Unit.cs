using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer shadow;
    SpriteRenderer render;
    Animator animator;
    public float dirX, dirY;
    public float moveSpeed;
    public bool isMoving;
    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("dirX", dirX);
        animator.SetFloat("dirY", dirY);
        animator.SetBool("isMoving", isMoving);

        shadow.sprite = render.sprite;
    }

    public void Move(Vector2 dir)
    {
        Vector2 normalize = dir.normalized;
        dirX = normalize.x;
        dirY = normalize.y;
        transform.Translate(normalize * moveSpeed);
    }

    public void SetMoving(bool val)
    {
        isMoving = val;
    }
}
