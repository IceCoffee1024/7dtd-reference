using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000522 RID: 1314
[Preserve]
public class EModelSupplyCrate : EModelBase
{
	// Token: 0x06002AF1 RID: 10993 RVA: 0x001107AE File Offset: 0x0010E9AE
	public override void Init(World _world, Entity _entity, EModelInstanceAssets _assets)
	{
		base.Init(_world, _entity, _assets);
		this.parachute = base.transform.FindInChilds("parachute_supplies", false);
	}

	// Token: 0x06002AF2 RID: 10994 RVA: 0x001107D0 File Offset: 0x0010E9D0
	public override void SetSkinTexture(string _texture)
	{
		base.SetSkinTexture(_texture);
		if (_texture == null || _texture.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.modelTransformParent.childCount; i++)
		{
			Transform child = this.modelTransformParent.GetChild(i);
			if (child != this.parachute)
			{
				child.GetComponent<Renderer>().material.mainTexture = DataLoader.LoadAsset<Texture2D>(DataLoader.IsInResources(_texture) ? ("Entities/" + _texture) : _texture, false);
				return;
			}
		}
	}

	// Token: 0x040020EA RID: 8426
	public Transform parachute;
}
