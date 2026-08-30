using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public class POIBoundsHelper : MonoBehaviour
{
	// Token: 0x06002C37 RID: 11319 RVA: 0x00117D48 File Offset: 0x00115F48
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

	// Token: 0x06002C38 RID: 11320 RVA: 0x00117E6C File Offset: 0x0011606C
	public void SetSidesVisible(bool visible)
	{
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].MeshRenderer.enabled = visible;
		}
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x00117EA8 File Offset: 0x001160A8
	public void SetPosition(Vector3 position, Vector3 size)
	{
		base.transform.position = position;
		for (int i = 0; i < this.SideHelpers.Count; i++)
		{
			this.SideHelpers[i].SetSize(size);
		}
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x00117EEC File Offset: 0x001160EC
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

	// Token: 0x06002C3B RID: 11323 RVA: 0x0011803A File Offset: 0x0011623A
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

	// Token: 0x06002C3C RID: 11324 RVA: 0x00118072 File Offset: 0x00116272
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

	// Token: 0x040021EB RID: 8683
	public List<POIBoundsSideHelper> SideHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x040021EC RID: 8684
	public List<POIBoundsSideHelper> ActivatedHelpers = new List<POIBoundsSideHelper>();

	// Token: 0x040021ED RID: 8685
	public POIBoundsHelper.WallVisibilityStates CurrentState;

	// Token: 0x040021EE RID: 8686
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Material material;

	// Token: 0x040021EF RID: 8687
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float showTime = 3f;

	// Token: 0x040021F0 RID: 8688
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fullAlpha = -1f;

	// Token: 0x02000545 RID: 1349
	public enum WallVisibilityStates
	{
		// Token: 0x040021F2 RID: 8690
		None,
		// Token: 0x040021F3 RID: 8691
		Showing,
		// Token: 0x040021F4 RID: 8692
		Visible,
		// Token: 0x040021F5 RID: 8693
		ReadyToHide,
		// Token: 0x040021F6 RID: 8694
		Hiding
	}
}
