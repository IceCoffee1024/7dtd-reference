using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200129F RID: 4767
public class MultiSourceAtlasManager : MonoBehaviour
{
	// Token: 0x06009756 RID: 38742 RVA: 0x003907CC File Offset: 0x0038E9CC
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

	// Token: 0x06009757 RID: 38743 RVA: 0x00390814 File Offset: 0x0038EA14
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

	// Token: 0x06009758 RID: 38744 RVA: 0x0039085C File Offset: 0x0038EA5C
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

	// Token: 0x06009759 RID: 38745 RVA: 0x003908D8 File Offset: 0x0038EAD8
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

	// Token: 0x0600975A RID: 38746 RVA: 0x00390978 File Offset: 0x0038EB78
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

	// Token: 0x0400715A RID: 29018
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<MultiSourceAtlasManager.BaseAtlas> atlases = new List<MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x0400715B RID: 29019
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager.BaseAtlas> atlasesForSprites = new Dictionary<string, MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x020012A0 RID: 4768
	[PublicizedFrom(EAccessModifier.Private)]
	public class BaseAtlas
	{
		// Token: 0x0400715C RID: 29020
		public GameObject Parent;

		// Token: 0x0400715D RID: 29021
		public INGUIAtlas Atlas;

		// Token: 0x0400715E RID: 29022
		public bool IsLoadedInGame;
	}
}
