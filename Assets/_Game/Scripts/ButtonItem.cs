using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private Color buttonColor;

    private int quantity;
    private bool isDragging = false;
    private GameObject draggingBlock;
    private Vector3 position;
    private Vector3 prevPosition;

    private void Start()
    {
        UpdateQuantity(10);
    }

    private void Update()
    {
        if (isDragging)
        {
            DragBlock();
        }

        //thu thap block khi an chuot phai
        else if (Input.GetMouseButtonDown(1))
        {
            CollectBlock();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDragging();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopDragging();
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void UpdateQuantity(int value)
    {
        quantity += value;
        quantityText.text = quantity.ToString();
    }

    public void StartDragging()
    {
        if (quantity > 0)
        {
            draggingBlock = Instantiate(blockPrefab);
            draggingBlock.GetComponent<MeshRenderer>().material.color = buttonColor; //gan color cho block
            isDragging = true;
        }
    }

    public void DragBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, blockLayer))
        {
            position = hit.point;
            if (hit.collider.gameObject.CompareTag("Block"))
            {
                GameObject hitBlock = hit.collider.gameObject;
                Color hitBlockColor = hitBlock.GetComponent<MeshRenderer>().material.color;

                //kiem tra cung mau thi cho xep chong
                if (hitBlockColor == buttonColor)
                {
                    position = hitBlock.transform.position + new Vector3(0, hit.normal.y, 0);
                }
                else
                {
                    position = hitBlock.transform.position + new Vector3(hit.normal.x, 0, hit.normal.z);
                }

                if (position != hitBlock.transform.position)
                {
                    draggingBlock.transform.position = position;
                }
            }
        }

        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            draggingBlock.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
        }
    }

    public void StopDragging()
    {
        if (isDragging)
        {
            DragBlock();
            if (position != prevPosition)
            {
                draggingBlock.transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
                prevPosition = draggingBlock.transform.position;
            }
            UpdateQuantity(-1);

            isDragging = false;
            draggingBlock = null;
        }
    }


    public void CollectBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject selectedBlock = hit.collider.gameObject;
            if (selectedBlock != null && selectedBlock.CompareTag("Block"))
            {
                Color blockColor = selectedBlock.GetComponent<MeshRenderer>().material.color;
                if (blockColor == buttonColor)
                {
                    Destroy(selectedBlock);
                    UpdateQuantity(1);
                }
            }
        }
    }
}
