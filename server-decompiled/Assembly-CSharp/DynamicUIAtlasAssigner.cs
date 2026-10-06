using System;
using UnityEngine;

// Token: 0x0200129D RID: 4765
public class DynamicUIAtlasAssigner : MonoBehaviour
{
	// Token: 0x0600974B RID: 38731 RVA: 0x0038FD04 File Offset: 0x0038DF04
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		GameObject gameObject = GameObject.Find(this.AtlasPathInScene);
		if (gameObject == null)
		{
			Log.Warning("Could not assign atlas: Atlas object not found");
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		this.atlas = gameObject.GetComponent<DynamicUIAtlas>();
		if (this.atlas == null)
		{
			Log.Warning("Could not assign atlas: Atlas component not found");
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		this.atlas.AtlasUpdatedEv += this.AtlasUpdateCallback;
		this.sprites = base.GetComponents<UISprite>();
		foreach (UISprite uisprite in this.sprites)
		{
			uisprite.atlas = this.atlas;
			if (!string.IsNullOrEmpty(this.OptionalSpriteName))
			{
				uisprite.spriteName = this.OptionalSpriteName;
			}
		}
	}

	// Token: 0x0600974C RID: 38732 RVA: 0x0038FDC3 File Offset: 0x0038DFC3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDestroy()
	{
		if (this.atlas != null)
		{
			this.atlas.AtlasUpdatedEv -= this.AtlasUpdateCallback;
		}
	}

	// Token: 0x0600974D RID: 38733 RVA: 0x0038FDEC File Offset: 0x0038DFEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void AtlasUpdateCallback()
	{
		if (!string.IsNullOrEmpty(this.OptionalSpriteName))
		{
			foreach (UISprite uisprite in this.sprites)
			{
				uisprite.spriteName = null;
				uisprite.spriteName = this.OptionalSpriteName;
			}
		}
	}

	// Token: 0x04007156 RID: 29014
	public string AtlasPathInScene;

	// Token: 0x04007157 RID: 29015
	public string OptionalSpriteName;

	// Token: 0x04007158 RID: 29016
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public DynamicUIAtlas atlas;

	// Token: 0x04007159 RID: 29017
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UISprite[] sprites;
}
