using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum Direction
{
    Left,
    Right,
}
[RequireComponent(typeof(Image))]
public class AlphaSlise : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float minAlpha, maxAlpha;
    [SerializeField] private float speedSlise;
    private Image image;
    private Direction direction;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        direction = Direction.Right;
    }
    private void FixedUpdate()
    {
        if (direction == Direction.Right)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + speedSlise);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - speedSlise);
        }
        if (image.color.a <= minAlpha)
        {
            direction = Direction.Right;
        }
        if (image.color.a >= maxAlpha)
        {
            direction = Direction.Left;
        }

    }
}
