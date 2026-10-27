using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000ED9 RID: 3801
[Preserve]
public class XUiC_ActiveBuffList : XUiController, IEntityUINotificationChanged
{
	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x0600760C RID: 30220 RVA: 0x002C8B7A File Offset: 0x002C6D7A
	// (set) Token: 0x0600760D RID: 30221 RVA: 0x002C8B82 File Offset: 0x002C6D82
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

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x0600760E RID: 30222 RVA: 0x002C8BB1 File Offset: 0x002C6DB1
	// (set) Token: 0x0600760F RID: 30223 RVA: 0x002C8BB9 File Offset: 0x002C6DB9
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

	// Token: 0x06007610 RID: 30224 RVA: 0x002C8BEC File Offset: 0x002C6DEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		XUiC_BuffInfoWindow childByType = base.WindowGroup.Controller.GetChildByType<XUiC_BuffInfoWindow>();
		this.SelectedEntry = ((this.entryList[0].Notification != null) ? this.entryList[0] : null);
		childByType.SetBuff(this.SelectedEntry);
	}

	// Token: 0x06007611 RID: 30225 RVA: 0x002C8C3C File Offset: 0x002C6E3C
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

	// Token: 0x06007612 RID: 30226 RVA: 0x002C8CEC File Offset: 0x002C6EEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressBuff(XUiController _sender, int _mouseButton)
	{
		XUiC_ActiveBuffEntry xuiC_ActiveBuffEntry = _sender as XUiC_ActiveBuffEntry;
		if (xuiC_ActiveBuffEntry != null)
		{
			this.SelectedEntry = xuiC_ActiveBuffEntry;
		}
	}

	// Token: 0x06007613 RID: 30227 RVA: 0x002C8D0C File Offset: 0x002C6F0C
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

	// Token: 0x06007614 RID: 30228 RVA: 0x002C8EC8 File Offset: 0x002C70C8
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

	// Token: 0x06007615 RID: 30229 RVA: 0x002C8F44 File Offset: 0x002C7144
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

	// Token: 0x06007616 RID: 30230 RVA: 0x002C8F88 File Offset: 0x002C7188
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

	// Token: 0x06007617 RID: 30231 RVA: 0x002C8FE0 File Offset: 0x002C71E0
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

	// Token: 0x06007618 RID: 30232 RVA: 0x002C905F File Offset: 0x002C725F
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PlayerStats.RemoveUINotificationChangedDelegate(this);
	}

	// Token: 0x04005792 RID: 22418
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ActiveBuffEntry> entryList = new List<XUiC_ActiveBuffEntry>();

	// Token: 0x04005793 RID: 22419
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityUINotification> buffNotificationList = new List<EntityUINotification>();

	// Token: 0x04005794 RID: 22420
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x04005795 RID: 22421
	public bool setFirstEntry;

	// Token: 0x04005796 RID: 22422
	public XUiC_Paging pager;

	// Token: 0x04005797 RID: 22423
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageDownAction;

	// Token: 0x04005798 RID: 22424
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageUpAction;

	// Token: 0x04005799 RID: 22425
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400579A RID: 22426
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
