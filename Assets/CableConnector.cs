using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CableConnector : MonoBehaviour
{
    public LayerMask layerMask = ~0;

    public GameObject placeHolder;

    public bool isDragging;

    public float distanceFactor = 0.7f;

    private Cable cableDraggin;

    private Vector3 defaultUp;
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

        if (!hit.collider.CompareTag("Connector")) return;

        var cable = hit.collider.transform.parent.parent.GetComponent<Cable>();

        if (cable.isConnected) return;

        if (cable.type == Cable.Type.OUT) return;

        isDragging = true;
        cableDraggin = cable;
        defaultUp = cableDraggin.transform.up;
    }

    void UpdateCable()
    {
        if (!Input.GetMouseButton(0)) return;

        if (!isDragging) return;

        var destination = GetMousePoint();
        placeHolder.transform.position = destination;

        var distance = Vector3.Distance(destination, cableDraggin.transform.position);
        cableDraggin.transform.localScale = new Vector3(1, distance * distanceFactor, 1);

        var direction = destination - cableDraggin.transform.position;

        cableDraggin.transform.up = direction;

    }
    void ReleaseCable()
    {
        if (!Input.GetMouseButtonUp(0)) return;


        var destination = GetMousePoint();

        Collider[] colliders = Physics.OverlapBox(destination, Vector3.one * .5f, Quaternion.identity, layerMask);


        foreach (Collider collider in colliders)
        {
            if (!collider.CompareTag("Connector")) continue;

            Cable destinationCable = collider.transform.parent.parent.GetComponent<Cable>();

            if(destinationCable == cableDraggin) continue;

            if (!destinationCable.CanConnect(cableDraggin)) continue;

            cableDraggin.isConnected = true;
            destinationCable.isConnected = true;
            isDragging = false;

            return;
        }


        cableDraggin.transform.localScale = Vector3.one;
        cableDraggin.transform.up = defaultUp;

    }

    private Vector3 GetMousePoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var point = ray.GetPoint(10);

        return new Vector3(point.x, point.y, cableDraggin.transform.position.z);
    }

    private void OnDrawGizmos()
    {
        if (isDragging)
        {
            var destination = GetMousePoint();
            Gizmos.DrawCube(destination, Vector3.one * .5f);
        }
    }

}
