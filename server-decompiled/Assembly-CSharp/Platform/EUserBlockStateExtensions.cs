using System;

namespace Platform
{
	// Token: 0x02001C1E RID: 7198
	public static class EUserBlockStateExtensions
	{
		// Token: 0x0600D60E RID: 54798 RVA: 0x004CFBE0 File Offset: 0x004CDDE0
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
