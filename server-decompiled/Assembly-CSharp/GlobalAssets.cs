using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200122F RID: 4655
public static class GlobalAssets
{
	// Token: 0x0600947F RID: 38015 RVA: 0x00383BA5 File Offset: 0x00381DA5
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> LoadShaderMappings()
	{
		return JsonUtility.FromJson<AssetMappings>(File.ReadAllText(Path.Combine(Addressables.RuntimePath, "shaders.json"))).ToDictionary();
	}

	// Token: 0x06009480 RID: 38016 RVA: 0x00383BC8 File Offset: 0x00381DC8
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

	// Token: 0x04006F49 RID: 28489
	public const string ShaderMappingFile = "shaders.json";

	// Token: 0x04006F4A RID: 28490
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> shaders;
}
