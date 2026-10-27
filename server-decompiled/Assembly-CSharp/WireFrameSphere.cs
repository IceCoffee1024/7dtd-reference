using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D05 RID: 3333
[RequireComponent(typeof(LineRenderer))]
public class WireFrameSphere : MonoBehaviour
{
	// Token: 0x06006562 RID: 25954 RVA: 0x0027D758 File Offset: 0x0027B958
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.lr = base.gameObject.GetComponent<LineRenderer>();
		this.lr.startWidth = 0.01f;
		this.lr.endWidth = 0.01f;
		this.positions = new List<Vector3>();
		this.player = global::UnityEngine.Object.FindAnyObjectByType<LocalPlayerCamera>();
	}

	// Token: 0x06006563 RID: 25955 RVA: 0x0027D7AC File Offset: 0x0027B9AC
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

	// Token: 0x06006564 RID: 25956 RVA: 0x0027D890 File Offset: 0x0027BA90
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

	// Token: 0x06006565 RID: 25957 RVA: 0x00148FD3 File Offset: 0x001471D3
	public void KillWF()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004F1D RID: 20253
	public Vector3 center;

	// Token: 0x04004F1E RID: 20254
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float TAU = 6.2831855f;

	// Token: 0x04004F1F RID: 20255
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float radius = 1f;

	// Token: 0x04004F20 RID: 20256
	public float newRadius = 1f;

	// Token: 0x04004F21 RID: 20257
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer lr;

	// Token: 0x04004F22 RID: 20258
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> positions;

	// Token: 0x04004F23 RID: 20259
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfVertices = 100;

	// Token: 0x04004F24 RID: 20260
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float angle;

	// Token: 0x04004F25 RID: 20261
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera player;
}
