using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
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

	void FindNeighbouringTriangles(Mesh mesh, int currentTriangle) {
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		 
		Vector3 v1 = vertices [triangles [currentTriangle++]];
		Vector3 v2 = vertices [triangles [currentTriangle++]];
		Vector3 v3 = vertices [triangles [currentTriangle]];
		
		for (int i = 0; i < triangles.Length; i++) {
			Vector3 v = vertices[triangles[i]];
			if(v == v1 || v == v2 || v == v3) {
				int testTriangle = i/3;
				if(testTriangle != currentTriangle) {
					//This triangle should be destroyed next 
				}
			}
		}
	}
}
