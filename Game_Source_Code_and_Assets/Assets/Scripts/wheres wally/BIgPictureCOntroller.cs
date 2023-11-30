using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIgPictureCOntroller : MonoBehaviour
{
    private Vector3 correctholePosition = new Vector3(-3.67f, 2.75f, 0);

    public GameObject box;
    public GameObject picture;

    private Vector3 offset;
    private BoxCollider2D boxCollider;
    private BoxCollider2D pictureCollider;

    private void Start()
    {
        boxCollider = box.GetComponent<BoxCollider2D>();
        pictureCollider = picture.GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        offset = picture.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 targetPosition = GetMouseWorldPosition() + offset;
        Vector3 clampedPosition = ClampPositionInsideBox(targetPosition);
        picture.transform.position = clampedPosition;

        // if near the target, auto find
        Vector3 circlepointlocalPosition = picture.transform.localPosition;
        float distance = Vector3.Distance(circlepointlocalPosition, correctholePosition);
        if (distance < 0.5f){
            picture.transform.localPosition = correctholePosition;

        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private Vector3 ClampPositionInsideBox(Vector3 position)
    {
        Vector3 boxMin = boxCollider.bounds.min;
        Vector3 boxMax = boxCollider.bounds.max;
        Vector3 pictureExtents = pictureCollider.bounds.extents;

        float clampedX = Mathf.Clamp(position.x, boxMin.x + pictureExtents.x, boxMax.x - pictureExtents.x);
        float clampedY = Mathf.Clamp(position.y, boxMin.y + pictureExtents.y, boxMax.y - pictureExtents.y);

        return new Vector3(clampedX, clampedY, position.z);
    }
}
