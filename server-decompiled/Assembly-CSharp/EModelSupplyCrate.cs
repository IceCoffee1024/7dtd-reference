using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000515 RID: 1301
[Preserve]
public class EModelSupplyCrate : EModelBase
{
	// Token: 0x06002ACB RID: 10955 RVA: 0x0010E93A File Offset: 0x0010CB3A
	public override void Init(World _world, Entity _entity, EModelInstanceAssets _assets)
	{
		base.Init(_world, _entity, _assets);
		this.parachute = base.transform.FindInChilds("parachute_supplies", false);
	}

	// Token: 0x06002ACC RID: 10956 RVA: 0x0010E95C File Offset: 0x0010CB5C
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

	// Token: 0x040020A2 RID: 8354
	public Transform parachute;
}
