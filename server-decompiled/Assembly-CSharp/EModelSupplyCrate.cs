using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000520 RID: 1312
[Preserve]
public class EModelSupplyCrate : EModelBase
{
	// Token: 0x06002AEA RID: 10986 RVA: 0x0011023A File Offset: 0x0010E43A
	public override void Init(World _world, Entity _entity, EModelInstanceAssets _assets)
	{
		base.Init(_world, _entity, _assets);
		this.parachute = base.transform.FindInChilds("parachute_supplies", false);
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x0011025C File Offset: 0x0010E45C
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

	// Token: 0x040020D9 RID: 8409
	public Transform parachute;
}
