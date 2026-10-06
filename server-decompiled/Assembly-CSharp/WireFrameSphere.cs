using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CFA RID: 3322
[RequireComponent(typeof(LineRenderer))]
public class WireFrameSphere : MonoBehaviour
{
	// Token: 0x0600653D RID: 25917 RVA: 0x0027C230 File Offset: 0x0027A430
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.lr = base.gameObject.GetComponent<LineRenderer>();
		this.lr.startWidth = 0.01f;
		this.lr.endWidth = 0.01f;
		this.positions = new List<Vector3>();
		this.player = global::UnityEngine.Object.FindAnyObjectByType<LocalPlayerCamera>();
	}

	// Token: 0x0600653E RID: 25918 RVA: 0x0027C284 File Offset: 0x0027A484
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.player != null)
		{
			float num = Mathf.Abs(this.player.transform.position.magnitude - base.transform.position.magnitude);
			this.lr.startWidth = (this.lr.endWidth = 0.01f * num);
		}
		if (this.radius != this.newRadius)
		{
			this.radius = this.newRadius;
			this.positions.Clear();
			this.positions.AddRange(this.RenderCircleOnPlane(true));
			this.positions.AddRange(this.RenderCircleOnPlane(false));
			this.lr.positionCount = this.positions.Count;
			this.lr.SetPositions(this.positions.ToArray());
		}
	}

	// Token: 0x0600653F RID: 25919 RVA: 0x0027C368 File Offset: 0x0027A568
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3> RenderCircleOnPlane(bool xyPlane)
	{
		this.numOfVertices = 100 + (int)Math.Pow((double)((int)this.radius), 2.0);
		List<Vector3> list = new List<Vector3>(this.numOfVertices);
		this.angle = 6.2831855f / (float)(this.numOfVertices - 1);
		for (int i = 0; i < this.numOfVertices; i++)
		{
			float num = this.center.x + this.radius * Mathf.Cos((float)i * this.angle);
			float num2 = this.center.y + (xyPlane ? (this.radius * Mathf.Sin((float)i * this.angle)) : 0f);
			float num3 = this.center.z + (xyPlane ? 0f : (this.radius * Mathf.Sin((float)i * this.angle)));
			list.Add(new Vector3(num, num2, num3));
		}
		return list;
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x001483DF File Offset: 0x001465DF
	public void KillWF()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004EF8 RID: 20216
	public Vector3 center;

	// Token: 0x04004EF9 RID: 20217
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float TAU = 6.2831855f;

	// Token: 0x04004EFA RID: 20218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float radius = 1f;

	// Token: 0x04004EFB RID: 20219
	public float newRadius = 1f;

	// Token: 0x04004EFC RID: 20220
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer lr;

	// Token: 0x04004EFD RID: 20221
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> positions;

	// Token: 0x04004EFE RID: 20222
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfVertices = 100;

	// Token: 0x04004EFF RID: 20223
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float angle;

	// Token: 0x04004F00 RID: 20224
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera player;
}
