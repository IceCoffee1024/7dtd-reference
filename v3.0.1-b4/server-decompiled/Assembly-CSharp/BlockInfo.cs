using System;
using UnityEngine.Scripting;

// Token: 0x02000121 RID: 289
[Preserve]
public class BlockInfo : Block
{
	// Token: 0x060007A8 RID: 1960 RVA: 0x00036834 File Offset: 0x00034A34
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockInfo.PropInfoType))
		{
			this.InformationType = Enum.Parse<BlockInfo.InformationTypes>(base.Properties.Values[BlockInfo.PropInfoType]);
		}
		base.Properties.ParseFloat(BlockInfo.PropTakeDelay, ref this.TakeDelay);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00036894 File Offset: 0x00034A94
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return this.TakeDelay > 0f;
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x000368A4 File Offset: 0x00034AA4
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[0].enabled = flag && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x000368F0 File Offset: 0x00034AF0
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

	// Token: 0x060007AC RID: 1964 RVA: 0x000369B8 File Offset: 0x00034BB8
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_commandName == "take")
		{
			base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
			return true;
		}
		return false;
	}

	// Token: 0x0400090A RID: 2314
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTakeDelay = "TakeDelay";

	// Token: 0x0400090B RID: 2315
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropInfoType = "InfoType";

	// Token: 0x0400090C RID: 2316
	public BlockInfo.InformationTypes InformationType;

	// Token: 0x0400090D RID: 2317
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;

	// Token: 0x0400090E RID: 2318
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("take", "hand", false, false, null)
	};

	// Token: 0x02000122 RID: 290
	public enum InformationTypes
	{
		// Token: 0x04000910 RID: 2320
		Time,
		// Token: 0x04000911 RID: 2321
		Day,
		// Token: 0x04000912 RID: 2322
		TimeAndDay
	}
}
