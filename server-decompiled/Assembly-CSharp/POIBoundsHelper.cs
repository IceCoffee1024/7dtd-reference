using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class POIBoundsHelper : MonoBehaviour
{
	// Token: 0x06002C61 RID: 11361 RVA: 0x00118DBC File Offset: 0x00116FBC
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

	// Token: 0x06002C62 RID: 11362 RVA: 0x00118EE0 File Offset: 0x001170E0
	public void SetSidesVisible(bool visible)
	{
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].MeshRenderer.enabled = visible;
		}
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x00118F1C File Offset: 0x0011711C
	public void SetPosition(Vector3 position, Vector3 size)
	{
		base.transform.position = position;
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].SetSize(size);
		}
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x00118F60 File Offset: 0x00117160
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

	// Token: 0x06002C65 RID: 11365 RVA: 0x001190AE File Offset: 0x001172AE
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

	// Token: 0x06002C66 RID: 11366 RVA: 0x001190E6 File Offset: 0x001172E6
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

	// Token: 0x0400221D RID: 8733
	public List<POIBoundsSideHelper> SideHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x0400221E RID: 8734
	public List<POIBoundsSideHelper> ActivatedHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x0400221F RID: 8735
	public POIBoundsHelper.WallVisibilityStates CurrentState;

	// Token: 0x04002220 RID: 8736
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x04002221 RID: 8737
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float showTime = 3f;

	// Token: 0x04002222 RID: 8738
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fullAlpha = -1f;

	// Token: 0x0200054A RID: 1354
	public enum WallVisibilityStates
	{
		// Token: 0x04002224 RID: 8740
		None,
		// Token: 0x04002225 RID: 8741
		Showing,
		// Token: 0x04002226 RID: 8742
		Visible,
		// Token: 0x04002227 RID: 8743
		ReadyToHide,
		// Token: 0x04002228 RID: 8744
		Hiding
	}
}
