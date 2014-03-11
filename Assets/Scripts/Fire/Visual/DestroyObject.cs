using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyObject : MonoBehaviour {

	private SortedList sortedListOfTrianglesToBeDeleted;
	private SortedList sortedListOfNeighbouringTriangles;

	// Use this for initialization
	void Start () {
		sortedListOfNeighbouringTriangles = new SortedList ();
		sortedListOfNeighbouringTriangles.Add (0, null);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			MeshHelper.Subdivide(mesh, 2);
		}
		if (Input.GetKeyDown ("return")) {
			sortedListOfTrianglesToBeDeleted = sortedListOfNeighbouringTriangles;
			sortedListOfNeighbouringTriangles = FindNeighbouringTriangles(GetComponent<MeshFilter>().mesh, sortedListOfTrianglesToBeDeleted);
			DestroyTriangles(GetComponent<MeshFilter>().mesh ,sortedListOfTrianglesToBeDeleted);
		}
	}

	int FindFirstTriangle(Mesh mesh, Vector3 positionOfFire) {
		int closestTriangle = 0;
		float testDistance, currentMaxDistanceBetweenPoints = float.MaxValue;
		Vector3 centerOfTriangle;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		for(int i = 0; i < triangles.Length ; i = i + 3) {
			centerOfTriangle = FindCenterOfTriangle(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
			testDistance = FindDistanceBetweenTwoPoints(positionOfFire, centerOfTriangle);
			if(testDistance < currentMaxDistanceBetweenPoints) {
				currentMaxDistanceBetweenPoints = testDistance;
				closestTriangle = i;
			}
		}
		return closestTriangle;
	}

	Vector3 FindCenterOfTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
		Vector3 centerOfTriangle = new Vector3((v1.x + v2.x + v3.x)/3, (v1.y + v2.y + v3.y)/3, (v1.z + v2.z + v3.z)/3);
		return centerOfTriangle;
	}

	float FindDistanceBetweenTwoPoints(Vector3 v1, Vector3 v2) {
		float distance = Mathf.Sqrt (Mathf.Pow((v1.x - v2.x), 2) + Mathf.Pow((v1.y - v2.y), 2) + Mathf.Pow((v1.z - v2.z), 2));
		return distance;
	}

	SortedList FindNeighbouringTriangles(Mesh mesh, SortedList listOfTriangles) {
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		SortedList listOfNeighbouringTriangles = new SortedList (); 

		foreach (DictionaryEntry de in listOfTriangles) {
			Vector3 v1 = vertices [triangles [(int)de.Key]];
			Vector3 v2 = vertices [triangles [(int)de.Key + 1]];
			Vector3 v3 = vertices [triangles [(int)de.Key + 2]];	
			for (int i = 0; i < triangles.Length; i++) {
				Vector3 v = vertices[triangles[i]];
				if(v == v1 || v == v2 || v == v3) {
					int testTriangle = i/3;
					Debug.Log(i - i%3);
					Debug.Log(testTriangle != (int)de.Key && !listOfNeighbouringTriangles.Contains(i - i%3));
					if(testTriangle != (int)de.Key && !listOfNeighbouringTriangles.Contains(i - i%3)) {
						listOfNeighbouringTriangles.Add(i - i%3, null);
					}
				}
			}
		}
		return listOfNeighbouringTriangles;
	}

	void DestroyTriangles(Mesh mesh, SortedList sortedListOfTriangles){
		List<int> triangles = new List<int>(mesh.triangles);
		foreach(DictionaryEntry de in sortedListOfTriangles) {
			triangles.RemoveAt((int)de.Key);
			triangles.RemoveAt((int)de.Key);
			triangles.RemoveAt((int)de.Key);
		}
		mesh.triangles = triangles.ToArray();
	}
}
