using System;
using UnityEngine;

// Token: 0x020015B6 RID: 5558
public class vp_WaypointGizmo : MonoBehaviour
{
	// Token: 0x0600AD92 RID: 44434 RVA: 0x0040965C File Offset: 0x0040785C
	public void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_GizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600AD93 RID: 44435 RVA: 0x004096C0 File Offset: 0x004078C0
	public void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_SelectedGizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04008047 RID: 32839
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_GizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04008048 RID: 32840
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SelectedGizmoColor = new Color32(160, byte.MaxValue, 100, 100);
}
