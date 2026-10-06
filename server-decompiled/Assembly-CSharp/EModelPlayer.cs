using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200051D RID: 1309
[Preserve]
public class EModelPlayer : EModelBase
{
	// Token: 0x06002AD2 RID: 10962 RVA: 0x0010FBF0 File Offset: 0x0010DDF0
	public override void SetSkinTexture(string _textureName)
	{
		base.SetSkinTexture(_textureName);
		Transform transform = this.modelTransformParent.Find(this.modelTransformParent.GetChild(0).name);
		Transform transform2;
		if (transform != null && (transform2 = transform.Find("body")) != null)
		{
			Texture2D texture2D = DataLoader.LoadAsset<Texture2D>(DataLoader.IsInResources(_textureName) ? ("Entities/" + _textureName) : _textureName, false);
			transform2.GetComponent<Renderer>().material.mainTexture = texture2D;
		}
	}
}
