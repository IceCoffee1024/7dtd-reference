using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001287 RID: 4743
public class MultiSourceAtlasManager : MonoBehaviour
{
	// Token: 0x060096C9 RID: 38601 RVA: 0x0038BF84 File Offset: 0x0038A184
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

	// Token: 0x060096CA RID: 38602 RVA: 0x0038BFCC File Offset: 0x0038A1CC
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

	// Token: 0x060096CB RID: 38603 RVA: 0x0038C014 File Offset: 0x0038A214
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

	// Token: 0x060096CC RID: 38604 RVA: 0x0038C090 File Offset: 0x0038A290
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

	// Token: 0x060096CD RID: 38605 RVA: 0x0038C130 File Offset: 0x0038A330
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

	// Token: 0x040070F0 RID: 28912
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<MultiSourceAtlasManager.BaseAtlas> atlases = new List<MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x040070F1 RID: 28913
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager.BaseAtlas> atlasesForSprites = new Dictionary<string, MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x02001288 RID: 4744
	[PublicizedFrom(EAccessModifier.Private)]
	public class BaseAtlas
	{
		// Token: 0x040070F2 RID: 28914
		public GameObject Parent;

		// Token: 0x040070F3 RID: 28915
		public INGUIAtlas Atlas;

		// Token: 0x040070F4 RID: 28916
		public bool IsLoadedInGame;
	}
}
