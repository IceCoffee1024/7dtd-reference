using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200158A RID: 5514
public sealed class vp_DecalManager
{
	// Token: 0x170013D1 RID: 5073
	// (get) Token: 0x0600AC61 RID: 44129 RVA: 0x0040501C File Offset: 0x0040321C
	// (set) Token: 0x0600AC62 RID: 44130 RVA: 0x00405023 File Offset: 0x00403223
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

	// Token: 0x170013D2 RID: 5074
	// (get) Token: 0x0600AC63 RID: 44131 RVA: 0x00405030 File Offset: 0x00403230
	// (set) Token: 0x0600AC64 RID: 44132 RVA: 0x00405037 File Offset: 0x00403237
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

	// Token: 0x0600AC65 RID: 44133 RVA: 0x00405058 File Offset: 0x00403258
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_DecalManager()
	{
		vp_DecalManager.Refresh();
	}

	// Token: 0x0600AC66 RID: 44134 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_DecalManager()
	{
	}

	// Token: 0x0600AC67 RID: 44135 RVA: 0x004050A8 File Offset: 0x004032A8
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

	// Token: 0x0600AC68 RID: 44136 RVA: 0x0040510C File Offset: 0x0040330C
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

	// Token: 0x0600AC69 RID: 44137 RVA: 0x0040520C File Offset: 0x0040340C
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

	// Token: 0x0600AC6A RID: 44138 RVA: 0x0040524C File Offset: 0x0040344C
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

	// Token: 0x0400800D RID: 32781
	public static readonly vp_DecalManager instance = new vp_DecalManager();

	// Token: 0x0400800E RID: 32782
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<GameObject> m_Decals = new List<GameObject>();

	// Token: 0x0400800F RID: 32783
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxDecals = 100f;

	// Token: 0x04008010 RID: 32784
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadedDecals = 20f;

	// Token: 0x04008011 RID: 32785
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_NonFadedDecals = 0f;

	// Token: 0x04008012 RID: 32786
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadeAmount = 0f;
}
