using System;
using UnityEngine;

// Token: 0x02001566 RID: 5478
public class vp_Gizmo : MonoBehaviour
{
	// Token: 0x0600AB41 RID: 43841 RVA: 0x003FCFE4 File Offset: 0x003FB1E4
	public void OnDrawGizmos()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.gizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600AB42 RID: 43842 RVA: 0x003FD058 File Offset: 0x003FB258
	public void OnDrawGizmosSelected()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.selectedGizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04007E96 RID: 32406
	public Color gizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04007E97 RID: 32407
	public Color selectedGizmoColor = new Color(1f, 1f, 1f, 0.4f);
}
