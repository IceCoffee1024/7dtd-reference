using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F0D RID: 3853
[Preserve]
public class XUiC_DialogResponseList : XUiController
{
	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x060078EE RID: 30958 RVA: 0x002D9882 File Offset: 0x002D7A82
	// (set) Token: 0x060078EF RID: 30959 RVA: 0x002D988A File Offset: 0x002D7A8A
	public Dialog CurrentDialog
	{
		get
		{
			return this.currentDialog;
		}
		set
		{
			this.currentDialog = value;
			base.RefreshBindings();
			this.IsDirty = true;
		}
	}

	// Token: 0x060078F0 RID: 30960 RVA: 0x002D98A0 File Offset: 0x002D7AA0
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
		this.lblResponderName = base.GetChildById("lblName").ViewComponent as XUiV_Label;
		XUiController childById = base.GetChildById("items");
		for (int i = 0; i < childById.Children.Count; i++)
		{
			XUiC_DialogResponseEntry xuiC_DialogResponseEntry = childById.Children[i] as XUiC_DialogResponseEntry;
			if (xuiC_DialogResponseEntry != null)
			{
				this.entryList.Add(xuiC_DialogResponseEntry);
				this.length++;
			}
		}
	}

	// Token: 0x060078F1 RID: 30961 RVA: 0x002D9931 File Offset: 0x002D7B31
	public override void OnOpen()
	{
		base.OnOpen();
		this.lblResponderName.Text = Localization.Get(this.xui.Dialog.Respondent.EntityName, false, null);
	}

	// Token: 0x060078F2 RID: 30962 RVA: 0x002D9960 File Offset: 0x002D7B60
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			List<BaseResponseEntry> list = new List<BaseResponseEntry>();
			this.uniqueResponseIDs.Clear();
			if (this.currentDialog != null)
			{
				list = this.currentDialog.CurrentStatement.GetResponses();
			}
			int num = 0;
			for (int i = 0; i < this.entryList.Count; i++)
			{
				XUiC_DialogResponseEntry xuiC_DialogResponseEntry = this.entryList[i];
				if (xuiC_DialogResponseEntry != null)
				{
					xuiC_DialogResponseEntry.OnPress -= this.OnPressResponse;
					if (num < list.Count)
					{
						xuiC_DialogResponseEntry.ViewComponent.SoundPlayOnClick = true;
						if (list[num].UniqueID == "" || !this.uniqueResponseIDs.Contains(list[num].UniqueID))
						{
							xuiC_DialogResponseEntry.CurrentResponse = list[num].Response;
							xuiC_DialogResponseEntry.OnPress += this.OnPressResponse;
						}
						else
						{
							xuiC_DialogResponseEntry.CurrentResponse = null;
						}
						if (xuiC_DialogResponseEntry.CurrentResponse == null)
						{
							i--;
						}
						else if (list[num].UniqueID != "")
						{
							this.uniqueResponseIDs.Add(list[num].UniqueID);
						}
						num++;
					}
					else
					{
						xuiC_DialogResponseEntry.ViewComponent.SoundPlayOnClick = false;
						xuiC_DialogResponseEntry.CurrentResponse = null;
					}
				}
			}
			if (list.Count > 0)
			{
				this.entryList[0].SelectCursorElement(true, false);
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x060078F3 RID: 30963 RVA: 0x002D9ADC File Offset: 0x002D7CDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressResponse(XUiController _sender, int _mouseButton)
	{
		if (((XUiC_DialogResponseEntry)_sender).HasRequirement)
		{
			DialogResponse currentResponse = ((XUiC_DialogResponseEntry)_sender).CurrentResponse;
			this.currentDialog.SelectResponse(currentResponse, this.xui.playerUI.entityPlayer);
			((XUiC_DialogWindowGroup)this.windowGroup.Controller).RefreshDialog();
		}
	}

	// Token: 0x060078F4 RID: 30964 RVA: 0x002D9B33 File Offset: 0x002D7D33
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005941 RID: 22849
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog conversation;

	// Token: 0x04005942 RID: 22850
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005943 RID: 22851
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_DialogResponseEntry> entryList = new List<XUiC_DialogResponseEntry>();

	// Token: 0x04005944 RID: 22852
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005945 RID: 22853
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> uniqueResponseIDs = new List<string>();

	// Token: 0x04005946 RID: 22854
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;

	// Token: 0x04005947 RID: 22855
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblResponderName;
}
