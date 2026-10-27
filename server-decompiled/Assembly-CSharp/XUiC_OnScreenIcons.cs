using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FBC RID: 4028
[Preserve]
public class XUiC_OnScreenIcons : XUiController
{
	// Token: 0x06007E92 RID: 32402 RVA: 0x00301B0A File Offset: 0x002FFD0A
	public override void Init()
	{
		base.Init();
		NavObjectManager.Instance.OnNavObjectAdded += this.Instance_OnNavObjectAdded;
		NavObjectManager.Instance.OnNavObjectRemoved += this.Instance_OnNavObjectRemoved;
		this.xui.OnScreenIconsWindow = this;
	}

	// Token: 0x06007E93 RID: 32403 RVA: 0x00301B4A File Offset: 0x002FFD4A
	public override void Cleanup()
	{
		base.Cleanup();
		this.screenIconList.Clear();
		this.disabledIcons.Clear();
		this.xui.OnScreenIconsWindow = null;
	}

	// Token: 0x06007E94 RID: 32404 RVA: 0x00301B74 File Offset: 0x002FFD74
	[PublicizedFrom(EAccessModifier.Private)]
	public void Instance_OnNavObjectRemoved(NavObject newNavObject)
	{
		if (newNavObject.HasOnScreen)
		{
			this.UnRegisterIcon(newNavObject);
		}
	}

	// Token: 0x06007E95 RID: 32405 RVA: 0x00301B85 File Offset: 0x002FFD85
	[PublicizedFrom(EAccessModifier.Private)]
	public void Instance_OnNavObjectAdded(NavObject newNavObject)
	{
		if (newNavObject.HasOnScreen)
		{
			this.RegisterIcon(newNavObject);
		}
	}

	// Token: 0x06007E96 RID: 32406 RVA: 0x00301B98 File Offset: 0x002FFD98
	public override void Update(float _dt)
	{
		if (this.screenIconList.Count == 0 || !XUiC_OnScreenIcons.ShowIcons)
		{
			return;
		}
		Vector3 vector = new Vector3((float)(-(float)base.ViewComponent.Size.x) * 0.5f, (float)(-(float)base.ViewComponent.Size.y) * 0.5f, 0f);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		Vector3 forward = entityPlayer.cameraTransform.forward;
		Vector3 position = entityPlayer.GetPosition();
		int num = 300;
		for (int i = this.screenIconList.Count - 1; i >= 0; i--)
		{
			XUiC_OnScreenIcons.OnScreenIcon onScreenIcon = this.screenIconList[i];
			onScreenIcon.Update(vector, position, forward, this.xui, ref num);
			if (onScreenIcon.ReadyForUnload)
			{
				this.screenIconList.RemoveAt(i);
				this.disabledIcons.Add(onScreenIcon);
				onScreenIcon.Transform.gameObject.SetActive(false);
			}
		}
		base.Update(_dt);
	}

	// Token: 0x06007E97 RID: 32407 RVA: 0x00301C97 File Offset: 0x002FFE97
	public override void OnOpen()
	{
		base.OnOpen();
		this.Camera = this.xui.playerUI.entityPlayer.playerCamera;
	}

	// Token: 0x06007E98 RID: 32408 RVA: 0x002D16E9 File Offset: 0x002CF8E9
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x06007E99 RID: 32409 RVA: 0x00301CBC File Offset: 0x002FFEBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RegisterIcon(NavObject newNavObject)
	{
		if (!XUiC_OnScreenIcons.ShowIcons)
		{
			return;
		}
		for (int i = 0; i < this.screenIconList.Count; i++)
		{
			if (this.screenIconList[i].NavObject == newNavObject)
			{
				return;
			}
		}
		XUiC_OnScreenIcons.OnScreenIcon onScreenIcon = this.CreateIcon();
		onScreenIcon.Owner = this;
		onScreenIcon.NavObject = newNavObject;
		onScreenIcon.Init();
	}

	// Token: 0x06007E9A RID: 32410 RVA: 0x00301D18 File Offset: 0x002FFF18
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OnScreenIcons.OnScreenIcon CreateIcon()
	{
		XUiC_OnScreenIcons.OnScreenIcon onScreenIcon;
		if (this.disabledIcons.Count > 0)
		{
			onScreenIcon = this.disabledIcons[0];
			this.disabledIcons.RemoveAt(0);
			onScreenIcon.ReadyForUnload = false;
			if (onScreenIcon.Transform)
			{
				onScreenIcon.Sprite.color = Color.clear;
				onScreenIcon.Sprite.spriteName = "";
			}
		}
		else
		{
			onScreenIcon = new XUiC_OnScreenIcons.OnScreenIcon();
		}
		this.screenIconList.Add(onScreenIcon);
		return onScreenIcon;
	}

	// Token: 0x06007E9B RID: 32411 RVA: 0x00301D98 File Offset: 0x002FFF98
	public void UnRegisterIcon(NavObject navObject)
	{
		if (!XUiC_OnScreenIcons.ShowIcons)
		{
			return;
		}
		for (int i = this.screenIconList.Count - 1; i >= 0; i--)
		{
			XUiC_OnScreenIcons.OnScreenIcon onScreenIcon = this.screenIconList[i];
			if (onScreenIcon.NavObject == navObject)
			{
				if (!this.disabledIcons.Contains(onScreenIcon))
				{
					this.disabledIcons.Add(onScreenIcon);
					if (onScreenIcon.Transform)
					{
						onScreenIcon.Transform.gameObject.SetActive(false);
					}
				}
				this.screenIconList.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04005DE8 RID: 24040
	public List<XUiC_OnScreenIcons.OnScreenIcon> screenIconList = new List<XUiC_OnScreenIcons.OnScreenIcon>();

	// Token: 0x04005DE9 RID: 24041
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_OnScreenIcons.OnScreenIcon> disabledIcons = new List<XUiC_OnScreenIcons.OnScreenIcon>();

	// Token: 0x04005DEA RID: 24042
	public Camera Camera;

	// Token: 0x04005DEB RID: 24043
	public static bool ShowIcons = true;

	// Token: 0x02000FBD RID: 4029
	public class OnScreenIcon
	{
		// Token: 0x06007E9E RID: 32414 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init()
		{
		}

		// Token: 0x06007E9F RID: 32415 RVA: 0x00301E48 File Offset: 0x00300048
		public void Update(Vector3 offset, Vector3 playerPosition, Vector3 cameraForward, XUi xui, ref int depth)
		{
			if (!this.NavObject.IsValid())
			{
				return;
			}
			if (this.Sprite)
			{
				this.Sprite.enabled = (this.Label.enabled = !this.NavObject.hiddenOnCompass);
				depth = this.UpdateDepth(depth) + 1;
			}
			this.ScreenSettings = this.NavObject.CurrentScreenSettings;
			if (this.ScreenSettings == null)
			{
				this.HideObjects();
				return;
			}
			EntityPlayerLocal entityPlayer = xui.playerUI.entityPlayer;
			if (entityPlayer.IsDead())
			{
				this.HideObjects();
				return;
			}
			float num = Vector3.Distance(this.NavObject.GetPosition(), entityPlayer.position - Origin.position);
			float maxDistance = this.NavObject.GetMaxDistance(this.ScreenSettings, entityPlayer);
			if (maxDistance != -1f && num > maxDistance)
			{
				this.HideObjects();
				return;
			}
			if (num < this.ScreenSettings.MinDistance)
			{
				this.HideObjects();
				return;
			}
			this.CreateObjects();
			string spriteName = this.NavObject.GetSpriteName(this.ScreenSettings);
			if (this.Sprite.spriteName != spriteName)
			{
				this.Sprite.atlas = xui.GetAtlasByName("UIAtlas", spriteName);
				this.Sprite.spriteName = spriteName;
			}
			Color color = (this.NavObject.UseOverrideColor ? this.NavObject.OverrideColor : this.ScreenSettings.Color);
			this.Sprite.color = color;
			Vector3 vector = this.NavObject.GetPosition() + this.ScreenSettings.Offset;
			if (this.ScreenSettings.UseHeadOffset && this.NavObject.TrackType == NavObject.TrackTypes.Entity)
			{
				vector.y += this.NavObject.TrackedEntity.GetEyeHeight();
				vector += this.NavObject.TrackedEntity.EntityClass.NavObjectHeadOffset;
			}
			Vector3 vector2 = vector - this.Owner.Camera.transform.position;
			vector2.Normalize();
			Vector3 vector3 = entityPlayer.finalCamera.WorldToScreenPoint(vector);
			if (this.ScreenSettings.ShowOffScreen)
			{
				if (vector3.x < 30f)
				{
					vector3.x = 30f;
				}
				float num2 = ((this.ScreenSettings.ShowTextType != NavObjectScreenSettings.ShowTextTypes.None) ? 30f : 0f);
				if (vector3.y < 30f + num2)
				{
					vector3.y = 30f + num2;
				}
				if (vector3.x > (float)(Screen.width - 30))
				{
					vector3.x = (float)(Screen.width - 30);
				}
				if (vector3.y > (float)(Screen.height - 30))
				{
					vector3.y = (float)(Screen.height - 30);
				}
				if (Vector3.Dot(vector2, cameraForward) < 0f)
				{
					if (vector3.y < (float)(Screen.height / 2))
					{
						vector3.y = (float)(Screen.height - 30);
					}
					else
					{
						vector3.y = 30f + num2;
					}
					vector3.x = (float)Screen.width - vector3.x - 30f;
				}
			}
			else if (Vector3.Dot(vector2, cameraForward) < 0f)
			{
				this.HideObjects();
				return;
			}
			Vector3 vector4 = xui.TranslateScreenVectorToXuiVector(vector3);
			vector4.z = 0f;
			this.Transform.localPosition = vector4;
			this.Transform.gameObject.SetActive(true);
			if (maxDistance != -1f)
			{
				float num3 = 1f;
				if (num >= maxDistance)
				{
					num3 = 0f;
				}
				else if (num >= this.ScreenSettings.FadeEndDistance)
				{
					num3 = 1f - Utils.FastClamp01((num - this.ScreenSettings.FadeEndDistance) / (maxDistance - this.ScreenSettings.FadeEndDistance));
					if (num3 < 1f)
					{
						this.Sprite.color = color;
					}
				}
				this.Sprite.alpha = num3;
			}
			else
			{
				this.Sprite.alpha = 1f;
			}
			float num4 = 1f;
			if (num <= maxDistance - this.ScreenSettings.FadeEndDistance && this.ScreenSettings.HasPulse)
			{
				float num5 = Mathf.PingPong(Time.time, 0.5f);
				this.Sprite.color = Color.Lerp(Color.grey, color, num5 * 4f);
				if (num5 > 0.25f)
				{
					num4 += num5 - 0.25f;
				}
			}
			int num6 = (int)(num4 * this.ScreenSettings.SpriteSize);
			this.Sprite.SetDimensions(num6, num6);
			if (this.ScreenSettings.SpriteFillType != NavObjectScreenSettings.SpriteFillTypes.None && this.NavObject.TrackedEntity != null)
			{
				if (this.FillSprite == null)
				{
					this.SetupFillSprite();
				}
				this.FillSprite.color = this.ScreenSettings.SpriteFillColor;
				this.FillSprite.alpha = this.Sprite.alpha;
				this.FillSprite.spriteName = this.ScreenSettings.SpriteFillName;
				this.FillSprite.SetDimensions(num6, num6);
				if (this.ScreenSettings.SpriteFillType == NavObjectScreenSettings.SpriteFillTypes.Health)
				{
					this.FillSprite.fillAmount = ((EntityAlive)this.NavObject.TrackedEntity).Stats.Health.ValuePercent;
				}
			}
			else
			{
				this.RemoveFillSprite();
			}
			if (this.ScreenSettings.SubSpriteName != null)
			{
				if (this.SubSprite == null)
				{
					this.SetupSubSprite();
				}
				int num7 = (int)this.ScreenSettings.SubSpriteSize;
				this.SubSprite.transform.localPosition = this.ScreenSettings.SubSpriteOffset;
				this.SubSprite.SetDimensions(num7, num7);
				this.SubSprite.spriteName = this.ScreenSettings.SubSpriteName;
			}
			else
			{
				this.RemoveSubSprite();
			}
			if (this.Label != null)
			{
				if (this.ScreenSettings.ShowTextType != NavObjectScreenSettings.ShowTextTypes.None)
				{
					this.Label.alpha = this.Sprite.alpha;
					this.Label.fontSize = this.ScreenSettings.FontSize;
					this.Label.color = (this.NavObject.UseOverrideFontColor ? this.NavObject.OverrideColor : this.ScreenSettings.FontColor);
					if (this.ScreenSettings.ShowTextType == NavObjectScreenSettings.ShowTextTypes.Distance)
					{
						string text = "m";
						if (num >= 1000f)
						{
							num /= 1000f;
							text = "km";
						}
						this.Label.text = string.Format("{0} {1}", num.ToCultureInvariantString("0.0"), text);
						return;
					}
					if (this.ScreenSettings.ShowTextType == NavObjectScreenSettings.ShowTextTypes.Name)
					{
						this.Label.text = this.NavObject.DisplayName;
						return;
					}
					this.Label.text = ((this.NavObject.TrackedEntity != null) ? this.NavObject.TrackedEntity.spawnByName : "");
					return;
				}
				else
				{
					this.Label.text = "";
				}
			}
		}

		// Token: 0x06007EA0 RID: 32416 RVA: 0x00302548 File Offset: 0x00300748
		public void CreateObjects()
		{
			if (this.Sprite)
			{
				return;
			}
			GameObject gameObject = new GameObject("ScreenIcon");
			Transform transform = gameObject.transform;
			this.Transform = transform;
			transform.SetParent(this.Owner.ViewComponent.UiTransform, false);
			gameObject.layer = 12;
			GameObject gameObject2 = new GameObject("Sprite");
			gameObject2.transform.SetParent(transform, false);
			gameObject2.layer = 12;
			UISprite uisprite = gameObject2.AddComponent<UISprite>();
			this.Sprite = uisprite;
			uisprite.atlas = this.Owner.xui.GetAtlasByName("UIAtlas", "menu_empty");
			uisprite.spriteName = "menu_empty";
			uisprite.SetDimensions(50, 50);
			uisprite.color = Color.clear;
			uisprite.pivot = UIWidget.Pivot.Center;
			uisprite.depth = 300;
			GameObject gameObject3 = new GameObject("Label");
			gameObject3.transform.SetParent(transform, false);
			gameObject3.layer = 12;
			UILabel uilabel = gameObject3.AddComponent<UILabel>();
			this.Label = uilabel;
			uilabel.font = this.Owner.xui.GetUIFontByName("ReferenceFont", true);
			uilabel.fontSize = 24;
			uilabel.pivot = UIWidget.Pivot.Center;
			uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			uilabel.alignment = NGUIText.Alignment.Center;
			uilabel.effectStyle = UILabel.Effect.Outline;
			uilabel.effectColor = new Color32(0, 0, 0, byte.MaxValue);
			uilabel.effectDistance = new Vector2(2f, 2f);
			uilabel.color = Color.white;
			uilabel.text = "";
			uilabel.depth = 300;
			uilabel.width = 200;
			this.ScreenSettings = this.NavObject.CurrentScreenSettings;
			uilabel.transform.localPosition = new Vector2(0f, -(this.ScreenSettings.SpriteSize * 0.5f + 8f));
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x0030271F File Offset: 0x0030091F
		[PublicizedFrom(EAccessModifier.Private)]
		public void HideObjects()
		{
			if (this.Transform)
			{
				this.Transform.localPosition = NavObject.InvalidPos;
			}
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x00302740 File Offset: 0x00300940
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetupFillSprite()
		{
			GameObject gameObject = new GameObject("FilledSprite");
			gameObject.layer = 12;
			gameObject.transform.SetParent(this.Transform, false);
			UISprite uisprite = gameObject.AddComponent<UISprite>();
			this.FillSprite = uisprite;
			uisprite.atlas = this.Owner.xui.GetAtlasByName("UIAtlas", "menu_empty");
			uisprite.spriteName = "menu_empty";
			uisprite.SetDimensions(50, 50);
			uisprite.color = Color.clear;
			uisprite.pivot = UIWidget.Pivot.Center;
			uisprite.fillDirection = UIBasicSprite.FillDirection.Radial360;
			uisprite.type = UIBasicSprite.Type.Filled;
			uisprite.depth = 300;
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x003027E0 File Offset: 0x003009E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetupSubSprite()
		{
			GameObject gameObject = new GameObject("SubSprite");
			gameObject.layer = 12;
			Transform transform = gameObject.transform;
			transform.SetParent(this.Transform, false);
			transform.localPosition = this.ScreenSettings.SubSpriteOffset;
			UISprite uisprite = gameObject.AddComponent<UISprite>();
			this.SubSprite = uisprite;
			uisprite.atlas = this.Owner.xui.GetAtlasByName("UIAtlas", "menu_empty");
			uisprite.spriteName = "menu_empty";
			int num = (int)this.ScreenSettings.SubSpriteSize;
			uisprite.SetDimensions(num, num);
			uisprite.color = Color.clear;
			uisprite.pivot = UIWidget.Pivot.Center;
			uisprite.depth = 300;
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x00302891 File Offset: 0x00300A91
		public void RemoveFillSprite()
		{
			if (this.FillSprite != null)
			{
				global::UnityEngine.Object.Destroy(this.FillSprite.gameObject);
				this.FillSprite = null;
			}
		}

		// Token: 0x06007EA5 RID: 32421 RVA: 0x003028B8 File Offset: 0x00300AB8
		public void RemoveSubSprite()
		{
			if (this.SubSprite != null)
			{
				global::UnityEngine.Object.Destroy(this.SubSprite.gameObject);
				this.SubSprite = null;
			}
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x003028E0 File Offset: 0x00300AE0
		public int UpdateDepth(int depth)
		{
			this.Sprite.depth = depth;
			this.Label.depth = depth;
			if (this.FillSprite != null)
			{
				depth = (this.FillSprite.depth = depth + 1);
			}
			if (this.SubSprite != null)
			{
				depth = (this.SubSprite.depth = depth + 1);
			}
			return depth;
		}

		// Token: 0x04005DEC RID: 24044
		public XUiC_OnScreenIcons Owner;

		// Token: 0x04005DED RID: 24045
		public NavObject NavObject;

		// Token: 0x04005DEE RID: 24046
		public Transform Transform;

		// Token: 0x04005DEF RID: 24047
		public UISprite Sprite;

		// Token: 0x04005DF0 RID: 24048
		public UISprite FillSprite;

		// Token: 0x04005DF1 RID: 24049
		public UISprite SubSprite;

		// Token: 0x04005DF2 RID: 24050
		public UILabel Label;

		// Token: 0x04005DF3 RID: 24051
		public bool ReadyForUnload;

		// Token: 0x04005DF4 RID: 24052
		public NavObjectScreenSettings ScreenSettings;
	}
}
