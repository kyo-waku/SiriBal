using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralCorrectionGizmo : MonoBehaviour
{
    public float gizmoSize = 0.3f;
	public Color gizmoColor = Color.yellow;

	void OnDrawGizmos()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawWireSphere (transform.position, gizmoSize);
	}
}
