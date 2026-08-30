using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200126D RID: 4717
public class MultiSourceAtlasManager : MonoBehaviour
{
	// Token: 0x060095CE RID: 38350 RVA: 0x00386E28 File Offset: 0x00385028
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

	// Token: 0x060095CF RID: 38351 RVA: 0x00386E70 File Offset: 0x00385070
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

	// Token: 0x060095D0 RID: 38352 RVA: 0x00386EB8 File Offset: 0x003850B8
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

	// Token: 0x060095D1 RID: 38353 RVA: 0x00386F34 File Offset: 0x00385134
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

	// Token: 0x060095D2 RID: 38354 RVA: 0x00386FD4 File Offset: 0x003851D4
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

	// Token: 0x0400701B RID: 28699
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<MultiSourceAtlasManager.BaseAtlas> atlases = new List<MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x0400701C RID: 28700
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager.BaseAtlas> atlasesForSprites = new Dictionary<string, MultiSourceAtlasManager.BaseAtlas>();

	// Token: 0x0200126E RID: 4718
	[PublicizedFrom(EAccessModifier.Private)]
	public class BaseAtlas
	{
		// Token: 0x0400701D RID: 28701
		public GameObject Parent;

		// Token: 0x0400701E RID: 28702
		public INGUIAtlas Atlas;

		// Token: 0x0400701F RID: 28703
		public bool IsLoadedInGame;
	}
}
