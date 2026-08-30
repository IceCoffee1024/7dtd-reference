using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200132A RID: 4906
[CreateAssetMenu(fileName = "NewIdPalette", menuName = "ID Maps/ID Palette", order = 0)]
public class IdPalette : ScriptableObject
{
	// Token: 0x17001247 RID: 4679
	// (get) Token: 0x06009AD8 RID: 39640 RVA: 0x003A7E83 File Offset: 0x003A6083
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

	// Token: 0x06009AD9 RID: 39641 RVA: 0x003A7EC2 File Offset: 0x003A60C2
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ResetStatic()
	{
		IdPalette._instance = null;
	}

	// Token: 0x06009ADA RID: 39642 RVA: 0x003A7ECA File Offset: 0x003A60CA
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

	// Token: 0x040074B7 RID: 29879
	public List<PaletteEntry> entries = new List<PaletteEntry>();

	// Token: 0x040074B8 RID: 29880
	public const string ResourcesPath = "IdPalette";

	// Token: 0x040074B9 RID: 29881
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static IdPalette _instance;
}
