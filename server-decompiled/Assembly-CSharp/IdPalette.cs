using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001376 RID: 4982
[CreateAssetMenu(fileName = "NewIdPalette", menuName = "ID Maps/ID Palette", order = 0)]
public class IdPalette : ScriptableObject
{
	// Token: 0x170012D1 RID: 4817
	// (get) Token: 0x06009CD7 RID: 40151 RVA: 0x003B40AB File Offset: 0x003B22AB
	public static IdPalette Instance
	{
		get
		{
			if (IdPalette._instance)
			{
				return IdPalette._instance;
			}
			IdPalette._instance = Resources.Load<IdPalette>("IdPalette");
			if (IdPalette._instance == null)
			{
				Debug.LogError("IdPalette.Instance: Couldn't find Resources/IdPalette.asset. Create one IdPalette and place it under a Resources folder.");
			}
			return IdPalette._instance;
		}
	}

	// Token: 0x06009CD8 RID: 40152 RVA: 0x003B40EA File Offset: 0x003B22EA
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ResetStatic()
	{
		IdPalette._instance = null;
	}

	// Token: 0x06009CD9 RID: 40153 RVA: 0x003B40F2 File Offset: 0x003B22F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (IdPalette._instance != null && IdPalette._instance != this)
		{
			Debug.LogWarning("Multiple IdPalette instances loaded. Using the first one: " + IdPalette._instance.name);
			return;
		}
		IdPalette._instance = this;
	}

	// Token: 0x0400764B RID: 30283
	public List<PaletteEntry> entries = new List<PaletteEntry>();

	// Token: 0x0400764C RID: 30284
	public const string ResourcesPath = "IdPalette";

	// Token: 0x0400764D RID: 30285
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static IdPalette _instance;
}
