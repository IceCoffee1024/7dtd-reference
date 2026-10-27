using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A4D RID: 6733
	[Preserve]
	public class ActionRenameSigns : ActionBaseContainersAction
	{
		// Token: 0x0600CCEA RID: 52458 RVA: 0x004AB6A4 File Offset: 0x004A98A4
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

		// Token: 0x0600CCEB RID: 52459 RVA: 0x004AB6DC File Offset: 0x004A98DC
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

		// Token: 0x0600CCEC RID: 52460 RVA: 0x004AB72C File Offset: 0x004A992C
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
