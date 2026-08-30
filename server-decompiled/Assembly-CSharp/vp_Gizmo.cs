using System;
using UnityEngine;

// Token: 0x02001581 RID: 5505
public class vp_Gizmo : MonoBehaviour
{
	// Token: 0x0600AC3E RID: 44094 RVA: 0x00402324 File Offset: 0x00400524
	public void OnDrawGizmos()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.gizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600AC3F RID: 44095 RVA: 0x00402398 File Offset: 0x00400598
	public void OnDrawGizmosSelected()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.selectedGizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04007F6F RID: 32623
	public Color gizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04007F70 RID: 32624
	public Color selectedGizmoColor = new Color(1f, 1f, 1f, 0.4f);
}
