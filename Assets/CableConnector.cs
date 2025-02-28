using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CableConnector : MonoBehaviour
{
    private Dictionary<GameObject, int> cablesPerSocket = new Dictionary<GameObject, int>();

    [Header("Settings")]
    public LayerMask layerMask = ~0;
    public GameObject placeHolder;
    public float distanceFactor = 0.7f;
    public int totalCables = 3;

    private Cable cableDragging;
    private Vector3 defaultUp;
    private bool isDragging;
    private int cablesConnected = 0;

    // Evento para avisar que todos os cabos est�o conectados
    public Action OnAllCablesConnected;
    public Action OnCancel;
    public UnityEvent thingsToDoAfter;
    void Update()
    {
        HandleCableInteraction();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel.Invoke();
        }
    }

    void HandleCableInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickCable();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            DragCable();
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            ReleaseCable();
        }
    }

    void TryPickCable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 25, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Connector"))
            {
                Cable cable = hit.collider.transform.parent.parent.GetComponent<Cable>();
                if (!cable.isConnected && cable.type != Cable.Type.OUT)
                {
                    cableDragging = cable;
                    defaultUp = cableDragging.transform.up;
                    isDragging = true;
                }
            }
        }
    }

    void DragCable()
    {
        Vector3 destination = GetMousePoint();
        placeHolder.transform.position = destination;

        float distance = Vector3.Distance(destination, cableDragging.transform.position);
        cableDragging.transform.localScale = new Vector3(1, distance * distanceFactor, 1);

        Vector3 direction = destination - cableDragging.transform.position;
        cableDragging.transform.up = direction;
    }

    void ReleaseCable()
    {
        Vector3 destination = GetMousePoint();
        Collider[] colliders = Physics.OverlapBox(destination, Vector3.one * 0.5f, Quaternion.identity, layerMask);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Connector"))
            {
                Cable targetCable = collider.transform.parent.parent.GetComponent<Cable>();
                if (targetCable != cableDragging && targetCable.CanConnect(cableDragging))
                {
                    ConnectCables(targetCable);
                    return;
                }
            }
        }
    }

    void ConnectCables(Cable targetCable)
    {
        cableDragging.isConnected = true;
        targetCable.isConnected = true;
        isDragging = false;

        // Identificar a tomada (socket)
        GameObject socket = targetCable.transform.parent.gameObject;

        if (!cablesPerSocket.ContainsKey(socket))
        {
            cablesPerSocket[socket] = 0;
        }

        cablesPerSocket[socket]++;
        Debug.Log($"Cabos conectados na tomada {socket.name}: {cablesPerSocket[socket]}/{totalCables}");

        // Verificar se todos os cabos dessa tomada est�o conectados
        if (cablesPerSocket[socket] >= totalCables)
        {
            Debug.Log($"Todos os cabos conectados na tomada {socket.name}!");
            thingsToDoAfter.Invoke();
            OnAllCablesConnected?.Invoke();
        }
    }

    Vector3 GetMousePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.GetPoint(10);
        return new Vector3(point.x, point.y, cableDragging.transform.position.z);
    }

    private void OnDrawGizmos()
    {
        if (isDragging)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(GetMousePoint(), Vector3.one * 0.5f);
        }
    }
}