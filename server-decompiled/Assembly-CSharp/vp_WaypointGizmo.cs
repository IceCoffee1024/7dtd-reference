using System;
using UnityEngine;

// Token: 0x02001579 RID: 5497
public class vp_WaypointGizmo : MonoBehaviour
{
	// Token: 0x0600ABE2 RID: 44002 RVA: 0x003FF4C8 File Offset: 0x003FD6C8
	public void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_GizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600ABE3 RID: 44003 RVA: 0x003FF52C File Offset: 0x003FD72C
	public void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_SelectedGizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04007EEB RID: 32491
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_GizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04007EEC RID: 32492
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SelectedGizmoColor = new Color32(160, byte.MaxValue, 100, 100);
}
