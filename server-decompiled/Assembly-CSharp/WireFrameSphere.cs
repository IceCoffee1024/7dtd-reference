using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CEC RID: 3308
[RequireComponent(typeof(LineRenderer))]
public class WireFrameSphere : MonoBehaviour
{
	// Token: 0x0600654B RID: 25931 RVA: 0x00278E8C File Offset: 0x0027708C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.lr = base.gameObject.GetComponent<LineRenderer>();
		this.lr.startWidth = 0.01f;
		this.lr.endWidth = 0.01f;
		this.positions = new List<Vector3>();
		this.player = global::UnityEngine.Object.FindAnyObjectByType<LocalPlayerCamera>();
	}

	// Token: 0x0600654C RID: 25932 RVA: 0x00278EE0 File Offset: 0x002770E0
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

	// Token: 0x0600654D RID: 25933 RVA: 0x00278FC4 File Offset: 0x002771C4
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

	// Token: 0x0600654E RID: 25934 RVA: 0x00147A47 File Offset: 0x00145C47
	public void KillWF()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004EC2 RID: 20162
	public Vector3 center;

	// Token: 0x04004EC3 RID: 20163
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float TAU = 6.2831855f;

	// Token: 0x04004EC4 RID: 20164
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float radius = 1f;

	// Token: 0x04004EC5 RID: 20165
	public float newRadius = 1f;

	// Token: 0x04004EC6 RID: 20166
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer lr;

	// Token: 0x04004EC7 RID: 20167
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> positions;

	// Token: 0x04004EC8 RID: 20168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfVertices = 100;

	// Token: 0x04004EC9 RID: 20169
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float angle;

	// Token: 0x04004ECA RID: 20170
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera player;
}
