using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB7 RID: 3767
[Preserve]
public class XUiC_BuffPopoutList : XUiController, IEntityUINotificationChanged
{
	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x060075FE RID: 30206 RVA: 0x002C4B79 File Offset: 0x002C2D79
	// (set) Token: 0x060075FF RID: 30207 RVA: 0x002C4B81 File Offset: 0x002C2D81
	public EntityPlayer LocalPlayer
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007600 RID: 30208 RVA: 0x002C4B8C File Offset: 0x002C2D8C
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("item");
		XUiV_Sprite xuiV_Sprite = childById.GetChildById("BarContent").ViewComponent as XUiV_Sprite;
		this.barSpriteSize = new Vector2((float)xuiV_Sprite.Size.x, (float)xuiV_Sprite.Size.y);
		this.PrefabItems = childById.ViewComponent.UiTransform;
		this.height = (float)(childById.ViewComponent.Size.y + 2);
		this.xui.BuffPopoutList = this;
	}

	// Token: 0x06007601 RID: 30209 RVA: 0x002C4C1A File Offset: 0x002C2E1A
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.BuffPopoutList = null;
	}

	// Token: 0x06007602 RID: 30210 RVA: 0x002C4C30 File Offset: 0x002C2E30
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.LocalPlayer == null && XUi.IsGameRunning())
		{
			this.LocalPlayer = this.xui.playerUI.entityPlayer;
		}
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (windowManager.IsHUDEnabled() || (this.xui.DragAndDropWindow.InMenu && windowManager.IsHUDPartialHidden()))
		{
			if (base.ViewComponent.IsVisible && this.LocalPlayer.IsDead())
			{
				base.ViewComponent.IsVisible = false;
			}
			else if (!base.ViewComponent.IsVisible && !this.LocalPlayer.IsDead())
			{
				base.ViewComponent.IsVisible = true;
			}
		}
		else
		{
			base.ViewComponent.IsVisible = false;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			XUiC_BuffPopoutList.Data data = this.items[i];
			if (data.Notification.Buff != null && data.Notification.Buff.Paused)
			{
				this.removeEntry(data.Notification, i);
				this.disabledItems.Add(data.Notification);
			}
			else
			{
				if (data.Notification.DisplayMode == EnumEntityUINotificationDisplayMode.IconPlusCurrentValue)
				{
					UILabel component = data.Item.transform.Find("TextContent").GetComponent<UILabel>();
					string units = data.Notification.Units;
					if (!(units == "%"))
					{
						if (!(units == "°"))
						{
							if (!(units == "cvar"))
							{
								if (!(units == "duration"))
								{
									if (data.Notification.Buff.BuffClass.DisplayValueKey != null)
									{
										if (data.Notification.Buff.BuffClass.DisplayValueFormat == BuffClass.CVarDisplayFormat.Time)
										{
											component.text = string.Format(Localization.Get(data.Notification.Buff.BuffClass.DisplayValueKey, false, null), XUiC_BuffPopoutList.GetCVarValueAsTimeString(data.Notification.CurrentValue));
										}
										else
										{
											component.text = string.Format(Localization.Get(data.Notification.Buff.BuffClass.DisplayValueKey, false, null), data.Notification.CurrentValue);
										}
									}
									else
									{
										component.text = ((int)data.Notification.CurrentValue).ToString();
									}
								}
								else
								{
									component.text = XUiC_BuffPopoutList.GetCVarValueAsTimeString(data.Notification.Buff.BuffClass.DurationMax - data.Notification.Buff.DurationInSeconds);
								}
							}
							else
							{
								BuffClass buffClass = data.Notification.Buff.BuffClass;
								if (buffClass.DisplayValueKey != null)
								{
									string text = Localization.Get(buffClass.DisplayValueKey, false, null);
									BuffClass.CVarDisplayFormat displayValueFormat = buffClass.DisplayValueFormat;
									if (displayValueFormat != BuffClass.CVarDisplayFormat.Time)
									{
										if (displayValueFormat == BuffClass.CVarDisplayFormat.Degrees)
										{
											component.text = string.Format(text, ValueDisplayFormatters.Temperature(data.Notification.CurrentValue, -1));
										}
										else
										{
											component.text = string.Format(text, data.Notification.CurrentValue);
										}
									}
									else
									{
										component.text = string.Format(text, XUiC_BuffPopoutList.GetCVarValueAsTimeString(data.Notification.CurrentValue));
									}
								}
								else if (buffClass.DisplayValueFormat == BuffClass.CVarDisplayFormat.Time)
								{
									component.text = XUiC_BuffPopoutList.GetCVarValueAsTimeString(data.Notification.CurrentValue);
								}
								else
								{
									component.text = ((int)data.Notification.CurrentValue).ToString();
								}
							}
						}
						else if (component != null)
						{
							component.text = ValueDisplayFormatters.Temperature(data.Notification.CurrentValue, -1);
						}
					}
					else if (component != null)
					{
						component.text = ((int)(data.Notification.CurrentValue * 100f)).ToString() + "%";
					}
				}
				else
				{
					EnumEntityUINotificationDisplayMode displayMode = data.Notification.DisplayMode;
				}
				bool flag = false;
				if (data.Notification.Buff != null)
				{
					flag = EffectManager.GetValue(PassiveEffects.BuffBlink, null, 0f, this.LocalPlayer, null, data.Notification.Buff.BuffClass.NameTag, false, false, false, true, true, 1, true, false) >= 1f;
				}
				if (data.Notification.Buff != null && (data.Notification.Buff.BuffClass.IconBlink || flag))
				{
					Color color = data.Notification.GetColor();
					float num = Mathf.PingPong(Time.time, 0.5f);
					data.Sprite.color = Color.Lerp(Color.grey, color, num * 4f);
					float num2 = 1f;
					if (num > 0.25f)
					{
						num2 = 1f + num - 0.25f;
					}
					data.Sprite.SetDimensions((int)(this.spriteSize.x * num2), (int)(this.spriteSize.y * num2));
				}
				else
				{
					data.Sprite.color = data.Notification.GetColor();
					data.Sprite.SetDimensions((int)this.spriteSize.x, (int)this.spriteSize.y);
				}
			}
		}
		if (this.disabledItems.Count > 0)
		{
			for (int j = this.disabledItems.Count - 1; j >= 0; j--)
			{
				EntityUINotification entityUINotification = this.disabledItems[j];
				if (!entityUINotification.Buff.Paused)
				{
					this.AddNotification(entityUINotification);
					this.disabledItems.RemoveAt(j);
				}
			}
			this.updateEntries();
		}
	}

	// Token: 0x06007603 RID: 30211 RVA: 0x002C51E3 File Offset: 0x002C33E3
	public static string GetCVarValueAsTimeString(float cvarValue)
	{
		return XUiM_PlayerBuffs.GetCVarValueAsTimeString(cvarValue);
	}

	// Token: 0x06007604 RID: 30212 RVA: 0x002C51EC File Offset: 0x002C33EC
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.initialized)
		{
			this.PrefabItems.gameObject.SetActive(false);
			EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
			List<EntityUINotification> notifications = entityPlayer.PlayerStats.Notifications;
			for (int i = 0; i < notifications.Count; i++)
			{
				this.AddNotification(notifications[i]);
			}
			entityPlayer.PlayerStats.AddUINotificationChangedDelegate(this);
			this.initialized = true;
		}
	}

	// Token: 0x06007605 RID: 30213 RVA: 0x002C5268 File Offset: 0x002C3468
	public override void OnClose()
	{
		base.OnClose();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (entityPlayer != null)
		{
			entityPlayer.PlayerStats.RemoveUINotificationChangedDelegate(this);
		}
		this.initialized = false;
		for (int i = 0; i < this.items.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.items[i].Item.gameObject);
		}
		this.items.Clear();
	}

	// Token: 0x06007606 RID: 30214 RVA: 0x002C52E4 File Offset: 0x002C34E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeEntry(EntityUINotification notification, int currentIndex = -1)
	{
		int num = ((currentIndex == -1) ? this.GetNotificationIndex(notification) : currentIndex);
		if (num == -1)
		{
			return;
		}
		TemporaryObject temporaryObject = this.items[num].Item.transform.GetComponent<TemporaryObject>();
		if (temporaryObject == null)
		{
			temporaryObject = this.items[num].Item.transform.gameObject.AddComponent<TemporaryObject>();
		}
		temporaryObject.enabled = true;
		TweenColor tweenColor = this.items[num].Item.transform.GetComponent<TweenColor>();
		if (tweenColor == null)
		{
			tweenColor = this.items[num].Item.transform.gameObject.AddComponent<TweenColor>();
		}
		tweenColor.from = Color.white;
		tweenColor.to = new Color(1f, 1f, 1f, 0f);
		tweenColor.enabled = true;
		tweenColor.duration = 0.4f;
		TweenScale tweenScale = this.items[num].Item.gameObject.AddComponent<TweenScale>();
		tweenScale.from = Vector3.one;
		tweenScale.to = Vector3.zero;
		tweenScale.enabled = true;
		tweenScale.duration = 0.5f;
		this.items.RemoveAt(num);
		this.updateEntries();
	}

	// Token: 0x06007607 RID: 30215 RVA: 0x002C5428 File Offset: 0x002C3628
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetNotificationIndex(EntityUINotification notification)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].Notification.Subject == notification.Subject)
			{
				if (notification.Subject != EnumEntityUINotificationSubject.Buff)
				{
					return i;
				}
				if (this.items[i].Notification.Buff.BuffClass.ShowOnHUD && this.items[i].Notification.Buff.BuffClass.Name == notification.Buff.BuffClass.Name)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06007608 RID: 30216 RVA: 0x002C54D8 File Offset: 0x002C36D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateEntries()
	{
		int num = 0;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!this.items[i].Notification.Buff.Paused)
			{
				TweenPosition tweenPosition = this.items[i].Item.GetComponent<TweenPosition>();
				if (tweenPosition)
				{
					global::UnityEngine.Object.Destroy(tweenPosition);
				}
				tweenPosition = this.items[i].Item.AddComponent<TweenPosition>();
				tweenPosition.from = this.items[i].Item.transform.localPosition;
				tweenPosition.to = new Vector3(this.items[i].Item.transform.localPosition.x, (float)num * this.height + (float)this.yOffset, this.items[i].Item.transform.localPosition.z);
				tweenPosition.enabled = true;
				num++;
			}
		}
	}

	// Token: 0x06007609 RID: 30217 RVA: 0x002C55E7 File Offset: 0x002C37E7
	public void EntityUINotificationAdded(EntityUINotification _notification)
	{
		this.AddNotification(_notification);
	}

	// Token: 0x0600760A RID: 30218 RVA: 0x002C55F0 File Offset: 0x002C37F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddNotification(EntityUINotification _notification)
	{
		int notificationIndex = this.GetNotificationIndex(_notification);
		if (notificationIndex == -1)
		{
			if (_notification.Icon != "")
			{
				if (_notification.Buff != null)
				{
					if (!_notification.Buff.BuffClass.ShowOnHUD)
					{
						return;
					}
					if (_notification.Buff.Paused)
					{
						this.disabledItems.Add(_notification);
					}
				}
				GameObject gameObject = base.ViewComponent.UiTransform.gameObject.AddChild(this.PrefabItems.gameObject);
				gameObject.SetActive(true);
				gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
				gameObject.GetComponent<UIPanel>();
				UIEventListener uieventListener = UIEventListener.Get(gameObject.gameObject);
				uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnNotificationClicked));
				gameObject.transform.Find("Background").GetComponent<UISprite>().color = Color.white;
				UISprite component = gameObject.transform.Find("Icon").GetComponent<UISprite>();
				component.atlas = this.xui.GetAtlasByName(((global::UnityEngine.Object)component.atlas).name, _notification.Icon);
				component.spriteName = _notification.Icon;
				component.color = _notification.GetColor();
				if (this.spriteSize == Vector2.zero)
				{
					this.spriteSize = new Vector2((float)component.width, (float)component.height);
				}
				UISprite component2 = gameObject.transform.Find("BarContent").GetComponent<UISprite>();
				UILabel component3 = gameObject.transform.Find("TextContent").GetComponent<UILabel>();
				if (_notification.DisplayMode == EnumEntityUINotificationDisplayMode.IconPlusCurrentValue)
				{
					string units = _notification.Units;
					if (!(units == "%"))
					{
						if (!(units == "°"))
						{
							if (component3 != null)
							{
								component3.text = _notification.CurrentValue.ToCultureInvariantString("0");
							}
						}
						else if (component3 != null)
						{
							component3.text = _notification.CurrentValue.ToCultureInvariantString("0") + "°";
						}
					}
					else if (component3 != null)
					{
						component3.text = (_notification.CurrentValue * 100f).ToCultureInvariantString("0") + "%";
					}
				}
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, (float)this.items.Count * this.height + (float)this.yOffset, gameObject.transform.localPosition.z);
				XUiC_BuffPopoutList.Data data = new XUiC_BuffPopoutList.Data();
				data.Item = gameObject;
				data.TimeAdded = Time.time;
				data.Notification = _notification;
				data.Sprite = component;
				data.BarSprite = component2;
				if (_notification.Buff != null && _notification.Buff.BuffClass.TooltipKey != null)
				{
					GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, _notification.Buff.BuffClass.TooltipKey, false, false, 0f);
				}
				this.items.Add(data);
				return;
			}
		}
		else
		{
			this.items[notificationIndex].Notification = _notification;
			this.items[notificationIndex].TimeAdded = Time.time;
		}
	}

	// Token: 0x0600760B RID: 30219 RVA: 0x002C5939 File Offset: 0x002C3B39
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnNotificationClicked(GameObject go)
	{
		this.HandleClickForItem(go);
	}

	// Token: 0x0600760C RID: 30220 RVA: 0x002C5944 File Offset: 0x002C3B44
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleClickForItem(GameObject go)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].Item == go)
			{
				Manager.PlayInsidePlayerHead("craft_click_craft", -1, 0f, false, false);
				if (!this.xui.playerUI.windowManager.IsWindowOpen("character"))
				{
					XUiC_WindowSelector.OpenSelectorAndWindow(this.xui.playerUI.entityPlayer, "character");
				}
				this.SelectedNotification = this.items[i].Notification;
				return;
			}
		}
	}

	// Token: 0x0600760D RID: 30221 RVA: 0x002C59E3 File Offset: 0x002C3BE3
	public void EntityUINotificationRemoved(EntityUINotification _notification)
	{
		this.removeEntry(_notification, -1);
	}

	// Token: 0x0600760E RID: 30222 RVA: 0x002C59ED File Offset: 0x002C3BED
	public void SetYOffset(int _yOffset)
	{
		if (_yOffset != this.yOffset)
		{
			this.yOffset = _yOffset;
			this.updateEntries();
		}
	}

	// Token: 0x0600760F RID: 30223 RVA: 0x002C5A08 File Offset: 0x002C3C08
	public void SetBuffBarLengthAndColor(string _buffName, float _length, Color _color)
	{
		XUiC_BuffPopoutList.Data data = null;
		foreach (XUiC_BuffPopoutList.Data data2 in this.items)
		{
			if (string.Compare(data2.Notification.Buff.BuffName, _buffName, true) == 0)
			{
				data = data2;
				break;
			}
		}
		if (data != null)
		{
			data.BarSprite.fillAmount = _length;
			data.BarSprite.color = _color;
			float num = this.barSpriteSize.x * _length;
			data.BarSprite.SetDimensions((int)num, (int)this.barSpriteSize.y);
		}
	}

	// Token: 0x04005707 RID: 22279
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;

	// Token: 0x04005708 RID: 22280
	[PublicizedFrom(EAccessModifier.Private)]
	public float height;

	// Token: 0x04005709 RID: 22281
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffset;

	// Token: 0x0400570A RID: 22282
	public Transform PrefabItems;

	// Token: 0x0400570C RID: 22284
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 spriteSize;

	// Token: 0x0400570D RID: 22285
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 barSpriteSize;

	// Token: 0x0400570E RID: 22286
	public EntityUINotification SelectedNotification;

	// Token: 0x0400570F RID: 22287
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_BuffPopoutList.Data> items = new List<XUiC_BuffPopoutList.Data>();

	// Token: 0x04005710 RID: 22288
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityUINotification> disabledItems = new List<EntityUINotification>();

	// Token: 0x02000EB8 RID: 3768
	[PublicizedFrom(EAccessModifier.Private)]
	public class Data
	{
		// Token: 0x04005711 RID: 22289
		public GameObject Item;

		// Token: 0x04005712 RID: 22290
		public float TimeAdded;

		// Token: 0x04005713 RID: 22291
		public EntityUINotification Notification;

		// Token: 0x04005714 RID: 22292
		public UISprite Sprite;

		// Token: 0x04005715 RID: 22293
		public UISprite BarSprite;
	}
}
