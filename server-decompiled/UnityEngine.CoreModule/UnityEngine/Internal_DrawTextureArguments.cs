using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200015A RID: 346
	[VisibleToOtherModules(new string[] { "UnityEngine.IMGUIModule" })]
	internal struct Internal_DrawTextureArguments
	{
		// Token: 0x04000464 RID: 1124
		public Rect screenRect;

		// Token: 0x04000465 RID: 1125
		public Rect sourceRect;

		// Token: 0x04000466 RID: 1126
		public int leftBorder;

		// Token: 0x04000467 RID: 1127
		public int rightBorder;

		// Token: 0x04000468 RID: 1128
		public int topBorder;

		// Token: 0x04000469 RID: 1129
		public int bottomBorder;

		// Token: 0x0400046A RID: 1130
		public Color leftBorderColor;

		// Token: 0x0400046B RID: 1131
		public Color rightBorderColor;

		// Token: 0x0400046C RID: 1132
		public Color topBorderColor;

		// Token: 0x0400046D RID: 1133
		public Color bottomBorderColor;

		// Token: 0x0400046E RID: 1134
		public Color color;

		// Token: 0x0400046F RID: 1135
		public Vector4 borderWidths;

		// Token: 0x04000470 RID: 1136
		public Vector4 cornerRadiuses;

		// Token: 0x04000471 RID: 1137
		public bool smoothCorners;

		// Token: 0x04000472 RID: 1138
		public int pass;

		// Token: 0x04000473 RID: 1139
		public Texture texture;

		// Token: 0x04000474 RID: 1140
		public Material mat;
	}
}
