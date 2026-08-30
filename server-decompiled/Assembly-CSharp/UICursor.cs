using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/Examples/UI Cursor")]
public class UICursor : MonoBehaviour
{
	// Token: 0x06000158 RID: 344 RVA: 0x0000EAD7 File Offset: 0x0000CCD7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		UICursor.instance = this;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000EADF File Offset: 0x0000CCDF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		UICursor.instance = null;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mTrans = base.transform;
		this.mSprite = base.GetComponentInChildren<UISprite>();
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mSprite != null)
		{
			this.mAtlas = this.mSprite.atlas;
			this.mSpriteName = this.mSprite.spriteName;
			if (this.mSprite.depth < 100)
			{
				this.mSprite.depth = 100;
			}
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000EB80 File Offset: 0x0000CD80
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (this.uiCamera != null)
		{
			mousePosition.x = Mathf.Clamp01(mousePosition.x / (float)Screen.width);
			mousePosition.y = Mathf.Clamp01(mousePosition.y / (float)Screen.height);
			this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
			if (this.uiCamera.orthographic)
			{
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
				return;
			}
		}
		else
		{
			mousePosition.x -= (float)Screen.width * 0.5f;
			mousePosition.y -= (float)Screen.height * 0.5f;
			mousePosition.x = Mathf.Round(mousePosition.x);
			mousePosition.y = Mathf.Round(mousePosition.y);
			this.mTrans.localPosition = mousePosition;
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000EC98 File Offset: 0x0000CE98
	public static void Clear()
	{
		if (UICursor.instance != null && UICursor.instance.mSprite != null)
		{
			UICursor.Set(UICursor.instance.mAtlas, UICursor.instance.mSpriteName);
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000ECD4 File Offset: 0x0000CED4
	public static void Set(INGUIAtlas atlas, string sprite)
	{
		if (UICursor.instance != null && UICursor.instance.mSprite)
		{
			UICursor.instance.mSprite.atlas = atlas;
			UICursor.instance.mSprite.spriteName = sprite;
			UICursor.instance.mSprite.MakePixelPerfect();
			UICursor.instance.Update();
		}
	}

	// Token: 0x040001EA RID: 490
	public static UICursor instance;

	// Token: 0x040001EB RID: 491
	public Camera uiCamera;

	// Token: 0x040001EC RID: 492
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x040001ED RID: 493
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UISprite mSprite;

	// Token: 0x040001EE RID: 494
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public INGUIAtlas mAtlas;

	// Token: 0x040001EF RID: 495
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string mSpriteName;
}
