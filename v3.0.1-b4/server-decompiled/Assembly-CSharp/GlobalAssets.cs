using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020011FB RID: 4603
public static class GlobalAssets
{
	// Token: 0x060092ED RID: 37613 RVA: 0x0037A081 File Offset: 0x00378281
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> LoadShaderMappings()
	{
		return JsonUtility.FromJson<AssetMappings>(File.ReadAllText(Path.Combine(Addressables.RuntimePath, "shaders.json"))).ToDictionary();
	}

	// Token: 0x060092EE RID: 37614 RVA: 0x0037A0A4 File Offset: 0x003782A4
	public static Shader FindShader(string name)
	{
		if (GlobalAssets.shaders == null)
		{
			GlobalAssets.shaders = GlobalAssets.LoadShaderMappings();
		}
		string text;
		if (GlobalAssets.shaders.TryGetValue(name, out text))
		{
			return LoadManager.LoadAssetFromAddressables<Shader>(text, null, null, false, true, false).Asset;
		}
		return Shader.Find(name);
	}

	// Token: 0x04006E06 RID: 28166
	public const string ShaderMappingFile = "shaders.json";

	// Token: 0x04006E07 RID: 28167
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> shaders;
}
