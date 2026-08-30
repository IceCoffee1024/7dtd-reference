using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F0 RID: 6640
	[Preserve]
	public class ActionRenameSigns : ActionBaseContainersAction
	{
		// Token: 0x0600CA7E RID: 51838 RVA: 0x004A36FC File Offset: 0x004A18FC
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

		// Token: 0x0600CA7F RID: 51839 RVA: 0x004A3734 File Offset: 0x004A1934
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

		// Token: 0x0600CA80 RID: 51840 RVA: 0x004A3784 File Offset: 0x004A1984
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
