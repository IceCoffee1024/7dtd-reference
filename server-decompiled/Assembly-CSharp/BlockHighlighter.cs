using System;
using UnityEngine;

// Token: 0x02000911 RID: 2321
public static class BlockHighlighter
{
	// Token: 0x06004336 RID: 17206 RVA: 0x001A2B67 File Offset: 0x001A0D67
	public static void AddBlock(Vector3i _pos)
	{
		BlockHighlighter.EnforceGo();
		BlockHighlighter.EnforceTemplateLoaded();
		global::UnityEngine.Object.Instantiate<GameObject>(BlockHighlighter.blockPrefab, BlockHighlighter.topGameObject.transform).transform.position = _pos.ToVector3() + BlockHighlighter.halfBlockOffset;
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x001A2BA2 File Offset: 0x001A0DA2
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnforceGo()
	{
		if (BlockHighlighter.topGameObject != null)
		{
			return;
		}
		BlockHighlighter.topGameObject = new GameObject("BlockHighlighter");
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x001A2BC1 File Offset: 0x001A0DC1
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnforceTemplateLoaded()
	{
		if (BlockHighlighter.blockPrefab != null)
		{
			return;
		}
		BlockHighlighter.blockPrefab = DataLoader.LoadAsset<GameObject>("@:Entities/Misc/block_highlightPrefab.prefab", false);
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x001A2BE1 File Offset: 0x001A0DE1
	public static void Cleanup()
	{
		if (BlockHighlighter.topGameObject != null)
		{
			global::UnityEngine.Object.Destroy(BlockHighlighter.topGameObject);
			BlockHighlighter.topGameObject = null;
		}
	}

	// Token: 0x04003622 RID: 13858
	[PublicizedFrom(EAccessModifier.Private)]
	public const string TemplatePath = "@:Entities/Misc/block_highlightPrefab.prefab";

	// Token: 0x04003623 RID: 13859
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 halfBlockOffset = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04003624 RID: 13860
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject topGameObject;

	// Token: 0x04003625 RID: 13861
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject blockPrefab;
}
