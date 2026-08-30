using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C9 RID: 6601
	[Preserve]
	public class ActionRenameSigns : ActionBaseContainersAction
	{
		// Token: 0x0600C8F5 RID: 51445 RVA: 0x0049D308 File Offset: 0x0049B508
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

		// Token: 0x0600C8F6 RID: 51446 RVA: 0x0049D340 File Offset: 0x0049B540
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

		// Token: 0x0600C8F7 RID: 51447 RVA: 0x0049D390 File Offset: 0x0049B590
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
