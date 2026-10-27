using System;
using UnityEngine;

// Token: 0x020012AF RID: 4783
public class DynamicUIAtlasAssigner : MonoBehaviour
{
	// Token: 0x060097A3 RID: 38819 RVA: 0x0039210C File Offset: 0x0039030C
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

	// Token: 0x060097A4 RID: 38820 RVA: 0x003921CB File Offset: 0x003903CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDestroy()
	{
		if (this.atlas != null)
		{
			this.atlas.AtlasUpdatedEv -= this.AtlasUpdateCallback;
		}
	}

	// Token: 0x060097A5 RID: 38821 RVA: 0x003921F4 File Offset: 0x003903F4
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

	// Token: 0x04007197 RID: 29079
	public string AtlasPathInScene;

	// Token: 0x04007198 RID: 29080
	public string OptionalSpriteName;

	// Token: 0x04007199 RID: 29081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public DynamicUIAtlas atlas;

	// Token: 0x0400719A RID: 29082
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UISprite[] sprites;
}
