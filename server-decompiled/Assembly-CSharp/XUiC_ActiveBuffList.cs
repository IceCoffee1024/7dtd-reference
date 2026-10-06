using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000ECE RID: 3790
[Preserve]
public class XUiC_ActiveBuffList : XUiController, IEntityUINotificationChanged
{
	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x060075E7 RID: 30183 RVA: 0x002C760E File Offset: 0x002C580E
	// (set) Token: 0x060075E8 RID: 30184 RVA: 0x002C7616 File Offset: 0x002C5816
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				this.IsDirty = true;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x060075E9 RID: 30185 RVA: 0x002C7645 File Offset: 0x002C5845
	// (set) Token: 0x060075EA RID: 30186 RVA: 0x002C764D File Offset: 0x002C584D
	public XUiC_ActiveBuffEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = true;
			}
		}
	}

	// Token: 0x060075EB RID: 30187 RVA: 0x002C7680 File Offset: 0x002C5880
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		XUiC_BuffInfoWindow childByType = base.WindowGroup.Controller.GetChildByType<XUiC_BuffInfoWindow>();
		this.SelectedEntry = ((this.entryList[0].Notification != null) ? this.entryList[0] : null);
		childByType.SetBuff(this.SelectedEntry);
	}

	// Token: 0x060075EC RID: 30188 RVA: 0x002C76D0 File Offset: 0x002C58D0
	public override void Init()
	{
		base.Init();
		XUiC_BuffInfoWindow childByType = base.WindowGroup.Controller.GetChildByType<XUiC_BuffInfoWindow>();
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_ActiveBuffEntry)
			{
				XUiC_ActiveBuffEntry xuiC_ActiveBuffEntry = (XUiC_ActiveBuffEntry)this.children[i];
				xuiC_ActiveBuffEntry.InfoWindow = childByType;
				this.entryList.Add(xuiC_ActiveBuffEntry);
				this.length++;
			}
		}
		this.pager = base.Parent.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.Page = this.pager.CurrentPageNumber;
			};
		}
	}

	// Token: 0x060075ED RID: 30189 RVA: 0x002C7780 File Offset: 0x002C5980
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressBuff(XUiController _sender, int _mouseButton)
	{
		XUiC_ActiveBuffEntry xuiC_ActiveBuffEntry = _sender as XUiC_ActiveBuffEntry;
		if (xuiC_ActiveBuffEntry != null)
		{
			this.SelectedEntry = xuiC_ActiveBuffEntry;
		}
	}

	// Token: 0x060075EE RID: 30190 RVA: 0x002C77A0 File Offset: 0x002C59A0
	public override void Update(float _dt)
	{
		EntityUINotification selectedNotification = this.xui.BuffPopoutList.SelectedNotification;
		if (selectedNotification != null)
		{
			this.xui.BuffPopoutList.SelectedNotification = null;
			for (int i = 0; i < this.buffNotificationList.Count; i++)
			{
				if (this.buffNotificationList[i] == selectedNotification)
				{
					this.Page = i / this.length;
				}
			}
		}
		if (this.IsDirty)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging != null)
			{
				xuiC_Paging.SetLastPageByElementsAndPageLength(this.buffNotificationList.Count, this.entryList.Count);
			}
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 != null)
			{
				xuiC_Paging2.SetPage(this.page);
			}
			for (int j = 0; j < this.length; j++)
			{
				int num = j + this.length * this.page;
				XUiC_ActiveBuffEntry xuiC_ActiveBuffEntry = this.entryList[j];
				if (xuiC_ActiveBuffEntry != null)
				{
					xuiC_ActiveBuffEntry.OnPress -= this.OnPressBuff;
					if (num < this.buffNotificationList.Count)
					{
						xuiC_ActiveBuffEntry.Notification = this.buffNotificationList[num];
						xuiC_ActiveBuffEntry.OnPress += this.OnPressBuff;
						xuiC_ActiveBuffEntry.ViewComponent.SoundPlayOnClick = true;
					}
					else
					{
						xuiC_ActiveBuffEntry.Notification = null;
						xuiC_ActiveBuffEntry.ViewComponent.SoundPlayOnClick = false;
					}
				}
			}
			if (this.setFirstEntry)
			{
				this.SetFirstEntry();
				this.setFirstEntry = false;
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
		if (selectedNotification != null)
		{
			for (int k = 0; k < this.entryList.Count; k++)
			{
				if (this.entryList[k].Notification == selectedNotification)
				{
					this.SelectedEntry = this.entryList[k];
					return;
				}
			}
		}
	}

	// Token: 0x060075EF RID: 30191 RVA: 0x002C795C File Offset: 0x002C5B5C
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetNotificationIndex(EntityUINotification notification)
	{
		for (int i = 0; i < this.buffNotificationList.Count; i++)
		{
			if (this.buffNotificationList[i].Subject == notification.Subject)
			{
				if (notification.Subject != EnumEntityUINotificationSubject.Buff)
				{
					return i;
				}
				if (this.buffNotificationList[i].Buff.BuffClass.Name == notification.Buff.BuffClass.Name)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x060075F0 RID: 30192 RVA: 0x002C79D8 File Offset: 0x002C5BD8
	public void EntityUINotificationAdded(EntityUINotification _notification)
	{
		if (_notification.Buff == null)
		{
			return;
		}
		int notificationIndex = this.GetNotificationIndex(_notification);
		if (notificationIndex == -1)
		{
			this.buffNotificationList.Add(_notification);
		}
		else
		{
			this.buffNotificationList[notificationIndex] = _notification;
		}
		this.IsDirty = true;
	}

	// Token: 0x060075F1 RID: 30193 RVA: 0x002C7A1C File Offset: 0x002C5C1C
	public void EntityUINotificationRemoved(EntityUINotification _notification)
	{
		if (_notification.Buff == null)
		{
			return;
		}
		this.buffNotificationList.Remove(_notification);
		if (this.SelectedEntry != null && this.SelectedEntry.Notification == _notification)
		{
			this.SelectedEntry.InfoWindow.SetBuffInfo(null);
			this.SelectedEntry = null;
		}
		this.IsDirty = true;
	}

	// Token: 0x060075F2 RID: 30194 RVA: 0x002C7A74 File Offset: 0x002C5C74
	public override void OnOpen()
	{
		base.OnOpen();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		this.buffNotificationList.Clear();
		List<EntityUINotification> notifications = entityPlayer.PlayerStats.Notifications;
		for (int i = 0; i < notifications.Count; i++)
		{
			if (notifications[i].Buff != null)
			{
				this.buffNotificationList.Add(notifications[i]);
			}
		}
		entityPlayer.PlayerStats.AddUINotificationChangedDelegate(this);
		this.IsDirty = true;
	}

	// Token: 0x060075F3 RID: 30195 RVA: 0x002C7AF3 File Offset: 0x002C5CF3
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PlayerStats.RemoveUINotificationChangedDelegate(this);
	}

	// Token: 0x0400576D RID: 22381
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ActiveBuffEntry> entryList = new List<XUiC_ActiveBuffEntry>();

	// Token: 0x0400576E RID: 22382
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityUINotification> buffNotificationList = new List<EntityUINotification>();

	// Token: 0x0400576F RID: 22383
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x04005770 RID: 22384
	public bool setFirstEntry;

	// Token: 0x04005771 RID: 22385
	public XUiC_Paging pager;

	// Token: 0x04005772 RID: 22386
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageDownAction;

	// Token: 0x04005773 RID: 22387
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageUpAction;

	// Token: 0x04005774 RID: 22388
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005775 RID: 22389
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
