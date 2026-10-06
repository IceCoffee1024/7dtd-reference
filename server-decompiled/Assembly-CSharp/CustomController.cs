using System;
using UnityEngine;

// Token: 0x020013E0 RID: 5088
public class CustomController : MonoBehaviour
{
	// Token: 0x06009FBA RID: 40890 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
	}

	// Token: 0x06009FBB RID: 40891 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
	}

	// Token: 0x06009FBC RID: 40892 RVA: 0x003C4064 File Offset: 0x003C2264
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CollidesWithX(Vector3 position, float movement, out float newVelocity)
	{
		float num = this.Speed * (float)Math.Sign(movement);
		Vector3 vector = new Vector3(position.x + num + this.m_BoxWidth, position.y, position.z);
		if (!this.m_WorldData.GetBlock((int)vector.x, (int)vector.y, (int)vector.z).Equals(BlockValue.Air))
		{
			newVelocity = 0f;
			return true;
		}
		newVelocity = movement;
		return false;
	}

	// Token: 0x06009FBD RID: 40893 RVA: 0x003C40E0 File Offset: 0x003C22E0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CollidesWithY(Vector3 position, float movement, out float newVelocity)
	{
		float num = this.Speed * (float)Math.Sign(movement);
		Vector3 vector = new Vector3(position.x, position.y + num + this.m_BoxWidth, position.z);
		Log.Out(string.Concat(new string[]
		{
			"Checking ",
			vector.x.ToCultureInvariantString(),
			", ",
			vector.z.ToCultureInvariantString(),
			", ",
			vector.y.ToCultureInvariantString()
		}));
		BlockValue block = this.m_WorldData.GetBlock((int)vector.x, (int)vector.z, (int)vector.y);
		if (!block.Equals(BlockValue.Air))
		{
			string[] array = new string[8];
			array[0] = "Block ";
			int num2 = 1;
			BlockValue blockValue = block;
			array[num2] = blockValue.ToString();
			array[2] = " hit at ";
			array[3] = vector.x.ToCultureInvariantString();
			array[4] = ", ";
			array[5] = vector.z.ToCultureInvariantString();
			array[6] = ", ";
			array[7] = vector.y.ToCultureInvariantString();
			Log.Out(string.Concat(array));
			newVelocity = 0f;
			return true;
		}
		newVelocity = movement;
		return false;
	}

	// Token: 0x040078F9 RID: 30969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float m_BoxWidth = 0.5f;

	// Token: 0x040078FA RID: 30970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_Velocity;

	// Token: 0x040078FB RID: 30971
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_Forward;

	// Token: 0x040078FC RID: 30972
	public float Speed = 0.1f;

	// Token: 0x040078FD RID: 30973
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public World m_WorldData;
}
