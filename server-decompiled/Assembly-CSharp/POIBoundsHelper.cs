using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054F RID: 1359
public class POIBoundsHelper : MonoBehaviour
{
	// Token: 0x06002C57 RID: 11351 RVA: 0x00119B8C File Offset: 0x00117D8C
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

	// Token: 0x06002C58 RID: 11352 RVA: 0x00119CB0 File Offset: 0x00117EB0
	public void SetSidesVisible(bool visible)
	{
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].MeshRenderer.enabled = visible;
		}
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x00119CEC File Offset: 0x00117EEC
	public void SetPosition(Vector3 position, Vector3 size)
	{
		base.transform.position = position;
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].SetSize(size);
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x00119D30 File Offset: 0x00117F30
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

	// Token: 0x06002C5B RID: 11355 RVA: 0x00119E7E File Offset: 0x0011807E
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

	// Token: 0x06002C5C RID: 11356 RVA: 0x00119EB6 File Offset: 0x001180B6
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

	// Token: 0x04002225 RID: 8741
	public List<POIBoundsSideHelper> SideHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x04002226 RID: 8742
	public List<POIBoundsSideHelper> ActivatedHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x04002227 RID: 8743
	public POIBoundsHelper.WallVisibilityStates CurrentState;

	// Token: 0x04002228 RID: 8744
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x04002229 RID: 8745
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float showTime = 3f;

	// Token: 0x0400222A RID: 8746
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fullAlpha = -1f;

	// Token: 0x02000550 RID: 1360
	public enum WallVisibilityStates
	{
		// Token: 0x0400222C RID: 8748
		None,
		// Token: 0x0400222D RID: 8749
		Showing,
		// Token: 0x0400222E RID: 8750
		Visible,
		// Token: 0x0400222F RID: 8751
		ReadyToHide,
		// Token: 0x04002230 RID: 8752
		Hiding
	}
}
