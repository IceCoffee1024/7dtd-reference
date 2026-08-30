using System;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class PrefabInstanceGizmo : MonoBehaviour
{
	// Token: 0x06002C6E RID: 11374 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x001192E4 File Offset: 0x001174E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrawGizmos()
	{
		if (this.pi != null)
		{
			Gizmos.color = ((!this.bSelected) ? Color.green : Color.yellow);
			if (!this.bSelected && base.transform.hasChanged)
			{
				Gizmos.color = Color.cyan;
			}
			Vector3 vector = this.pi.boundingBoxSize.ToVector3();
			Gizmos.DrawSphere(base.transform.position + new Vector3(0f, vector.y + 1f, 0f), 1.5f);
			Gizmos.DrawWireCube(base.transform.position + new Vector3(0f, (vector.y - (float)this.pi.prefab.yOffset) / 2f, 0f), this.pi.boundingBoxSize.ToVector3() + new Vector3(0f, (float)this.pi.prefab.yOffset, 0f));
			if (this.pi.prefab.yOffset != 0)
			{
				Gizmos.color = ((!this.bSelected) ? new Color(0f, 0.5f, 0f, 0.5f) : new Color(0.7f, 0.7f, 0f, 0.5f));
				if (!this.bSelected && base.transform.hasChanged)
				{
					Gizmos.color = Color.cyan;
				}
				Gizmos.DrawCube(base.transform.position + new Vector3(0f, (float)(-1 * this.pi.prefab.yOffset) / 2f, 0f), new Vector3((float)this.pi.boundingBoxSize.x, (float)(-1 * this.pi.prefab.yOffset) - 0.1f, (float)this.pi.boundingBoxSize.z));
				Gizmos.color = ((!this.bSelected) ? Color.green : Color.yellow);
				if (!this.bSelected && base.transform.hasChanged)
				{
					Gizmos.color = Color.cyan;
				}
				Gizmos.DrawWireCube(base.transform.position + new Vector3(0f, (float)(-1 * this.pi.prefab.yOffset) / 2f, 0f), new Vector3((float)this.pi.boundingBoxSize.x, (float)(-1 * this.pi.prefab.yOffset) - 0.1f, (float)this.pi.boundingBoxSize.z));
			}
			this.pos = base.transform.position - new Vector3((float)this.pi.boundingBoxSize.x * 0.5f, 0f, (float)this.pi.boundingBoxSize.z * 0.5f);
			Vector3 zero = Vector3.zero;
			switch (this.pi.rotation)
			{
			case 0:
				zero = new Vector3(-0.5f, 0f, -0.5f);
				break;
			case 1:
				zero = new Vector3(-0.5f, 0f, 0.5f);
				break;
			case 2:
				zero = new Vector3(0.5f, 0f, 0.5f);
				break;
			case 3:
				zero = new Vector3(0.5f, 0f, -0.5f);
				break;
			}
			if (Utils.FastAbs(this.pos.x - (float)((int)this.pos.x)) > 0.001f)
			{
				this.pos.x = this.pos.x - zero.x;
			}
			if (Utils.FastAbs(this.pos.z - (float)((int)this.pos.z)) > 0.001f)
			{
				this.pos.z = this.pos.z - zero.z;
			}
			this.rot = (int)this.pi.rotation;
			if (PrefabInstanceGizmo.Selected == this.pi)
			{
				this.pi.boundingBoxPosition = World.worldToBlockPos(this.pos);
				this.pi.rotation = (byte)this.rot;
			}
		}
		this.bSelected = false;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x0011972F File Offset: 0x0011792F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrawGizmosSelected()
	{
		this.bSelected = true;
		PrefabInstanceGizmo.Selected = this.pi;
	}

	// Token: 0x04002231 RID: 8753
	public static PrefabInstance Selected;

	// Token: 0x04002232 RID: 8754
	public PrefabInstance pi;

	// Token: 0x04002233 RID: 8755
	public Vector3 pos;

	// Token: 0x04002234 RID: 8756
	public int rot;

	// Token: 0x04002235 RID: 8757
	public bool bSelected;
}
