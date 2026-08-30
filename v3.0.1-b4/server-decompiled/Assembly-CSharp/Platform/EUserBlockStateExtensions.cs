using System;

namespace Platform
{
	// Token: 0x02001BC5 RID: 7109
	public static class EUserBlockStateExtensions
	{
		// Token: 0x0600D308 RID: 54024 RVA: 0x004C4D04 File Offset: 0x004C2F04
		public static bool IsBlocked(this EUserBlockState blockState)
		{
			bool flag;
			if (blockState != EUserBlockState.NotBlocked)
			{
				if (blockState - EUserBlockState.InGame > 1)
				{
					throw new ArgumentOutOfRangeException("blockState", blockState, string.Format("{0} not implemented for {1}.{2}", "IsBlocked", "EUserBlockState", blockState));
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}
	}
}
