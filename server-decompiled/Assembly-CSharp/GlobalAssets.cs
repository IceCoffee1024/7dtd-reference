using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02001241 RID: 4673
public static class GlobalAssets
{
	// Token: 0x060094D6 RID: 38102 RVA: 0x00385FA2 File Offset: 0x003841A2
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> LoadShaderMappings()
	{
		return JsonUtility.FromJson<AssetMappings>(File.ReadAllText(Path.Combine(Addressables.RuntimePath, "shaders.json"))).ToDictionary();
	}

	// Token: 0x060094D7 RID: 38103 RVA: 0x00385FC4 File Offset: 0x003841C4
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

	// Token: 0x04006F8A RID: 28554
	public const string ShaderMappingFile = "shaders.json";

	// Token: 0x04006F8B RID: 28555
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> shaders;
}
