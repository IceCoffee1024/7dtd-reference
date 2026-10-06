using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001364 RID: 4964
[CreateAssetMenu(fileName = "NewIdPalette", menuName = "ID Maps/ID Palette", order = 0)]
public class IdPalette : ScriptableObject
{
	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x06009C7F RID: 40063 RVA: 0x003B1CA7 File Offset: 0x003AFEA7
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

	// Token: 0x06009C80 RID: 40064 RVA: 0x003B1CE6 File Offset: 0x003AFEE6
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ResetStatic()
	{
		IdPalette._instance = null;
	}

	// Token: 0x06009C81 RID: 40065 RVA: 0x003B1CEE File Offset: 0x003AFEEE
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

	// Token: 0x0400760A RID: 30218
	public List<PaletteEntry> entries = new List<PaletteEntry>();

	// Token: 0x0400760B RID: 30219
	public const string ResourcesPath = "IdPalette";

	// Token: 0x0400760C RID: 30220
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static IdPalette _instance;
}
