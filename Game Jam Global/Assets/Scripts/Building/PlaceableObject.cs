using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceableObject : MonoBehaviour
{
public bool Placed {  get; private set; }
public Vector3 Size { get; private set; }
    private Vector3[] Vertices;

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }
        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), 
                                Math.Abs((vertices[0] - vertices[3]).y),
                                1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        Size = new Vector3(Size.y,Size.x,1);

        Vector3[] vertices = new Vector3[Vertices.Length];

        for(int i = 0;i< Vertices.Length; i++)
        {
            vertices[i] = Vertices[(i + 1) % Vertices.Length];
        }
        Vertices = vertices;
    }
    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;



        //invoke events of placement
    }
}
