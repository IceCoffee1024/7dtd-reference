using System;
using UnityEngine.Scripting;

// Token: 0x0200108F RID: 4239
[Preserve]
public class XUiC_SpawnBlockEditor : XUiController
{
	// Token: 0x06008658 RID: 34392 RVA: 0x00336190 File Offset: 0x00334390
	public override void Init()
	{
		base.Init();
		XUiC_SpawnBlockEditor.ID = base.WindowGroup.Id;
		this.list = base.GetChildByType<XUiC_StringList>();
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnOk") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnOk_OnPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnCancel") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += delegate(XUiController _, int _)
			{
				this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
			};
		}
	}

	// Token: 0x06008659 RID: 34393 RVA: 0x0033620C File Offset: 0x0033440C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_StringList.Entry selectedEntryData = this.list.SelectedEntryData;
		if (selectedEntryData == null)
		{
			return;
		}
		BlockValue block = GameManager.Instance.World.GetBlock(this.blockPos);
		block.meta = (byte)selectedEntryData.Tag;
		GameManager.Instance.World.SetBlockRPC(this.blockPos, block);
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x0600865A RID: 34394 RVA: 0x00336288 File Offset: 0x00334488
	[PublicizedFrom(EAccessModifier.Private)]
	public void setBlock(Vector3i _blockPos, BlockValue _blockValue, BlockSpawnEntity _block)
	{
		string[] spawnClasses = _block.spawnClasses;
		this.blockPos = _blockPos;
		this.list.ClearList();
		byte b = 0;
		while ((int)b < spawnClasses.Length)
		{
			this.list.AddEntry(spawnClasses[(int)b], b);
			b += 1;
		}
		this.list.SortList();
		this.list.RebuildList(false);
		byte meta = _blockValue.meta;
		if ((int)meta < spawnClasses.Length)
		{
			this.list.SelectByString(spawnClasses[(int)meta]);
		}
	}

	// Token: 0x0600865B RID: 34395 RVA: 0x00336305 File Offset: 0x00334505
	public static void Open(LocalPlayerUI _playerUi, Vector3i _blockPos, BlockValue _blockValue, BlockSpawnEntity _block)
	{
		XUiC_SpawnBlockEditor childByType = _playerUi.xui.FindWindowGroupByName(XUiC_SpawnBlockEditor.ID).GetChildByType<XUiC_SpawnBlockEditor>();
		_playerUi.windowManager.Open(XUiC_SpawnBlockEditor.ID, true);
		childByType.setBlock(_blockPos, _blockValue, _block);
	}

	// Token: 0x040063F2 RID: 25586
	public static string ID = "";

	// Token: 0x040063F3 RID: 25587
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_StringList list;

	// Token: 0x040063F4 RID: 25588
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;
}
