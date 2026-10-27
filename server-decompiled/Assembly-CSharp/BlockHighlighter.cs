using System;
using UnityEngine;

// Token: 0x0200092C RID: 2348
public static class BlockHighlighter
{
	// Token: 0x06004350 RID: 17232 RVA: 0x001A5DEB File Offset: 0x001A3FEB
	public static void AddBlock(Vector3i _pos)
	{
		BlockHighlighter.EnforceGo();
		BlockHighlighter.EnforceTemplateLoaded();
		global::UnityEngine.Object.Instantiate<GameObject>(BlockHighlighter.blockPrefab, BlockHighlighter.topGameObject.transform).transform.position = _pos.ToVector3() + BlockHighlighter.halfBlockOffset;
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x001A5E26 File Offset: 0x001A4026
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnforceGo()
	{
		if (BlockHighlighter.topGameObject != null)
		{
			return;
		}
		BlockHighlighter.topGameObject = new GameObject("BlockHighlighter");
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x001A5E45 File Offset: 0x001A4045
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnforceTemplateLoaded()
	{
		if (BlockHighlighter.blockPrefab != null)
		{
			return;
		}
		BlockHighlighter.blockPrefab = DataLoader.LoadAsset<GameObject>("@:Entities/Misc/block_highlightPrefab.prefab", false);
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x001A5E65 File Offset: 0x001A4065
	public static void Cleanup()
	{
		if (BlockHighlighter.topGameObject != null)
		{
			global::UnityEngine.Object.Destroy(BlockHighlighter.topGameObject);
			BlockHighlighter.topGameObject = null;
		}
	}

	// Token: 0x040036B7 RID: 14007
	[PublicizedFrom(EAccessModifier.Private)]
	public const string TemplatePath = "@:Entities/Misc/block_highlightPrefab.prefab";

	// Token: 0x040036B8 RID: 14008
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 halfBlockOffset = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x040036B9 RID: 14009
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject topGameObject;

	// Token: 0x040036BA RID: 14010
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject blockPrefab;
}
