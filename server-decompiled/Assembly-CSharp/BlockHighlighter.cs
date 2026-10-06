using System;
using UnityEngine;

// Token: 0x02000922 RID: 2338
public static class BlockHighlighter
{
	// Token: 0x0600432D RID: 17197 RVA: 0x001A4D43 File Offset: 0x001A2F43
	public static void AddBlock(Vector3i _pos)
	{
		BlockHighlighter.EnforceGo();
		BlockHighlighter.EnforceTemplateLoaded();
		global::UnityEngine.Object.Instantiate<GameObject>(BlockHighlighter.blockPrefab, BlockHighlighter.topGameObject.transform).transform.position = _pos.ToVector3() + BlockHighlighter.halfBlockOffset;
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x001A4D7E File Offset: 0x001A2F7E
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnforceGo()
	{
		if (BlockHighlighter.topGameObject != null)
		{
			return;
		}
		BlockHighlighter.topGameObject = new GameObject("BlockHighlighter");
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x001A4D9D File Offset: 0x001A2F9D
	[PublicizedFrom(EAccessModifier.Private)]
	public static void EnforceTemplateLoaded()
	{
		if (BlockHighlighter.blockPrefab != null)
		{
			return;
		}
		BlockHighlighter.blockPrefab = DataLoader.LoadAsset<GameObject>("@:Entities/Misc/block_highlightPrefab.prefab", false);
	}

	// Token: 0x06004330 RID: 17200 RVA: 0x001A4DBD File Offset: 0x001A2FBD
	public static void Cleanup()
	{
		if (BlockHighlighter.topGameObject != null)
		{
			global::UnityEngine.Object.Destroy(BlockHighlighter.topGameObject);
			BlockHighlighter.topGameObject = null;
		}
	}

	// Token: 0x04003697 RID: 13975
	[PublicizedFrom(EAccessModifier.Private)]
	public const string TemplatePath = "@:Entities/Misc/block_highlightPrefab.prefab";

	// Token: 0x04003698 RID: 13976
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 halfBlockOffset = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x04003699 RID: 13977
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject topGameObject;

	// Token: 0x0400369A RID: 13978
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject blockPrefab;
}
