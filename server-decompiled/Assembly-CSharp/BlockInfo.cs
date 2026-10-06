using System;
using UnityEngine.Scripting;

// Token: 0x02000124 RID: 292
[Preserve]
public class BlockInfo : Block
{
	// Token: 0x060007B8 RID: 1976 RVA: 0x00036C18 File Offset: 0x00034E18
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockInfo.PropInfoType))
		{
			this.InformationType = Enum.Parse<BlockInfo.InformationTypes>(base.Properties.Values[BlockInfo.PropInfoType]);
		}
		base.Properties.ParseFloat(BlockInfo.PropTakeDelay, ref this.TakeDelay);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00036C78 File Offset: 0x00034E78
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return this.TakeDelay > 0f;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00036C88 File Offset: 0x00034E88
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00036CD4 File Offset: 0x00034ED4
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		string text;
		switch (this.InformationType)
		{
		case BlockInfo.InformationTypes.Time:
			text = string.Format(Localization.Get("showClockTime", false, null), GameUtils.WorldTimeToHourMinutesString(GameManager.Instance.World.worldTime));
			break;
		case BlockInfo.InformationTypes.Day:
			text = string.Format(Localization.Get("showCalendarDate", false, null), GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime));
			break;
		case BlockInfo.InformationTypes.TimeAndDay:
			text = string.Format(Localization.Get("showTimeAndDay", false, null), GameUtils.WorldTimeToHourMinutesString(GameManager.Instance.World.worldTime), GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime));
			break;
		default:
			text = "";
			break;
		}
		return text;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00036D9C File Offset: 0x00034F9C
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_commandName == "take")
		{
			base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
			return true;
		}
		return false;
	}

	// Token: 0x0400090F RID: 2319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTakeDelay = "TakeDelay";

	// Token: 0x04000910 RID: 2320
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropInfoType = "InfoType";

	// Token: 0x04000911 RID: 2321
	public BlockInfo.InformationTypes InformationType;

	// Token: 0x04000912 RID: 2322
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;

	// Token: 0x04000913 RID: 2323
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("take", "hand", false, false, null)
	};

	// Token: 0x02000125 RID: 293
	public enum InformationTypes
	{
		// Token: 0x04000915 RID: 2325
		Time,
		// Token: 0x04000916 RID: 2326
		Day,
		// Token: 0x04000917 RID: 2327
		TimeAndDay
	}
}
