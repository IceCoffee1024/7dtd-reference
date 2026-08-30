using System;
using UnityEngine;

// Token: 0x0200126B RID: 4715
public class DynamicUIAtlasAssigner : MonoBehaviour
{
	// Token: 0x060095C3 RID: 38339 RVA: 0x00386360 File Offset: 0x00384560
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

	// Token: 0x060095C4 RID: 38340 RVA: 0x0038641F File Offset: 0x0038461F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDestroy()
	{
		if (this.atlas != null)
		{
			this.atlas.AtlasUpdatedEv -= this.AtlasUpdateCallback;
		}
	}

	// Token: 0x060095C5 RID: 38341 RVA: 0x00386448 File Offset: 0x00384648
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

	// Token: 0x04007017 RID: 28695
	public string AtlasPathInScene;

	// Token: 0x04007018 RID: 28696
	public string OptionalSpriteName;

	// Token: 0x04007019 RID: 28697
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public DynamicUIAtlas atlas;

	// Token: 0x0400701A RID: 28698
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UISprite[] sprites;
}
