using System;
using UnityEngine;

// Token: 0x020015B5 RID: 5557
public class vp_Gizmo : MonoBehaviour
{
	// Token: 0x0600AD49 RID: 44361 RVA: 0x0040957C File Offset: 0x0040777C
	public void OnDrawGizmos()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.gizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x0600AD4A RID: 44362 RVA: 0x004095F0 File Offset: 0x004077F0
	public void OnDrawGizmosSelected()
	{
		Vector3 center = base.GetComponent<Collider>().bounds.center;
		Vector3 size = base.GetComponent<Collider>().bounds.size;
		Gizmos.color = this.selectedGizmoColor;
		Gizmos.DrawCube(center, size);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04008033 RID: 32819
	public Color gizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04008034 RID: 32820
	public Color selectedGizmoColor = new Color(1f, 1f, 1f, 0.4f);
}
