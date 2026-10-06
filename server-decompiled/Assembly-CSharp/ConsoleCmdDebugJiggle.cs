using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000209 RID: 521
[Preserve]
public class ConsoleCmdDebugJiggle : ConsoleCmdAbstract
{
	// Token: 0x06000FD0 RID: 4048 RVA: 0x00066514 File Offset: 0x00064714
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugjiggle", "dgj" };
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0006652C File Offset: 0x0006472C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		if (_params.Count <= 0)
		{
			foreach (EntityAlive entityAlive in world.EntityAlives)
			{
				this.LogEntityJiggles(entityAlive);
			}
			return;
		}
		int num;
		if (!int.TryParse(_params[0], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Expected an entity id");
			return;
		}
		Entity entity = world.GetEntity(num);
		if (entity == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Could not find entity with id {0}", num));
			return;
		}
		this.LogEntityJiggles(entity);
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x000665F0 File Offset: 0x000647F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogEntityJiggles(Entity entity)
	{
		EModelBase emodel = entity.emodel;
		if (emodel == null)
		{
			return;
		}
		emodel.LogJiggles();
	}
}
