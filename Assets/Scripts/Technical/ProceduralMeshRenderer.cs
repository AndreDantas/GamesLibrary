using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct ProceduralMeshValues
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;

    public ProceduralMeshValues(Vector3[] vertices, Vector2[] uvs, int[] triangles)
    {
        this.vertices = vertices;
        this.uvs = uvs;
        this.triangles = triangles;
    }
}
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMeshRenderer : MonoBehaviour
{
    public MeshRenderer meshRender;
    public Color renderColor = Color.green;
    MeshFilter meshFilter;

    public void Awake()
    {
        if (meshRender == null)
            meshRender = GetComponent<MeshRenderer>();
        if ((meshFilter = GetComponent<MeshFilter>()) != null)
            meshFilter.mesh = null;

        //RenderCircles(new List<Vector3> { Vector3.right }, 0.5f, 30);
    }

    private void OnValidate()
    {
        if (meshRender == null)
            meshRender = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// This function renders squares in the positions.
    /// </summary>
    /// <param name="squarePos">List of positions.</param>
    /// <param name="scale">Scale of each square.</param>
    public void RenderSquaresArea(List<Vector3> squarePos, float width, float height)
    {
        if (squarePos == null || squarePos.Count == 0 || !meshFilter)
            return;

        Mesh mesh;
        meshFilter.mesh = mesh = new Mesh();

        int verticesNumber = squarePos.Count * 4; // 4 vertices per square
        int trianglesNumber = squarePos.Count * 6; // 2 Triangles per square

        Vector3[] vertices = new Vector3[verticesNumber];
        int[] triangles = new int[trianglesNumber];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < squarePos.Count; i++)
        {
            // Vertices Creation
            // Bottom left 0
            vertices[i * 4] = (squarePos[i] + new Vector3(-width / 2f, -height / 2f) - transform.position);
            uv[i * 4] = new Vector2(squarePos[i].x / width, squarePos[i].y / height) + new Vector2(-0.5f, -0.5f) - (Vector2)transform.position;
            // Bottom right 1
            vertices[i * 4 + 1] = (squarePos[i] + new Vector3(width / 2f, -height / 2f, 0) - transform.position);
            uv[i * 4 + 1] = new Vector2(squarePos[i].x / width, squarePos[i].y / height) + new Vector2(-0.5f, -0.5f) - (Vector2)transform.position;
            // Top right 2
            vertices[i * 4 + 2] = (squarePos[i] + new Vector3(width / 2f, height / 2f, 0) - transform.position);
            uv[i * 4 + 2] = new Vector2(squarePos[i].x / width, squarePos[i].y / height) + new Vector2(-0.5f, -0.5f) - (Vector2)transform.position;
            // Top left 3
            vertices[i * 4 + 3] = (squarePos[i] + new Vector3(-width / 2f, height / 2f, 0) - transform.position);
            uv[i * 4 + 3] = new Vector2(squarePos[i].x / width, squarePos[i].y / height) + new Vector2(-0.5f, -0.5f) - (Vector2)transform.position;


            // Triangles Creation
            triangles[i * 6] = i * 4;
            triangles[i * 6 + 1] = i * 4 + 3;
            triangles[i * 6 + 2] = i * 4 + 1;

            triangles[i * 6 + 3] = i * 4 + 1;
            triangles[i * 6 + 4] = i * 4 + 3;
            triangles[i * 6 + 5] = i * 4 + 2;


        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        if (meshRender)
            meshRender.material.color = renderColor;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    /// <summary>
    /// Clears the rendering.
    /// </summary>
    public void Clear()
    {
        if (meshFilter)
            meshFilter.mesh = null;
    }

    /// <summary>
    /// Updates the rendering if the mesh is not null.
    /// </summary>
    /// <param name="squarePos">List of positions.</param>
    /// <param name="scale">Scale of each square.</param>
    public void UpdateRenderSquare(List<Vector3> squarePos, float width, float height)
    {
        if (!meshFilter)
            return;
        if (meshFilter.mesh == null)
            return;

        RenderSquaresArea(squarePos, width, height);
    }

    public ProceduralMeshValues CalculateCircleMeshPoints(Vector3 circlePos, float radius = 1f, int circlePoints = 30)
    {


        circlePoints = Mathf.Max(3, circlePoints);

        Vector3[] vertices = new Vector3[circlePoints];
        Vector2[] uvs = new Vector2[circlePoints];
        int[] triangles = new int[circlePoints * 3];


        float angle = 360f / (circlePoints - 1);

        vertices[0] = circlePos;
        uvs[0] = new Vector2(0.5f, 0.5f) + (Vector2)circlePos;
        for (int i = 1; i < circlePoints; i++)
        {
            vertices[i] = Quaternion.AngleAxis(angle * (i - 1), Vector3.back) * Vector3.up * radius + circlePos;
            float normedHorizontal = (vertices[i].x + 1.0f) * 0.5f;
            float normedVertical = (vertices[i].y + 1.0f) * 0.5f;
            uvs[i] = new Vector2(normedHorizontal, normedVertical) + (Vector2)circlePos;

        }

        for (int j = 0; j + 2 < circlePoints; j++)
        {
            int index = j * 3;
            triangles[index] = 0;
            triangles[index + 1] = j + 1;
            triangles[index + 2] = j + 2;

        }

        int triIndex = circlePoints * 3 - 3;
        triangles[triIndex] = 0;
        triangles[triIndex + 1] = circlePoints - 1;
        triangles[triIndex + 2] = 1;

        ProceduralMeshValues mesh = new ProceduralMeshValues();
        mesh.vertices = vertices;
        mesh.uvs = uvs;
        mesh.triangles = triangles;

        return mesh;

    }
    public void RenderCircles(List<Vector3> circlePos, float radius = 1f, int circlePoints = 30)
    {

        if (circlePos == null || circlePos.Count == 0 || !meshFilter)
            return;

        Mesh mesh;
        meshFilter.mesh = mesh = new Mesh();
        ProceduralMeshValues procMeshV;
        Vector3[] vertices = new Vector3[circlePoints * circlePos.Count];
        Vector2[] uv = new Vector2[circlePoints * circlePos.Count];
        int[] triangles = new int[circlePoints * 3 * circlePos.Count];

        for (int i = 0; i < circlePos.Count; i++)
        {
            procMeshV = CalculateCircleMeshPoints(circlePos[i], radius, circlePoints);
            for (int j = 0; j < procMeshV.vertices.Length; j++)
            {

                int triIndex = j * 3;
                vertices[j + i * circlePoints] = procMeshV.vertices[j];
                uv[j + i * circlePoints] = procMeshV.uvs[j];
                triangles[triIndex + i * circlePoints * 3] = procMeshV.triangles[triIndex] + i * circlePoints;
                triangles[triIndex + i * circlePoints * 3 + 1] = procMeshV.triangles[triIndex + 1] + i * circlePoints;
                triangles[triIndex + i * circlePoints * 3 + 2] = procMeshV.triangles[triIndex + 2] + i * circlePoints;

            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        if (meshRender)
            meshRender.material.color = renderColor;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

    }
}
