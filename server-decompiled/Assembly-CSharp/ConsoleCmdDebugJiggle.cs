using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000206 RID: 518
[Preserve]
public class ConsoleCmdDebugJiggle : ConsoleCmdAbstract
{
	// Token: 0x06000FD2 RID: 4050 RVA: 0x00065FDC File Offset: 0x000641DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugjiggle", "dgj" };
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x00065FF4 File Offset: 0x000641F4
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

	// Token: 0x06000FD7 RID: 4055 RVA: 0x000660B8 File Offset: 0x000642B8
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
