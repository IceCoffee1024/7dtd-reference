using System;
using UnityEngine;

// Token: 0x020015C8 RID: 5576
public class vp_WaypointGizmo : MonoBehaviour
{
	// Token: 0x0600ADEA RID: 44522 RVA: 0x0040BA60 File Offset: 0x00409C60
	public void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_GizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600ADEB RID: 44523 RVA: 0x0040BAC4 File Offset: 0x00409CC4
	public void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_SelectedGizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04008088 RID: 32904
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_GizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04008089 RID: 32905
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SelectedGizmoColor = new Color32(160, byte.MaxValue, 100, 100);
}
