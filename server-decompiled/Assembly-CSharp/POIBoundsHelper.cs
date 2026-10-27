using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class POIBoundsHelper : MonoBehaviour
{
	// Token: 0x06002C5F RID: 11359 RVA: 0x0011A118 File Offset: 0x00118318
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (this.SideHelpers != null && this.SideHelpers.Count > 0)
		{
			this.SideHelpers[0].Setup();
			this.material = new Material(this.SideHelpers[0].MeshRenderer.material);
			this.fullAlpha = this.material.color.a;
			this.material.color = new Color(this.material.color.r, this.material.color.g, this.material.color.b, 0f);
			for (int i = 0; i < this.SideHelpers.Count; i++)
			{
				this.SideHelpers[i].Owner = this;
				this.SideHelpers[i].Setup();
				this.SideHelpers[i].MeshRenderer.material = this.material;
				this.SideHelpers[i].MeshRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x0011A23C File Offset: 0x0011843C
	public void SetSidesVisible(bool visible)
	{
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].MeshRenderer.enabled = visible;
		}
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x0011A278 File Offset: 0x00118478
	public void SetPosition(Vector3 position, Vector3 size)
	{
		base.transform.position = position;
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].SetSize(size);
		}
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x0011A2BC File Offset: 0x001184BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		switch (this.CurrentState)
		{
		case POIBoundsHelper.WallVisibilityStates.None:
		case POIBoundsHelper.WallVisibilityStates.Visible:
			break;
		case POIBoundsHelper.WallVisibilityStates.Showing:
		{
			float num = Mathf.MoveTowards(this.material.color.a, this.fullAlpha, Time.deltaTime);
			this.material.color = new Color(this.material.color.r, this.material.color.g, this.material.color.b, num);
			if (num == this.fullAlpha)
			{
				this.showTime = 3f;
				this.CurrentState = POIBoundsHelper.WallVisibilityStates.Visible;
				return;
			}
			break;
		}
		case POIBoundsHelper.WallVisibilityStates.ReadyToHide:
			this.showTime -= Time.deltaTime;
			if (this.showTime <= 0f)
			{
				this.CurrentState = POIBoundsHelper.WallVisibilityStates.Hiding;
				return;
			}
			break;
		case POIBoundsHelper.WallVisibilityStates.Hiding:
		{
			float num2 = Mathf.MoveTowards(this.material.color.a, 0f, Time.deltaTime);
			this.material.color = new Color(this.material.color.r, this.material.color.g, this.material.color.b, num2);
			if (num2 == 0f)
			{
				this.CurrentState = POIBoundsHelper.WallVisibilityStates.None;
				this.SetSidesVisible(false);
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x0011A40A File Offset: 0x0011860A
	public void AddSideEntered(POIBoundsSideHelper side)
	{
		if (!this.ActivatedHelpers.Contains(side))
		{
			this.ActivatedHelpers.Add(side);
		}
		if (this.ActivatedHelpers.Count > 0)
		{
			this.CurrentState = POIBoundsHelper.WallVisibilityStates.Showing;
			this.SetSidesVisible(true);
		}
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x0011A442 File Offset: 0x00118642
	public void RemoveSideEntered(POIBoundsSideHelper side)
	{
		if (this.ActivatedHelpers.Contains(side))
		{
			this.ActivatedHelpers.Remove(side);
		}
		if (this.ActivatedHelpers.Count == 0)
		{
			this.CurrentState = POIBoundsHelper.WallVisibilityStates.ReadyToHide;
			this.showTime = 3f;
		}
	}

	// Token: 0x0400223C RID: 8764
	public List<POIBoundsSideHelper> SideHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x0400223D RID: 8765
	public List<POIBoundsSideHelper> ActivatedHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x0400223E RID: 8766
	public POIBoundsHelper.WallVisibilityStates CurrentState;

	// Token: 0x0400223F RID: 8767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x04002240 RID: 8768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float showTime = 3f;

	// Token: 0x04002241 RID: 8769
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fullAlpha = -1f;

	// Token: 0x02000554 RID: 1364
	public enum WallVisibilityStates
	{
		// Token: 0x04002243 RID: 8771
		None,
		// Token: 0x04002244 RID: 8772
		Showing,
		// Token: 0x04002245 RID: 8773
		Visible,
		// Token: 0x04002246 RID: 8774
		ReadyToHide,
		// Token: 0x04002247 RID: 8775
		Hiding
	}
}
