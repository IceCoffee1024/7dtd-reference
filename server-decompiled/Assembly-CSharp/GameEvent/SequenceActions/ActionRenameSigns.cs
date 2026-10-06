using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1C RID: 6684
	[Preserve]
	public class ActionRenameSigns : ActionBaseContainersAction
	{
		// Token: 0x0600CBD6 RID: 52182 RVA: 0x004A7BA8 File Offset: 0x004A5DA8
		public override bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = true;
			ITileEntitySignable tileEntitySignable;
			if (te.TryGetSelfOrFeature<ITileEntitySignable>(out tileEntitySignable))
			{
				isEmpty = tileEntitySignable.GetAuthoredText().Text == base.ModifiedName;
				return true;
			}
			return false;
		}

		// Token: 0x0600CBD7 RID: 52183 RVA: 0x004A7BE0 File Offset: 0x004A5DE0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			bool flag = false;
			for (int i = 0; i < tileEntityList.Count; i++)
			{
				ITileEntitySignable tileEntitySignable;
				if (tileEntityList[i].TryGetSelfOrFeature<ITileEntitySignable>(out tileEntitySignable))
				{
					tileEntitySignable.SetText(base.ModifiedName, true, PlatformManager.MultiPlatform.User.PlatformUserId);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600CBD8 RID: 52184 RVA: 0x004A7C30 File Offset: 0x004A5E30
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRenameSigns
			{
				TargetingType = this.TargetingType,
				maxDistance = this.maxDistance,
				newName = this.newName,
				changeName = this.changeName,
				tileEntityList = this.tileEntityList
			};
		}
	}
}
