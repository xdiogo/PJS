using System;
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

    private Cable cableDragging;
    private Vector3 defaultUp;

    // Novo: Evento para avisar que todos os cabos estão conectados
    public static Action OnAllCablesConnected;
    private int totalCables = 3; // Número total de cabos
    private int cablesConnected = 0;

    void Start()
    {
        isDragging = false;
    }

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

        if (cable.isConnected || cable.type == Cable.Type.OUT) return;

        isDragging = true;
        cableDragging = cable;
        defaultUp = cableDragging.transform.up;
    }

    void UpdateCable()
    {
        if (!isDragging || !Input.GetMouseButton(0)) return;

        var destination = GetMousePoint();
        placeHolder.transform.position = destination;

        var distance = Vector3.Distance(destination, cableDragging.transform.position);
        cableDragging.transform.localScale = new Vector3(1, distance * distanceFactor, 1);

        var direction = destination - cableDragging.transform.position;
        cableDragging.transform.up = direction;
    }

    void ReleaseCable()
    {
        if (!Input.GetMouseButtonUp(0)) return;

        var destination = GetMousePoint();
        Collider[] colliders = Physics.OverlapBox(destination, Vector3.one * .5f, Quaternion.identity, layerMask);

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Connector")) continue;

            Cable destinationCable = collider.transform.parent.parent.GetComponent<Cable>();

            if (destinationCable == cableDragging || !destinationCable.CanConnect(cableDragging)) continue;

            cableDragging.isConnected = true;
            destinationCable.isConnected = true;
            isDragging = false;

            cablesConnected++;
            Debug.Log($"Cabos conectados: {cablesConnected}/{totalCables}");

            // Verifica se todos os cabos estão conectados
            if (cablesConnected == totalCables)
            {
                Debug.Log("Todos os cabos conectados!");
                OnAllCablesConnected?.Invoke(); // Dispara o evento de todos conectados
            }
            return;
        }

        // Resetar cabo se não conectar
        cableDragging.transform.localScale = Vector3.one;
        cableDragging.transform.up = defaultUp;
        isDragging = false;
    }

    private Vector3 GetMousePoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var point = ray.GetPoint(10);
        return new Vector3(point.x, point.y, cableDragging.transform.position.z);
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
