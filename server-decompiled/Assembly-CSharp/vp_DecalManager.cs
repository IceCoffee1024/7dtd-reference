using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D9 RID: 5593
public sealed class vp_DecalManager
{
	// Token: 0x1700145B RID: 5211
	// (get) Token: 0x0600AE69 RID: 44649 RVA: 0x004115B4 File Offset: 0x0040F7B4
	// (set) Token: 0x0600AE6A RID: 44650 RVA: 0x004115BB File Offset: 0x0040F7BB
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

	// Token: 0x1700145C RID: 5212
	// (get) Token: 0x0600AE6B RID: 44651 RVA: 0x004115C8 File Offset: 0x0040F7C8
	// (set) Token: 0x0600AE6C RID: 44652 RVA: 0x004115CF File Offset: 0x0040F7CF
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

	// Token: 0x0600AE6D RID: 44653 RVA: 0x004115F0 File Offset: 0x0040F7F0
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_DecalManager()
	{
		vp_DecalManager.Refresh();
	}

	// Token: 0x0600AE6E RID: 44654 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_DecalManager()
	{
	}

	// Token: 0x0600AE6F RID: 44655 RVA: 0x00411640 File Offset: 0x0040F840
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

	// Token: 0x0600AE70 RID: 44656 RVA: 0x004116A4 File Offset: 0x0040F8A4
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

	// Token: 0x0600AE71 RID: 44657 RVA: 0x004117A4 File Offset: 0x0040F9A4
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

	// Token: 0x0600AE72 RID: 44658 RVA: 0x004117E4 File Offset: 0x0040F9E4
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

	// Token: 0x040081AA RID: 33194
	public static readonly vp_DecalManager instance = new vp_DecalManager();

	// Token: 0x040081AB RID: 33195
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<GameObject> m_Decals = new List<GameObject>();

	// Token: 0x040081AC RID: 33196
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxDecals = 100f;

	// Token: 0x040081AD RID: 33197
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadedDecals = 20f;

	// Token: 0x040081AE RID: 33198
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_NonFadedDecals = 0f;

	// Token: 0x040081AF RID: 33199
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadeAmount = 0f;
}
