using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    public GameObject box;
    public GameObject board;

    private Vector3 offset;
    private BoxCollider2D boxCollider;
    private BoxCollider2D boardCollider;

    private void Start()
    {
        boxCollider = box.GetComponent<BoxCollider2D>();
        boardCollider = board.GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        offset = board.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 targetPosition = GetMouseWorldPosition() + offset;
        Vector3 clampedPosition = ClampPositionInsideBox(targetPosition);
        board.transform.position = clampedPosition;
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
        Vector3 boardExtents = boardCollider.bounds.extents;

        float clampedX = Mathf.Clamp(position.x, boxMin.x + boardExtents.x, boxMax.x - boardExtents.x);
        float clampedY = Mathf.Clamp(position.y, boxMin.y + boardExtents.y, boxMax.y - boardExtents.y);

        return new Vector3(clampedX, clampedY, position.z);
    }

    
}
