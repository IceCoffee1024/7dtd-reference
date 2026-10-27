using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012B1 RID: 4785
public class MultiSourceAtlasManager : MonoBehaviour
{
	// Token: 0x060097AE RID: 38830 RVA: 0x00392BD4 File Offset: 0x00390DD4
	public INGUIAtlas GetAtlasForSprite(string _spriteName)
	{
		MultiSourceAtlasManager.BaseAtlas baseAtlas;
		if (this.atlasesForSprites.TryGetValue(_spriteName, out baseAtlas))
		{
			return baseAtlas.Atlas;
		}
		if (this.atlases.Count <= 0)
		{
			return null;
		}
		return this.atlases[0].Atlas;
	}

	// Token: 0x060097AF RID: 38831 RVA: 0x00392C1C File Offset: 0x00390E1C
	public void AddAtlas(INGUIAtlas _atlas, GameObject _gameObject, bool _isLoadingInGame)
	{
		MultiSourceAtlasManager.BaseAtlas baseAtlas = new MultiSourceAtlasManager.BaseAtlas
		{
			Parent = _gameObject,
			Atlas = _atlas,
			IsLoadedInGame = _isLoadingInGame
		};
		this.atlases.Add(baseAtlas);
		_atlas.Name = base.name;
		this.recalcSpriteSources();
	}

	// Token: 0x060097B0 RID: 38832 RVA: 0x00392C64 File Offset: 0x00390E64
	public void CleanupAfterGame()
	{
		for (int i = this.atlases.Count - 1; i >= 0; i--)
		{
			MultiSourceAtlasManager.BaseAtlas baseAtlas = this.atlases[i];
			if (baseAtlas.IsLoadedInGame)
			{
				this.atlases.RemoveAt(i);
				global::UnityEngine.Object.Destroy(baseAtlas.Atlas.spriteMaterial.mainTexture);
				if (baseAtlas.Parent != null)
				{
					global::UnityEngine.Object.Destroy(baseAtlas.Parent);
				}
			}
		}
		this.recalcSpriteSources();
	}

	// Token: 0x060097B1 RID: 38833 RVA: 0x00392CE0 File Offset: 0x00390EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void recalcSpriteSources()
	{
		foreach (MultiSourceAtlasManager.BaseAtlas baseAtlas in this.atlases)
		{
			foreach (UISpriteData uispriteData in baseAtlas.Atlas.spriteList)
			{
				this.atlasesForSprites[uispriteData.name] = baseAtlas;
			}
		}
	}

	// Token: 0x060097B2 RID: 38834 RVA: 0x00392D80 File Offset: 0x00390F80
	public static MultiSourceAtlasManager Create(GameObject _parent, string _name)
	{
		return new GameObject(_name)
		{
			transform = 
			{
				parent = _parent.transform
			}
		}.AddComponent<MultiSourceAtlasManager>();
	}

	// Token: 0x0400719B RID: 29083
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<MultiSourceAtlasManager.BaseAtlas> atlases = new List<MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x0400719C RID: 29084
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager.BaseAtlas> atlasesForSprites = new Dictionary<string, MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x020012B2 RID: 4786
	[PublicizedFrom(EAccessModifier.Private)]
	public class BaseAtlas
	{
		// Token: 0x0400719D RID: 29085
		public GameObject Parent;

		// Token: 0x0400719E RID: 29086
		public INGUIAtlas Atlas;

		// Token: 0x0400719F RID: 29087
		public bool IsLoadedInGame;
	}
}
