using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000EAA RID: 3754
[Preserve]
public class XUiC_ActiveBuffList : XUiController, IEntityUINotificationChanged
{
	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x0600754E RID: 30030 RVA: 0x002C0D32 File Offset: 0x002BEF32
	// (set) Token: 0x0600754F RID: 30031 RVA: 0x002C0D3A File Offset: 0x002BEF3A
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

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x06007550 RID: 30032 RVA: 0x002C0D69 File Offset: 0x002BEF69
	// (set) Token: 0x06007551 RID: 30033 RVA: 0x002C0D71 File Offset: 0x002BEF71
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

	// Token: 0x06007552 RID: 30034 RVA: 0x002C0DA4 File Offset: 0x002BEFA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		XUiC_BuffInfoWindow childByType = base.WindowGroup.Controller.GetChildByType<XUiC_BuffInfoWindow>();
		this.SelectedEntry = ((this.entryList[0].Notification != null) ? this.entryList[0] : null);
		childByType.SetBuff(this.SelectedEntry);
	}

	// Token: 0x06007553 RID: 30035 RVA: 0x002C0DF4 File Offset: 0x002BEFF4
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

	// Token: 0x06007554 RID: 30036 RVA: 0x002C0EA4 File Offset: 0x002BF0A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressBuff(XUiController _sender, int _mouseButton)
	{
		XUiC_ActiveBuffEntry xuiC_ActiveBuffEntry = _sender as XUiC_ActiveBuffEntry;
		if (xuiC_ActiveBuffEntry != null)
		{
			this.SelectedEntry = xuiC_ActiveBuffEntry;
		}
	}

	// Token: 0x06007555 RID: 30037 RVA: 0x002C0EC4 File Offset: 0x002BF0C4
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

	// Token: 0x06007556 RID: 30038 RVA: 0x002C1080 File Offset: 0x002BF280
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

	// Token: 0x06007557 RID: 30039 RVA: 0x002C10FC File Offset: 0x002BF2FC
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

	// Token: 0x06007558 RID: 30040 RVA: 0x002C1140 File Offset: 0x002BF340
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

	// Token: 0x06007559 RID: 30041 RVA: 0x002C1198 File Offset: 0x002BF398
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

	// Token: 0x0600755A RID: 30042 RVA: 0x002C1217 File Offset: 0x002BF417
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PlayerStats.RemoveUINotificationChangedDelegate(this);
	}

	// Token: 0x04005691 RID: 22161
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ActiveBuffEntry> entryList = new List<XUiC_ActiveBuffEntry>();

	// Token: 0x04005692 RID: 22162
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityUINotification> buffNotificationList = new List<EntityUINotification>();

	// Token: 0x04005693 RID: 22163
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x04005694 RID: 22164
	public bool setFirstEntry;

	// Token: 0x04005695 RID: 22165
	public XUiC_Paging pager;

	// Token: 0x04005696 RID: 22166
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageDownAction;

	// Token: 0x04005697 RID: 22167
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageUpAction;

	// Token: 0x04005698 RID: 22168
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005699 RID: 22169
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
