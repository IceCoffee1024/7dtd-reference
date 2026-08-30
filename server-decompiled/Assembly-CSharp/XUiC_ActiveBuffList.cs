using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000EBF RID: 3775
[Preserve]
public class XUiC_ActiveBuffList : XUiController, IEntityUINotificationChanged
{
	// Token: 0x17000DE2 RID: 3554
	// (get) Token: 0x060075EF RID: 30191 RVA: 0x002C4256 File Offset: 0x002C2456
	// (set) Token: 0x060075F0 RID: 30192 RVA: 0x002C425E File Offset: 0x002C245E
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

	// Token: 0x17000DE3 RID: 3555
	// (get) Token: 0x060075F1 RID: 30193 RVA: 0x002C428D File Offset: 0x002C248D
	// (set) Token: 0x060075F2 RID: 30194 RVA: 0x002C4295 File Offset: 0x002C2495
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

	// Token: 0x060075F3 RID: 30195 RVA: 0x002C42C8 File Offset: 0x002C24C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		XUiC_BuffInfoWindow childByType = base.WindowGroup.Controller.GetChildByType<XUiC_BuffInfoWindow>();
		this.SelectedEntry = ((this.entryList[0].Notification != null) ? this.entryList[0] : null);
		childByType.SetBuff(this.SelectedEntry);
	}

	// Token: 0x060075F4 RID: 30196 RVA: 0x002C4318 File Offset: 0x002C2518
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

	// Token: 0x060075F5 RID: 30197 RVA: 0x002C43C8 File Offset: 0x002C25C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressBuff(XUiController _sender, int _mouseButton)
	{
		XUiC_ActiveBuffEntry xuiC_ActiveBuffEntry = _sender as XUiC_ActiveBuffEntry;
		if (xuiC_ActiveBuffEntry != null)
		{
			this.SelectedEntry = xuiC_ActiveBuffEntry;
		}
	}

	// Token: 0x060075F6 RID: 30198 RVA: 0x002C43E8 File Offset: 0x002C25E8
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

	// Token: 0x060075F7 RID: 30199 RVA: 0x002C45A4 File Offset: 0x002C27A4
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

	// Token: 0x060075F8 RID: 30200 RVA: 0x002C4620 File Offset: 0x002C2820
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

	// Token: 0x060075F9 RID: 30201 RVA: 0x002C4664 File Offset: 0x002C2864
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

	// Token: 0x060075FA RID: 30202 RVA: 0x002C46BC File Offset: 0x002C28BC
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

	// Token: 0x060075FB RID: 30203 RVA: 0x002C473B File Offset: 0x002C293B
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PlayerStats.RemoveUINotificationChangedDelegate(this);
	}

	// Token: 0x04005736 RID: 22326
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_ActiveBuffEntry> entryList = new List<XUiC_ActiveBuffEntry>();

	// Token: 0x04005737 RID: 22327
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityUINotification> buffNotificationList = new List<EntityUINotification>();

	// Token: 0x04005738 RID: 22328
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x04005739 RID: 22329
	public bool setFirstEntry;

	// Token: 0x0400573A RID: 22330
	public XUiC_Paging pager;

	// Token: 0x0400573B RID: 22331
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageDownAction;

	// Token: 0x0400573C RID: 22332
	[PublicizedFrom(EAccessModifier.Private)]
	public Action handlePageUpAction;

	// Token: 0x0400573D RID: 22333
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x0400573E RID: 22334
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
