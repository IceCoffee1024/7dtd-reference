using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F42 RID: 3906
[Preserve]
public class XUiC_DialogResponseList : XUiController
{
	// Token: 0x17000E5E RID: 3678
	// (get) Token: 0x060079E4 RID: 31204 RVA: 0x002E249A File Offset: 0x002E069A
	// (set) Token: 0x060079E5 RID: 31205 RVA: 0x002E24A2 File Offset: 0x002E06A2
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

	// Token: 0x060079E6 RID: 31206 RVA: 0x002E24B8 File Offset: 0x002E06B8
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

	// Token: 0x060079E7 RID: 31207 RVA: 0x002E2549 File Offset: 0x002E0749
	public override void OnOpen()
	{
		base.OnOpen();
		this.lblResponderName.Text = Localization.Get(this.xui.Dialog.Respondent.EntityName, false, null);
	}

	// Token: 0x060079E8 RID: 31208 RVA: 0x002E2578 File Offset: 0x002E0778
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

	// Token: 0x060079E9 RID: 31209 RVA: 0x002E26F4 File Offset: 0x002E08F4
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

	// Token: 0x060079EA RID: 31210 RVA: 0x002E274B File Offset: 0x002E094B
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005A5E RID: 23134
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog conversation;

	// Token: 0x04005A5F RID: 23135
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005A60 RID: 23136
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_DialogResponseEntry> entryList = new List<XUiC_DialogResponseEntry>();

	// Token: 0x04005A61 RID: 23137
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005A62 RID: 23138
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> uniqueResponseIDs = new List<string>();

	// Token: 0x04005A63 RID: 23139
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;

	// Token: 0x04005A64 RID: 23140
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblResponderName;
}
