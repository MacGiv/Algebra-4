using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AABB : MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] private Vector3[] vertices;
    [SerializeField] private Vector3[] boxVertices;
    [SerializeField] Vector3 minV;
    [SerializeField] Vector3 maxV;

    [SerializeField] FrustrumCulling frustrumCulling;

    public Vector3[] BoxVertices
    {
        get { return boxVertices; }
    }

    private void Start()
    {
        minV = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        maxV = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        //meshFilter = GetComponentInChildren<MeshFilter>();

        boxVertices = new Vector3[8];
        vertices = meshFilter.mesh.vertices;
        SearchVertex();
    }

    private void Update()
    {
        SearchVertex();
        SetVertices();
    }

    private void SearchVertex()
    {
        minV = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        maxV = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        for (int i = 0; i < vertices.Length; i++)
        {
            minV = Vector3.Min(minV, transform.TransformPoint(vertices[i]));
            maxV = Vector3.Max(maxV, transform.TransformPoint(vertices[i]));
        }
    }

    private void SetVertices()
    {
        Vector3 halfSize = GetSize() / 2;
        Vector3 centerV = GetCenter();

        boxVertices[0] = centerV + -halfSize;
        boxVertices[1] = centerV + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
        boxVertices[2] = centerV + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
        boxVertices[3] = centerV + new Vector3(halfSize.x, halfSize.y, halfSize.z);

        boxVertices[4] = centerV + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
        boxVertices[5] = centerV + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        boxVertices[6] = centerV + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        boxVertices[7] = centerV + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
    }

    public Vector3 GetCenter()
    {
        return (minV + maxV) / 2;
    }

    public Vector3 GetSize()
    {
        return maxV - minV;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GetCenter(), GetSize());
        Gizmos.color = Color.red;
        for (int i = 0; i < boxVertices.Length; i++)
        {
            Gizmos.DrawSphere(boxVertices[i], 0.01f);
        }
    }
}