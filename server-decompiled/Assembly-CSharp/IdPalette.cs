using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001344 RID: 4932
[CreateAssetMenu(fileName = "NewIdPalette", menuName = "ID Maps/ID Palette", order = 0)]
public class IdPalette : ScriptableObject
{
	// Token: 0x17001280 RID: 4736
	// (get) Token: 0x06009BD3 RID: 39891 RVA: 0x003AD12B File Offset: 0x003AB32B
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

	// Token: 0x06009BD4 RID: 39892 RVA: 0x003AD16A File Offset: 0x003AB36A
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ResetStatic()
	{
		IdPalette._instance = null;
	}

	// Token: 0x06009BD5 RID: 39893 RVA: 0x003AD172 File Offset: 0x003AB372
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

	// Token: 0x0400758F RID: 30095
	public List<PaletteEntry> entries = new List<PaletteEntry>();

	// Token: 0x04007590 RID: 30096
	public const string ResourcesPath = "IdPalette";

	// Token: 0x04007591 RID: 30097
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static IdPalette _instance;
}
