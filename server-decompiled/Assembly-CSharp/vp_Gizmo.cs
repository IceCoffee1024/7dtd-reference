using System;
using UnityEngine;

// Token: 0x020015A3 RID: 5539
public class vp_Gizmo : MonoBehaviour
{
	// Token: 0x0600ACF1 RID: 44273 RVA: 0x00407178 File Offset: 0x00405378
	public void OnDrawGizmos()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.gizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600ACF2 RID: 44274 RVA: 0x004071EC File Offset: 0x004053EC
	public void OnDrawGizmosSelected()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.selectedGizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04007FF2 RID: 32754
	public Color gizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04007FF3 RID: 32755
	public Color selectedGizmoColor = new Color(1f, 1f, 1f, 0.4f);
}
