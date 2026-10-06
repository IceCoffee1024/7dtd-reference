using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015C7 RID: 5575
public sealed class vp_DecalManager
{
	// Token: 0x1700144F RID: 5199
	// (get) Token: 0x0600AE11 RID: 44561 RVA: 0x0040F1B0 File Offset: 0x0040D3B0
	// (set) Token: 0x0600AE12 RID: 44562 RVA: 0x0040F1B7 File Offset: 0x0040D3B7
	public static float MaxDecals
	{
		get
		{
			return vp_DecalManager.m_MaxDecals;
		}
		set
		{
			vp_DecalManager.m_MaxDecals = value;
			vp_DecalManager.Refresh();
		}
	}

	// Token: 0x17001450 RID: 5200
	// (get) Token: 0x0600AE13 RID: 44563 RVA: 0x0040F1C4 File Offset: 0x0040D3C4
	// (set) Token: 0x0600AE14 RID: 44564 RVA: 0x0040F1CB File Offset: 0x0040D3CB
	public static float FadedDecals
	{
		get
		{
			return vp_DecalManager.m_FadedDecals;
		}
		set
		{
			if (value > vp_DecalManager.m_MaxDecals)
			{
				Debug.LogError("FadedDecals can't be larger than MaxDecals");
				return;
			}
			vp_DecalManager.m_FadedDecals = value;
			vp_DecalManager.Refresh();
		}
	}

	// Token: 0x0600AE15 RID: 44565 RVA: 0x0040F1EC File Offset: 0x0040D3EC
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_DecalManager()
	{
		vp_DecalManager.Refresh();
	}

	// Token: 0x0600AE16 RID: 44566 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_DecalManager()
	{
	}

	// Token: 0x0600AE17 RID: 44567 RVA: 0x0040F23C File Offset: 0x0040D43C
	public static void Add(GameObject decal)
	{
		if (vp_DecalManager.m_Decals.Contains(decal))
		{
			vp_DecalManager.m_Decals.Remove(decal);
		}
		Color color = decal.GetComponent<Renderer>().material.color;
		color.a = 1f;
		decal.GetComponent<Renderer>().material.color = color;
		vp_DecalManager.m_Decals.Add(decal);
		vp_DecalManager.FadeAndRemove();
	}

	// Token: 0x0600AE18 RID: 44568 RVA: 0x0040F2A0 File Offset: 0x0040D4A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FadeAndRemove()
	{
		if ((float)vp_DecalManager.m_Decals.Count > vp_DecalManager.m_NonFadedDecals)
		{
			int num = 0;
			while ((float)num < (float)vp_DecalManager.m_Decals.Count - vp_DecalManager.m_NonFadedDecals)
			{
				if (vp_DecalManager.m_Decals[num] != null)
				{
					Color color = vp_DecalManager.m_Decals[num].GetComponent<Renderer>().material.color;
					color.a -= vp_DecalManager.m_FadeAmount;
					vp_DecalManager.m_Decals[num].GetComponent<Renderer>().material.color = color;
				}
				num++;
			}
		}
		if (vp_DecalManager.m_Decals[0] != null)
		{
			if (vp_DecalManager.m_Decals[0].GetComponent<Renderer>().material.color.a <= 0f)
			{
				vp_Utility.Destroy(vp_DecalManager.m_Decals[0]);
				vp_DecalManager.m_Decals.Remove(vp_DecalManager.m_Decals[0]);
				return;
			}
		}
		else
		{
			vp_DecalManager.m_Decals.RemoveAt(0);
		}
	}

	// Token: 0x0600AE19 RID: 44569 RVA: 0x0040F3A0 File Offset: 0x0040D5A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Refresh()
	{
		if (vp_DecalManager.m_MaxDecals < vp_DecalManager.m_FadedDecals)
		{
			vp_DecalManager.m_MaxDecals = vp_DecalManager.m_FadedDecals;
		}
		vp_DecalManager.m_FadeAmount = vp_DecalManager.m_MaxDecals / vp_DecalManager.m_FadedDecals / vp_DecalManager.m_MaxDecals;
		vp_DecalManager.m_NonFadedDecals = vp_DecalManager.m_MaxDecals - vp_DecalManager.m_FadedDecals;
	}

	// Token: 0x0600AE1A RID: 44570 RVA: 0x0040F3E0 File Offset: 0x0040D5E0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugOutput()
	{
		int num = 0;
		int num2 = 0;
		using (List<GameObject>.Enumerator enumerator = vp_DecalManager.m_Decals.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Renderer>().material.color.a == 1f)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		Debug.Log(string.Concat(new string[]
		{
			"Decal count: ",
			vp_DecalManager.m_Decals.Count.ToString(),
			", Full: ",
			num.ToString(),
			", Faded: ",
			num2.ToString()
		}));
	}

	// Token: 0x04008169 RID: 33129
	public static readonly vp_DecalManager instance = new vp_DecalManager();

	// Token: 0x0400816A RID: 33130
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<GameObject> m_Decals = new List<GameObject>();

	// Token: 0x0400816B RID: 33131
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxDecals = 100f;

	// Token: 0x0400816C RID: 33132
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadedDecals = 20f;

	// Token: 0x0400816D RID: 33133
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_NonFadedDecals = 0f;

	// Token: 0x0400816E RID: 33134
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadeAmount = 0f;
}
