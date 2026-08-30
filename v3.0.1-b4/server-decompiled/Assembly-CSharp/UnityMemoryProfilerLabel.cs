using System;
using System.Text;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class UnityMemoryProfilerLabel : MonoBehaviour
{
	// Token: 0x0600010D RID: 269 RVA: 0x0000C013 File Offset: 0x0000A213
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		if (this.label == null)
		{
			base.enabled = !base.gameObject.TryGetComponent<UILabel>(out this.label);
			if (!base.enabled)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x0000C050 File Offset: 0x0000A250
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.totalRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory", 1, ProfilerRecorderOptions.Default);
		this.totalReservedRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory", 1, ProfilerRecorderOptions.Default);
		this.systemRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory", 1, ProfilerRecorderOptions.Default);
		this.gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory", 1, ProfilerRecorderOptions.Default);
		this.gcUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory", 1, ProfilerRecorderOptions.Default);
		this.gcAllocInFrameMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame", 1, ProfilerRecorderOptions.Default);
		this.gfxUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory", 1, ProfilerRecorderOptions.Default);
		this.mainThreadRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15, ProfilerRecorderOptions.Default);
		this.meshBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Mesh Memory", 1, ProfilerRecorderOptions.Default);
		this.meshCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Mesh Count", 1, ProfilerRecorderOptions.Default);
		this.textureBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Used Textures Bytes", 1, ProfilerRecorderOptions.Default);
		this.textureCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Used Textures Count", 1, ProfilerRecorderOptions.Default);
		this.renderTextureBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Render Textures Bytes", 1, ProfilerRecorderOptions.Default);
		this.renderTextureCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Render Textures Count", 1, ProfilerRecorderOptions.Default);
		this.setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count", 1, ProfilerRecorderOptions.Default);
		this.drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count", 1, ProfilerRecorderOptions.Default);
		this.verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count", 1, ProfilerRecorderOptions.Default);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.sb.Clear();
		this.sb.AppendLine("MEMORY");
		if (this.systemRecorder.Valid)
		{
			this.sb.AppendLine("System Used Memory " + UnityMemoryProfilerLabel.ToSize(this.systemRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
		}
		if (this.totalReservedRecorder.Valid)
		{
			this.sb.AppendLine("Total Reserved Memory " + UnityMemoryProfilerLabel.ToSize(this.totalReservedRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
		}
		if (this.totalRecorder.Valid)
		{
			this.sb.AppendLine("Total Used Memory " + UnityMemoryProfilerLabel.ToSize(this.totalRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
		}
		if (this.gcReservedMemoryRecorder.Valid)
		{
			this.sb.AppendLine("GC Reserved Memory " + UnityMemoryProfilerLabel.ToSize(this.gcReservedMemoryRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
		}
		if (this.gcUsedMemoryRecorder.Valid)
		{
			this.sb.AppendLine("GC Used Memory " + UnityMemoryProfilerLabel.ToSize(this.gcUsedMemoryRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
		}
		if (this.gcAllocInFrameMemoryRecorder.Valid)
		{
			this.sb.AppendLine("GC Allocated This Frame " + UnityMemoryProfilerLabel.ToSize(this.gcAllocInFrameMemoryRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.MiB) + "MiB");
		}
		if (this.mainThreadRecorder.Valid)
		{
			this.sb.AppendLine("Main Thread Memory " + UnityMemoryProfilerLabel.ToSize(this.mainThreadRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.MiB) + "MiB");
		}
		if (this.gfxUsedMemoryRecorder.Valid)
		{
			this.sb.AppendLine("GFX Used Memory " + UnityMemoryProfilerLabel.ToSize(this.gfxUsedMemoryRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
		}
		this.sb.AppendLine();
		this.sb.AppendLine("Rendering");
		if (this.meshCountRecorder.Valid)
		{
			this.sb.AppendLine(string.Format("Mesh Count {0}", this.meshCountRecorder.LastValue));
		}
		if (this.meshBytesRecorder.Valid)
		{
			if (this.meshBytesRecorder.LastValue > 1073741824L)
			{
				this.sb.AppendLine("Mesh Memory " + UnityMemoryProfilerLabel.ToSize(this.meshBytesRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
			}
			else
			{
				this.sb.AppendLine("Mesh Memory " + UnityMemoryProfilerLabel.ToSize(this.meshBytesRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.MiB) + "MiB");
			}
		}
		if (this.textureCountRecorder.Valid && this.textureCountRecorder.LastValue > 0L)
		{
			this.sb.AppendLine(string.Format("Used Textures Count {0}", this.textureCountRecorder.LastValue));
		}
		if (this.textureBytesRecorder.Valid && this.textureBytesRecorder.LastValue > 0L)
		{
			if (this.meshBytesRecorder.LastValue > 1073741824L)
			{
				this.sb.AppendLine("Used Textures " + UnityMemoryProfilerLabel.ToSize(this.textureBytesRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.GiB) + "GiB");
			}
			else
			{
				this.sb.AppendLine("Used Textures " + UnityMemoryProfilerLabel.ToSize(this.textureBytesRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.MiB) + "MiB");
			}
		}
		if (this.renderTextureCountRecorder.Valid)
		{
			this.sb.AppendLine(string.Format("Render Textures {0}", this.renderTextureCountRecorder.LastValue));
		}
		if (this.renderTextureBytesRecorder.Valid)
		{
			this.sb.AppendLine("Render Textures " + UnityMemoryProfilerLabel.ToSize(this.renderTextureBytesRecorder.LastValue, UnityMemoryProfilerLabel.SiSizeUnits.MiB) + "MiB");
		}
		if (this.setPassCallsRecorder.Valid)
		{
			this.sb.AppendLine(string.Format("SetPass Calls: {0}", this.setPassCallsRecorder.LastValue));
		}
		if (this.drawCallsRecorder.Valid)
		{
			this.sb.AppendLine(string.Format("Draw Calls: {0}", this.drawCallsRecorder.LastValue));
		}
		if (this.verticesRecorder.Valid)
		{
			this.sb.AppendLine(string.Format("Vertices: {0}", this.verticesRecorder.LastValue));
		}
		this.label.text = this.sb.ToString();
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000C69C File Offset: 0x0000A89C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.totalRecorder.Dispose();
		this.totalReservedRecorder.Dispose();
		this.systemRecorder.Dispose();
		this.gcReservedMemoryRecorder.Dispose();
		this.gcUsedMemoryRecorder.Dispose();
		this.gcAllocInFrameMemoryRecorder.Dispose();
		this.gfxUsedMemoryRecorder.Dispose();
		this.mainThreadRecorder.Dispose();
		this.textureBytesRecorder.Dispose();
		this.textureCountRecorder.Dispose();
		this.renderTextureBytesRecorder.Dispose();
		this.renderTextureCountRecorder.Dispose();
		this.setPassCallsRecorder.Dispose();
		this.drawCallsRecorder.Dispose();
		this.verticesRecorder.Dispose();
	}

	// Token: 0x06000111 RID: 273 RVA: 0x0000C750 File Offset: 0x0000A950
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ToSize(long _value, UnityMemoryProfilerLabel.SiSizeUnits _unit)
	{
		return ((double)_value / Math.Pow(1024.0, (double)((long)_unit))).ToString("0.0000");
	}

	// Token: 0x04000129 RID: 297
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public UILabel label;

	// Token: 0x0400012A RID: 298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder totalRecorder;

	// Token: 0x0400012B RID: 299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder totalReservedRecorder;

	// Token: 0x0400012C RID: 300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder systemRecorder;

	// Token: 0x0400012D RID: 301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder gcReservedMemoryRecorder;

	// Token: 0x0400012E RID: 302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder gcUsedMemoryRecorder;

	// Token: 0x0400012F RID: 303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder gcAllocInFrameMemoryRecorder;

	// Token: 0x04000130 RID: 304
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder gfxUsedMemoryRecorder;

	// Token: 0x04000131 RID: 305
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder mainThreadRecorder;

	// Token: 0x04000132 RID: 306
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder meshBytesRecorder;

	// Token: 0x04000133 RID: 307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder meshCountRecorder;

	// Token: 0x04000134 RID: 308
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder textureBytesRecorder;

	// Token: 0x04000135 RID: 309
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder textureCountRecorder;

	// Token: 0x04000136 RID: 310
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder renderTextureBytesRecorder;

	// Token: 0x04000137 RID: 311
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder renderTextureCountRecorder;

	// Token: 0x04000138 RID: 312
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder setPassCallsRecorder;

	// Token: 0x04000139 RID: 313
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder drawCallsRecorder;

	// Token: 0x0400013A RID: 314
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ProfilerRecorder verticesRecorder;

	// Token: 0x0400013B RID: 315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public StringBuilder sb = new StringBuilder(500);

	// Token: 0x02000027 RID: 39
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SiSizeUnits
	{
		// Token: 0x0400013D RID: 317
		Byte,
		// Token: 0x0400013E RID: 318
		KiB,
		// Token: 0x0400013F RID: 319
		MiB,
		// Token: 0x04000140 RID: 320
		GiB,
		// Token: 0x04000141 RID: 321
		TiB,
		// Token: 0x04000142 RID: 322
		PiB,
		// Token: 0x04000143 RID: 323
		EiB,
		// Token: 0x04000144 RID: 324
		ZiB,
		// Token: 0x04000145 RID: 325
		YiB
	}
}
