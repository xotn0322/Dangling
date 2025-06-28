using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterMovementController : MonoBehaviour
{
    public int columnCount = 50;
    public float width = 10f;
    public float height = 1f;
    public float k = 0.025f;
    public float m = 0f; // 초기 속도
    public float drag = 0.05f;
    public float spread = 0.05f;

    private List<WaterColumn> columns = new List<WaterColumn>();
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        Setup();
    }

    private void Setup()
    {
        columns.Clear();
        float space = width / columnCount;
        for (int i = 0; i <= columnCount; i++)
        {
            float xPos = i * space - width * 0.5f;
            columns.Add(new WaterColumn(xPos, height, k, drag));
        }
    }

    private void FixedUpdate()
    {
        // 컬럼 업데이트
        for (int i = 0; i < columns.Count; i++)
            columns[i].UpdateColumn();

        // 파동 전파
        float[] leftDeltas = new float[columns.Count];
        float[] rightDeltas = new float[columns.Count];

        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = (columns[i].height - columns[i - 1].height) * spread;
                columns[i - 1].velocity += leftDeltas[i];
            }
            if (i < columns.Count - 1)
            {
                rightDeltas[i] = (columns[i].height - columns[i + 1].height) * spread;
                columns[i + 1].velocity += rightDeltas[i];
            }
        }

        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
                columns[i - 1].height += leftDeltas[i];
            if (i < columns.Count - 1)
                columns[i + 1].height += rightDeltas[i];
        }

        MakeMesh();
    }

    private void MakeMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[columns.Count * 2];
        int v = 0;
        for (int i = 0; i < columns.Count; i++)
        {
            vertices[v] = new Vector3(columns[i].xPos, columns[i].height, 0f);
            vertices[v + 1] = new Vector3(columns[i].xPos, 0f, 0f);
            v += 2;
        }

        int[] triangles = new int[(columns.Count - 1) * 6];
        int t = 0;
        v = 0;
        for (int i = 0; i < columns.Count - 1; i++)
        {
            triangles[t++] = v;
            triangles[t++] = v + 2;
            triangles[t++] = v + 1;
            triangles[t++] = v + 1;
            triangles[t++] = v + 2;
            triangles[t++] = v + 3;
            v += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }

    public void SplashAtPosition(float x, float force)
    {
        WaterColumn nearest = null;
        float minDist = float.MaxValue;

        foreach (var col in columns)
        {
            float dist = Mathf.Abs(col.xPos - x);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = col;
            }
        }

        if (nearest != null)
            nearest.velocity += force;
    }
}
