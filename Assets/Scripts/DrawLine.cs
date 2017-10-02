using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

	public GameObject targetObject;

	void DrawConnectingLine() {
		GL.PushMatrix ();
		GL.LoadIdentity ();
		GL.Begin (GL.LINES);
		GL.Color (Color.red);
		GL.Vertex (this.transform.position);
		GL.Vertex (targetObject.transform.position);
		GL.End ();
		GL.PopMatrix ();
	}

	public void OnPostRender() {
		DrawConnectingLine ();
	}

	void OnDrawGizmos() {
		Debug.DrawLine (this.transform.position, targetObject.transform.position);
	}
}
