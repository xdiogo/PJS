using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableConnector : MonoBehaviour
{
    public LayerMask layerMask = ~0;

    public GameObject placeHolder;

    public bool isDragging;

    private GameObject cableDraggin;

    // Start is called before the first frame update
    void Start()
    {
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        ClickCable();
        ReleaseCable();
        UpdateCable();
    }


    void ClickCable()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hasHit = Physics.Raycast(ray, out RaycastHit hit, 25, layerMask, QueryTriggerInteraction.Collide);

        if (!hasHit) return;

        isDragging = true;
        cableDraggin = hit.collider.gameObject;
    }

    void UpdateCable()
    {
        if (!Input.GetMouseButton(0)) return;

        if (!isDragging) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var point = ray.GetPoint(10);

        placeHolder.transform.position = new Vector3(point.x, point.y, cableDraggin.transform.position.z);

    }
    void ReleaseCable()
    {
        if (!Input.GetMouseButtonUp(0)) return;


    }

}
