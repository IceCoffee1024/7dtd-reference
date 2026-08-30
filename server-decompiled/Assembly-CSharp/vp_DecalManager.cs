using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A5 RID: 5541
public sealed class vp_DecalManager
{
	// Token: 0x1700140A RID: 5130
	// (get) Token: 0x0600AD5E RID: 44382 RVA: 0x0040A35C File Offset: 0x0040855C
	// (set) Token: 0x0600AD5F RID: 44383 RVA: 0x0040A363 File Offset: 0x00408563
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

	// Token: 0x1700140B RID: 5131
	// (get) Token: 0x0600AD60 RID: 44384 RVA: 0x0040A370 File Offset: 0x00408570
	// (set) Token: 0x0600AD61 RID: 44385 RVA: 0x0040A377 File Offset: 0x00408577
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

	// Token: 0x0600AD62 RID: 44386 RVA: 0x0040A398 File Offset: 0x00408598
	[PublicizedFrom(EAccessModifier.Private)]
	static vp_DecalManager()
	{
		vp_DecalManager.Refresh();
	}

	// Token: 0x0600AD63 RID: 44387 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_DecalManager()
	{
	}

	// Token: 0x0600AD64 RID: 44388 RVA: 0x0040A3E8 File Offset: 0x004085E8
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

	// Token: 0x0600AD65 RID: 44389 RVA: 0x0040A44C File Offset: 0x0040864C
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

	// Token: 0x0600AD66 RID: 44390 RVA: 0x0040A54C File Offset: 0x0040874C
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

	// Token: 0x0600AD67 RID: 44391 RVA: 0x0040A58C File Offset: 0x0040878C
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

	// Token: 0x040080E6 RID: 32998
	public static readonly vp_DecalManager instance = new vp_DecalManager();

	// Token: 0x040080E7 RID: 32999
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<GameObject> m_Decals = new List<GameObject>();

	// Token: 0x040080E8 RID: 33000
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_MaxDecals = 100f;

	// Token: 0x040080E9 RID: 33001
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadedDecals = 20f;

	// Token: 0x040080EA RID: 33002
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_NonFadedDecals = 0f;

	// Token: 0x040080EB RID: 33003
	[PublicizedFrom(EAccessModifier.Private)]
	public static float m_FadeAmount = 0f;
}
