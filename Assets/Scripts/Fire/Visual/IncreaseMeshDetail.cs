// C#
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IncreaseMeshDetail : MonoBehaviour
{
	static List<Vector3> vertices;
	static List<Vector3> normals;
	// [... all other vertex data arrays you need]
	
	static List<int> indices;
	static Dictionary<uint,int> newVectices;
	
	void Start() {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Debug.Log(mesh.vertexCount);
		Subdivide (mesh);
		Debug.Log (mesh.vertexCount);
	}
	
	void Update() {
		if (Input.GetKeyDown ("space")) {
			Mesh mesh = GetComponent<MeshFilter> ().mesh;
			List<int> indices = new List<int> (mesh.triangles);
			
			indices.RemoveRange (0, 3);
			mesh.triangles = indices.ToArray ();
			Debug.Log (mesh.triangles.Length);
		}
	}
	
	static int GetNewVertex(int i1, int i2)
	{
		// Test both directions since the edge
		// could be reversed in another triangle
		uint t1 = ((uint)i1 << 16) | (uint)i2;
		uint t2 = ((uint)i2 << 16) | (uint)i1;
		if (newVectices.ContainsKey(t2))
			return newVectices[t2];
		if (newVectices.ContainsKey(t1))
			return newVectices[t1];
		// generate vertex:
		int newIndex = vertices.Count;
		newVectices.Add(t1,newIndex);
		
		// calculate new vertex
		vertices.Add((vertices[i1] + vertices[i2]) * 0.5f);
		normals.Add((normals[i1] + normals[i2]).normalized);
		// [... all other vertex data arrays]
		
		return newIndex;
	}
	
	
	public static void Subdivide(Mesh mesh)
	{
		newVectices = new Dictionary<uint,int>();
		Debug.Log("Test");
		vertices = new List<Vector3>(mesh.vertices);
		normals = new List<Vector3>(mesh.normals);
		// [... all other vertex data arrays]
		indices = new List<int>();
		
		int[] triangles = mesh.triangles;
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int i1 = triangles[i + 0];
			int i2 = triangles[i + 1];
			int i3 = triangles[i + 2];
			
			int a = GetNewVertex(i1, i2);
			int b = GetNewVertex(i2, i3);
			int c = GetNewVertex(i3, i1);
			indices.Add(i1);   indices.Add(a);   indices.Add(c);
			indices.Add(i2);   indices.Add(b);   indices.Add(a);
			indices.Add(i3);   indices.Add(c);   indices.Add(b);
			indices.Add(a );   indices.Add(b);   indices.Add(c); // center triangle
		}
		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		// [... all other vertex data arrays]
		mesh.triangles = indices.ToArray();
		
		// since this is a static function and it uses static variables
		// we should erase the arrays to free them:
		newVectices = null;
		vertices = null;
		normals = null;
		// [... all other vertex data arrays]
		
		indices = null;
	}
}