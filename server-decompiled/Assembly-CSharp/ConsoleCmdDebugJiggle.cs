using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200020B RID: 523
[Preserve]
public class ConsoleCmdDebugJiggle : ConsoleCmdAbstract
{
	// Token: 0x06000FD4 RID: 4052 RVA: 0x000666E4 File Offset: 0x000648E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugjiggle", "dgj" };
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x000666FC File Offset: 0x000648FC
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

	// Token: 0x06000FD9 RID: 4057 RVA: 0x000667C0 File Offset: 0x000649C0
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
