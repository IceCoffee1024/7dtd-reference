using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HarmonyLib
{
	// Token: 0x02000040 RID: 64
	internal static class PatchInfoSerialization
	{
		// Token: 0x0600011E RID: 286 RVA: 0x0000613C File Offset: 0x0000433C
		internal static byte[] Serialize(this PatchInfo patchInfo)
		{
			byte[] buffer;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PatchInfoSerialization.binaryFormatter.Serialize(memoryStream, patchInfo);
				buffer = memoryStream.GetBuffer();
			}
			return buffer;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006180 File Offset: 0x00004380
		internal static PatchInfo Deserialize(byte[] bytes)
		{
			PatchInfo patchInfo;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				patchInfo = (PatchInfo)PatchInfoSerialization.binaryFormatter.Deserialize(memoryStream);
			}
			return patchInfo;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000061C4 File Offset: 0x000043C4
		internal static int PriorityComparer(object obj, int index, int priority)
		{
			Traverse traverse = Traverse.Create(obj);
			int value = traverse.Field("priority").GetValue<int>();
			int value2 = traverse.Field("index").GetValue<int>();
			if (priority != value)
			{
				return -priority.CompareTo(value);
			}
			return index.CompareTo(value2);
		}

		// Token: 0x04000073 RID: 115
		internal static readonly BinaryFormatter binaryFormatter = new BinaryFormatter
		{
			Binder = new PatchInfoSerialization.Binder()
		};

		// Token: 0x02000096 RID: 150
		private class Binder : SerializationBinder
		{
			// Token: 0x060004EC RID: 1260 RVA: 0x000178F4 File Offset: 0x00015AF4
			public override Type BindToType(string assemblyName, string typeName)
			{
				Type[] array = new Type[]
				{
					typeof(PatchInfo),
					typeof(Patch[]),
					typeof(Patch)
				};
				foreach (Type type in array)
				{
					if (typeName == type.FullName)
					{
						return type;
					}
				}
				return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
			}
		}
	}
}
