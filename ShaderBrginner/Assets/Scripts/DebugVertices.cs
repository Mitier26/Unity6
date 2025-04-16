using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DebugVertices : MonoBehaviour {

	Mesh mesh;
	Vector3[] vertices;

	void OnDrawGizmos()
	{
		if (vertices == null)
		{
			mesh = GetComponent<MeshFilter>().sharedMesh;
			vertices = mesh.vertices;
		}

		foreach (Vector3 v in vertices)
		{
			Vector3 viewVertValue = SceneView.GetAllSceneCameras()[0].WorldToViewportPoint(transform.position + v);
			string vert = "v: " + v.ToString();
			string worldVert = " Wv: " + (transform.position + v).ToString();
			string viewVert = " Vv: " + viewVertValue.ToString();
			UnityEditor.Handles.Label(transform.position + v, vert + worldVert + viewVert);
		}

	}
}
