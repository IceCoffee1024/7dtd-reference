using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CDA RID: 3290
[RequireComponent(typeof(LineRenderer))]
public class WireFrameSphere : MonoBehaviour
{
	// Token: 0x060064BF RID: 25791 RVA: 0x00275F90 File Offset: 0x00274190
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.lr = base.gameObject.GetComponent<LineRenderer>();
		this.lr.startWidth = 0.01f;
		this.lr.endWidth = 0.01f;
		this.positions = new List<Vector3>();
		this.player = global::UnityEngine.Object.FindAnyObjectByType<LocalPlayerCamera>();
	}

	// Token: 0x060064C0 RID: 25792 RVA: 0x00275FE4 File Offset: 0x002741E4
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

	// Token: 0x060064C1 RID: 25793 RVA: 0x002760C8 File Offset: 0x002742C8
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

	// Token: 0x060064C2 RID: 25794 RVA: 0x00145937 File Offset: 0x00143B37
	public void KillWF()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004E24 RID: 20004
	public Vector3 center;

	// Token: 0x04004E25 RID: 20005
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float TAU = 6.2831855f;

	// Token: 0x04004E26 RID: 20006
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float radius = 1f;

	// Token: 0x04004E27 RID: 20007
	public float newRadius = 1f;

	// Token: 0x04004E28 RID: 20008
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer lr;

	// Token: 0x04004E29 RID: 20009
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector3> positions;

	// Token: 0x04004E2A RID: 20010
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfVertices = 100;

	// Token: 0x04004E2B RID: 20011
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float angle;

	// Token: 0x04004E2C RID: 20012
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerCamera player;
}
