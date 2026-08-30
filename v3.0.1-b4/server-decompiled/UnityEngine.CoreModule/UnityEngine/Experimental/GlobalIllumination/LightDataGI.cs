using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x020004C7 RID: 1223
	[UsedByNativeCode]
	public struct LightDataGI
	{
		// Token: 0x06002AC4 RID: 10948 RVA: 0x00047D80 File Offset: 0x00045F80
		public void Init(ref DirectionalLight light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = 0f;
			this.coneAngle = cookie.sizes.x;
			this.innerConeAngle = cookie.sizes.y;
			this.shape0 = light.penumbraWidthRadian;
			this.shape1 = 0f;
			this.type = LightType.Directional;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = FalloffType.Undefined;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x00047E54 File Offset: 0x00046054
		public void Init(ref PointLight light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = light.range;
			this.coneAngle = 0f;
			this.innerConeAngle = 0f;
			this.shape0 = light.sphereRadius;
			this.shape1 = 0f;
			this.type = LightType.Point;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = light.falloff;
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x00047F24 File Offset: 0x00046124
		public void Init(ref SpotLight light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = light.range;
			this.coneAngle = light.coneAngle;
			this.innerConeAngle = light.innerConeAngle;
			this.shape0 = light.sphereRadius;
			this.shape1 = (float)light.angularFalloff;
			this.type = LightType.Spot;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = light.falloff;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x00047FF8 File Offset: 0x000461F8
		public void Init(ref RectangleLight light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = light.range;
			this.coneAngle = 0f;
			this.innerConeAngle = 0f;
			this.shape0 = light.width;
			this.shape1 = light.height;
			this.type = LightType.Rectangle;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = light.falloff;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000480C8 File Offset: 0x000462C8
		public void Init(ref DiscLight light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = light.range;
			this.coneAngle = 0f;
			this.innerConeAngle = 0f;
			this.shape0 = light.radius;
			this.shape1 = 0f;
			this.type = LightType.Disc;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = light.falloff;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00048198 File Offset: 0x00046398
		public void Init(ref SpotLightBoxShape light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = light.range;
			this.coneAngle = 0f;
			this.innerConeAngle = 0f;
			this.shape0 = light.width;
			this.shape1 = light.height;
			this.type = LightType.SpotBoxShape;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = FalloffType.Undefined;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00048264 File Offset: 0x00046464
		public void Init(ref SpotLightPyramidShape light, ref Cookie cookie)
		{
			this.instanceID = light.instanceID;
			this.cookieID = cookie.instanceID;
			this.cookieScale = cookie.scale;
			this.color = light.color;
			this.indirectColor = light.indirectColor;
			this.orientation = light.orientation;
			this.position = light.position;
			this.range = light.range;
			this.coneAngle = light.angle;
			this.innerConeAngle = 0f;
			this.shape0 = light.aspectRatio;
			this.shape1 = 0f;
			this.type = LightType.SpotPyramidShape;
			this.mode = light.mode;
			this.shadow = (light.shadow ? 1 : 0);
			this.falloff = light.falloff;
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x00048334 File Offset: 0x00046534
		public void Init(ref DirectionalLight light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x00048354 File Offset: 0x00046554
		public void Init(ref PointLight light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x00048374 File Offset: 0x00046574
		public void Init(ref SpotLight light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x00048394 File Offset: 0x00046594
		public void Init(ref RectangleLight light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000483B4 File Offset: 0x000465B4
		public void Init(ref DiscLight light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000483D4 File Offset: 0x000465D4
		public void Init(ref SpotLightBoxShape light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000483F4 File Offset: 0x000465F4
		public void Init(ref SpotLightPyramidShape light)
		{
			Cookie cookie = Cookie.Defaults();
			this.Init(ref light, ref cookie);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x00048412 File Offset: 0x00046612
		public void InitNoBake(int lightInstanceID)
		{
			this.instanceID = lightInstanceID;
			this.mode = LightMode.Unknown;
		}

		// Token: 0x04001015 RID: 4117
		public int instanceID;

		// Token: 0x04001016 RID: 4118
		public int cookieID;

		// Token: 0x04001017 RID: 4119
		public float cookieScale;

		// Token: 0x04001018 RID: 4120
		public LinearColor color;

		// Token: 0x04001019 RID: 4121
		public LinearColor indirectColor;

		// Token: 0x0400101A RID: 4122
		public Quaternion orientation;

		// Token: 0x0400101B RID: 4123
		public Vector3 position;

		// Token: 0x0400101C RID: 4124
		public float range;

		// Token: 0x0400101D RID: 4125
		public float coneAngle;

		// Token: 0x0400101E RID: 4126
		public float innerConeAngle;

		// Token: 0x0400101F RID: 4127
		public float shape0;

		// Token: 0x04001020 RID: 4128
		public float shape1;

		// Token: 0x04001021 RID: 4129
		public LightType type;

		// Token: 0x04001022 RID: 4130
		public LightMode mode;

		// Token: 0x04001023 RID: 4131
		public byte shadow;

		// Token: 0x04001024 RID: 4132
		public FalloffType falloff;
	}
}
