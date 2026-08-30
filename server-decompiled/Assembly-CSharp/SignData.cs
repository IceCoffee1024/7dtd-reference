using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
public class SignData
{
	// Token: 0x0600507A RID: 20602 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData()
	{
	}

	// Token: 0x0600507B RID: 20603 RVA: 0x001EBF2C File Offset: 0x001EA12C
	public static SignData Duplicate(SignData original, string copyName = "")
	{
		SignData signData = new SignData();
		signData.guid = Guid.NewGuid();
		signData.name = (string.IsNullOrEmpty(copyName) ? original.name : copyName);
		signData.layers = null;
		signData.lastModified = DateTime.Now;
		signData.nextPolyId = original.nextPolyId;
		signData.nextGroupId = original.nextGroupId;
		signData.nextTextId = original.nextTextId;
		signData.nextNoiseId = original.nextNoiseId;
		signData.CloneLayersFrom(original);
		return signData;
	}

	// Token: 0x0600507C RID: 20604 RVA: 0x001EBFAC File Offset: 0x001EA1AC
	public void CloneLayersFrom(SignData source)
	{
		if (source.layers == null)
		{
			Log.Error("Failed to clone layers from source with null layers list.");
			return;
		}
		if (this.layers != null)
		{
			this.layers.Clear();
		}
		else
		{
			this.layers = new List<SignData.SignLayer>();
		}
		foreach (SignData.SignLayer signLayer in source.layers)
		{
			this.layers.Add(signLayer.Clone());
		}
		this.nextPolyId = source.nextPolyId;
		this.nextTextId = source.nextTextId;
		this.nextNoiseId = source.nextNoiseId;
		this.nextGroupId = source.nextGroupId;
		this.lastModified = DateTime.Now;
	}

	// Token: 0x0600507D RID: 20605 RVA: 0x001EC078 File Offset: 0x001EA278
	public void Write(BinaryWriter bw)
	{
		StreamUtils.Write(bw, this.guid);
		bw.Write(this.name);
		bw.Write(this.lastModified.ToUniversalTime().Ticks);
		bw.Write(this.nextPolyId);
		bw.Write(this.nextTextId);
		bw.Write(this.nextNoiseId);
		bw.Write(this.nextGroupId);
		List<SignData.SignLayer> list = this.layers;
		bw.Write((list != null) ? list.Count : 0);
		if (this.layers != null)
		{
			foreach (SignData.SignLayer signLayer in this.layers)
			{
				signLayer.Write(bw);
			}
		}
	}

	// Token: 0x0600507E RID: 20606 RVA: 0x001EC14C File Offset: 0x001EA34C
	public static SignData Read(BinaryReader br)
	{
		SignData signData = new SignData
		{
			guid = StreamUtils.ReadGuid(br),
			name = br.ReadString(),
			lastModified = new DateTime(br.ReadInt64(), DateTimeKind.Utc)
		};
		signData.nextPolyId = br.ReadInt32();
		signData.nextTextId = br.ReadInt32();
		signData.nextNoiseId = br.ReadInt32();
		signData.nextGroupId = br.ReadInt32();
		int num = br.ReadInt32();
		signData.layers = new List<SignData.SignLayer>(num);
		for (int i = 0; i < num; i++)
		{
			signData.layers.Add(SignData.SignLayer.Read(br));
		}
		return signData;
	}

	// Token: 0x0600507F RID: 20607 RVA: 0x001EC1EC File Offset: 0x001EA3EC
	public void WriteXml(XmlElement parentElement)
	{
		XmlElement xmlElement = parentElement.AddXmlElement("sign");
		xmlElement.SetAttrib("guid", this.guid.ToString());
		xmlElement.SetAttrib("name", this.name.ToString());
		xmlElement.SetAttrib("modified", this.lastModified.ToString("u", Utils.StandardCulture));
		xmlElement.SetAttribute("next_poly_id", this.nextPolyId.ToString());
		xmlElement.SetAttribute("next_text_id", this.nextTextId.ToString());
		xmlElement.SetAttribute("next_noise_id", this.nextNoiseId.ToString());
		xmlElement.SetAttribute("next_group_id", this.nextGroupId.ToString());
		foreach (SignData.SignLayer signLayer in this.layers)
		{
			signLayer.WriteXml(xmlElement.AddXmlElement("layer"));
		}
	}

	// Token: 0x06005080 RID: 20608 RVA: 0x001EC300 File Offset: 0x001EA500
	public static SignData ReadXML(XElement signElement)
	{
		SignData.<>c__DisplayClass30_0 CS$<>8__locals1;
		CS$<>8__locals1.signElement = signElement;
		SignData signData = new SignData
		{
			guid = Guid.Parse(CS$<>8__locals1.signElement.GetAttribute("guid")),
			name = CS$<>8__locals1.signElement.GetAttribute("name"),
			lastModified = DateTime.ParseExact(CS$<>8__locals1.signElement.GetAttribute("modified"), "u", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
			layers = new List<SignData.SignLayer>()
		};
		signData.nextPolyId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_poly_id", ref CS$<>8__locals1);
		signData.nextTextId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_text_id", ref CS$<>8__locals1);
		signData.nextNoiseId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_noise_id", ref CS$<>8__locals1);
		signData.nextGroupId = SignData.<ReadXML>g__ReadNextIdAttribute|30_0("next_group_id", ref CS$<>8__locals1);
		foreach (XElement xelement in CS$<>8__locals1.signElement.Elements("layer"))
		{
			signData.layers.Add(SignData.SignLayer.LayerFromXml(xelement));
		}
		return signData;
	}

	// Token: 0x06005081 RID: 20609 RVA: 0x001EC430 File Offset: 0x001EA630
	public void SetLayerDefaultName(SignData.SignLayer layer)
	{
		SignData.PolygonSignLayer polygonSignLayer = layer as SignData.PolygonSignLayer;
		if (polygonSignLayer != null)
		{
			this.nextPolyId++;
			polygonSignLayer.SetDefaultName(this.nextPolyId);
			return;
		}
		SignData.TextSignLayer textSignLayer = layer as SignData.TextSignLayer;
		if (textSignLayer != null)
		{
			this.nextTextId++;
			textSignLayer.SetDefaultName(this.nextTextId);
			return;
		}
		SignData.NoiseSignLayer noiseSignLayer = layer as SignData.NoiseSignLayer;
		if (noiseSignLayer != null)
		{
			this.nextNoiseId++;
			noiseSignLayer.SetDefaultName(this.nextNoiseId);
			return;
		}
		SignData.GroupSignLayer groupSignLayer = layer as SignData.GroupSignLayer;
		if (groupSignLayer == null)
		{
			return;
		}
		this.nextGroupId++;
		groupSignLayer.SetDefaultName(this.nextGroupId);
	}

	// Token: 0x06005082 RID: 20610 RVA: 0x001EC4D4 File Offset: 0x001EA6D4
	public void UnpackGroups(bool recursive)
	{
		for (int i = this.layers.Count - 1; i >= 0; i--)
		{
			int num;
			this.TryUnpackTopLevelGroup(i, recursive, out num);
		}
	}

	// Token: 0x06005083 RID: 20611 RVA: 0x001EC504 File Offset: 0x001EA704
	public bool TryUnpackTopLevelGroup(int groupIndex, bool recursive, out int unpackedLayerCount)
	{
		unpackedLayerCount = 0;
		SignData.GroupSignLayer groupSignLayer = this.layers[groupIndex] as SignData.GroupSignLayer;
		if (groupSignLayer != null)
		{
			this.layers.RemoveAt(groupIndex);
			this.UnpackLayers(groupSignLayer.transform, groupSignLayer.layers, this.layers, groupIndex, recursive, ref unpackedLayerCount);
			return true;
		}
		return false;
	}

	// Token: 0x06005084 RID: 20612 RVA: 0x001EC554 File Offset: 0x001EA754
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnpackLayers(SignData.SignTransform parentTransform, List<SignData.SignLayer> sourceLayers, List<SignData.SignLayer> destLayers, int destIndex, bool recursive, ref int unpackedLayerCount)
	{
		int i = sourceLayers.Count - 1;
		while (i >= 0)
		{
			SignData.SignLayer signLayer = sourceLayers[i];
			sourceLayers.RemoveAt(i);
			if (!recursive)
			{
				goto IL_0049;
			}
			SignData.GroupSignLayer groupSignLayer = signLayer as SignData.GroupSignLayer;
			if (groupSignLayer == null)
			{
				goto IL_0049;
			}
			this.UnpackLayers(parentTransform * groupSignLayer.transform, groupSignLayer.layers, destLayers, destIndex, recursive, ref unpackedLayerCount);
			IL_006C:
			i--;
			continue;
			IL_0049:
			signLayer.transform = parentTransform * signLayer.transform;
			destLayers.Insert(destIndex, signLayer);
			unpackedLayerCount++;
			goto IL_006C;
		}
	}

	// Token: 0x06005085 RID: 20613 RVA: 0x001EC5D8 File Offset: 0x001EA7D8
	public static SignData GetTestSignData(string text = "Hello World!", float hueOffset = 0f)
	{
		SignData signData = new SignData();
		signData.guid = Guid.NewGuid();
		signData.name = text;
		List<SignData.SignLayer> list = new List<SignData.SignLayer>();
		for (int i = 0; i < 20; i++)
		{
			float num = (float)i / 20f;
			float num2 = 8f + Mathf.Lerp(360f, 0f, num);
			float num3 = (Mathf.Lerp(1f, 0f, num) + hueOffset) % 1f;
			list.Add(new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, num2, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(num3, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.5f));
		}
		list.Add(new SignData.GroupSignLayer("Group Sign Layer", new Vector2(-1f, 0.3f), 8f, Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), SignData.DefaultSignData.PolygonGroupLayers, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply));
		list.Add(new SignData.GroupSignLayer("Group Sign Layer", new Vector2(-0.7f, 0.45f), 8f, 0.5f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), SignData.DefaultSignData.PolygonGroupLayers, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply));
		list.Add(new SignData.GroupSignLayer("Group Sign Layer", new Vector2(0.5f, -0.5f), 8f, 1.25f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), SignData.DefaultSignData.PolygonGroupLayers, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", new Vector2(0.025f, -0.025f), 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.black, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 1f, 0.25f));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.5f));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(0f + hueOffset, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.15f));
		list.Add(new SignData.TextSignLayer("Text Sign Layer", new Vector2(-0.005f, 0.005f), 8f, SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(0.05f + hueOffset, 0.9f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 1f, -0.5f));
		signData.layers = new List<SignData.SignLayer>
		{
			new SignData.GroupSignLayer("Group Sign Layer", Vector2.zero, 0f, 0.55f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), list, SignData.GroupSignLayer.OffsetTarget.All, 0f, 0f, SignData.GroupSignLayer.ColorMode.Multiply)
		};
		signData.lastModified = DateTime.Now;
		return signData;
	}

	// Token: 0x06005086 RID: 20614 RVA: 0x001EC94C File Offset: 0x001EAB4C
	public static SignData GetBasicTestSignData(string text = "Hello World!", float hueOffset = 0f)
	{
		return new SignData
		{
			guid = Guid.NewGuid(),
			name = text,
			layers = new List<SignData.SignLayer>
			{
				new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.85f * Vector2.one, new SignData.SignRenderSettings(Color.HSVToRGB(0.58f + hueOffset, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 8, 0f, 0f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
				new SignData.TextSignLayer("Text Sign Layer", Vector2.zero, 8f, 0.55f * SignData.DefaultSignData.textSize, new SignData.SignRenderSettings(Color.HSVToRGB(0.16f + hueOffset, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), "LiberationSans", text, 0f, 0f, 0f, 0.15f)
			},
			lastModified = DateTime.Now
		};
	}

	// Token: 0x06005087 RID: 20615 RVA: 0x001ECA54 File Offset: 0x001EAC54
	public static SignData GetErrorSignData()
	{
		return new SignData
		{
			guid = Guid.NewGuid(),
			name = "Error",
			layers = new List<SignData.SignLayer>
			{
				new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 3f * Vector2.one, new SignData.SignRenderSettings(new Color(1f, 0f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 4, 1f, 0f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal)
			},
			lastModified = DateTime.Now
		};
	}

	// Token: 0x06005088 RID: 20616 RVA: 0x001ECAF7 File Offset: 0x001EACF7
	public static SignData GetNewSignData(string signName)
	{
		return new SignData
		{
			guid = Guid.NewGuid(),
			name = signName,
			layers = new List<SignData.SignLayer>(),
			lastModified = DateTime.Now
		};
	}

	// Token: 0x06005089 RID: 20617 RVA: 0x001ECB26 File Offset: 0x001EAD26
	public static SignData GetDefaultSignData()
	{
		SignData testSignData = SignData.GetTestSignData("Hello World!", 0f);
		testSignData.guid = SignData.defaultSignDataID.signGuid;
		return testSignData;
	}

	// Token: 0x0600508B RID: 20619 RVA: 0x001ECB60 File Offset: 0x001EAD60
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <ReadXML>g__ReadNextIdAttribute|30_0(string attribute, ref SignData.<>c__DisplayClass30_0 A_1)
	{
		string text;
		if (A_1.signElement.TryGetAttribute(attribute, out text))
		{
			return StringParsers.ParseSInt32(text, 0, -1, NumberStyles.Integer);
		}
		return 0;
	}

	// Token: 0x04003DB0 RID: 15792
	public Guid guid;

	// Token: 0x04003DB1 RID: 15793
	public string name;

	// Token: 0x04003DB2 RID: 15794
	public List<SignData.SignLayer> layers;

	// Token: 0x04003DB3 RID: 15795
	public DateTime lastModified;

	// Token: 0x04003DB4 RID: 15796
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPolyId;

	// Token: 0x04003DB5 RID: 15797
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextTextId;

	// Token: 0x04003DB6 RID: 15798
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextNoiseId;

	// Token: 0x04003DB7 RID: 15799
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextGroupId;

	// Token: 0x04003DB8 RID: 15800
	public static readonly GlobalSignId defaultSignDataID = new GlobalSignId("[D]", Guid.Empty);

	// Token: 0x02000A61 RID: 2657
	public struct SignTransform : IEquatable<SignData.SignTransform>
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x0600508C RID: 20620 RVA: 0x001ECB8D File Offset: 0x001EAD8D
		public bool HasUniformScale
		{
			get
			{
				return Mathf.Approximately(this.scale.x, this.scale.y);
			}
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001ECBAA File Offset: 0x001EADAA
		public SignTransform(Vector2 position, float rotation, Vector2 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001ECBC4 File Offset: 0x001EADC4
		public static SignData.SignTransform operator *(SignData.SignTransform parent, SignData.SignTransform child)
		{
			float num = parent.rotation * 0.017453292f;
			Vector2 vector = Vector2.Scale(child.position, parent.scale);
			Vector2 vector2 = new Vector2(vector.x * Mathf.Cos(num) - vector.y * Mathf.Sin(num), vector.x * Mathf.Sin(num) + vector.y * Mathf.Cos(num));
			Vector2 vector3 = parent.position + vector2;
			float num2 = parent.rotation + child.rotation;
			Vector2 vector4 = Vector2.Scale(parent.scale, child.scale);
			return new SignData.SignTransform(vector3, num2, vector4);
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x001ECC62 File Offset: 0x001EAE62
		public bool Equals(SignData.SignTransform other)
		{
			return this.position == other.position && Mathf.Approximately(this.rotation, other.rotation) && this.scale == other.scale;
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x001ECCA0 File Offset: 0x001EAEA0
		public override bool Equals(object obj)
		{
			if (obj is SignData.SignTransform)
			{
				SignData.SignTransform signTransform = (SignData.SignTransform)obj;
				return this.Equals(signTransform);
			}
			return false;
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x001ECCC5 File Offset: 0x001EAEC5
		public override int GetHashCode()
		{
			return ((17 * 23 + this.position.GetHashCode()) * 23 + this.rotation.GetHashCode()) * 23 + this.scale.GetHashCode();
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001ECD02 File Offset: 0x001EAF02
		public static bool operator ==(SignData.SignTransform left, SignData.SignTransform right)
		{
			return left.Equals(right);
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x001ECD0C File Offset: 0x001EAF0C
		public static bool operator !=(SignData.SignTransform left, SignData.SignTransform right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x001ECD1C File Offset: 0x001EAF1C
		public void WriteXml(XmlElement element)
		{
			element.SetAttrib("pos", string.Format("{0}, {1}", this.position.x, this.position.y));
			element.SetAttrib("rot", this.rotation.ToCultureInvariantString());
			element.SetAttrib("scale", string.Format("{0}, {1}", this.scale.x, this.scale.y));
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001ECDAC File Offset: 0x001EAFAC
		public void ReadXml(XElement element)
		{
			this.position = StringParsers.ParseVector2(element.GetAttribute("pos"));
			this.rotation = float.Parse(element.GetAttribute("rot"), Utils.StandardCulture);
			this.scale = StringParsers.ParseVector2(element.GetAttribute("scale"));
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x001ECE0F File Offset: 0x001EB00F
		public void Write(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.position);
			bw.Write(this.rotation);
			StreamUtils.Write(bw, this.scale);
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x001ECE38 File Offset: 0x001EB038
		public static SignData.SignTransform Read(BinaryReader br)
		{
			Vector2 vector = StreamUtils.ReadVector2(br);
			float num = br.ReadSingle();
			Vector2 vector2 = StreamUtils.ReadVector2(br);
			return new SignData.SignTransform(vector, num, vector2);
		}

		// Token: 0x04003DB9 RID: 15801
		public Vector2 position;

		// Token: 0x04003DBA RID: 15802
		public float rotation;

		// Token: 0x04003DBB RID: 15803
		public Vector2 scale;

		// Token: 0x04003DBC RID: 15804
		public static readonly SignData.SignTransform Defaults = new SignData.SignTransform(Vector2.zero, 0f, new Vector2(0.3f, 0.3f));

		// Token: 0x04003DBD RID: 15805
		public static readonly SignData.SignTransform Identity = new SignData.SignTransform(Vector2.zero, 0f, Vector2.one);
	}

	// Token: 0x02000A62 RID: 2658
	public struct SignRenderSettings
	{
		// Token: 0x06005099 RID: 20633 RVA: 0x001ECE9E File Offset: 0x001EB09E
		public SignRenderSettings(Color color, SignData.SignRenderSettings.Mode mode = SignData.SignRenderSettings.Mode.ColorOnly)
		{
			this.color = color;
			this.mode = mode;
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x001ECEAE File Offset: 0x001EB0AE
		public void WriteXml(XmlElement element)
		{
			element.SetAttrib("color", "#" + ColorUtility.ToHtmlStringRGBA(this.color));
			element.SetAttrib("mode", this.mode.ToString());
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x001ECEF0 File Offset: 0x001EB0F0
		public void ReadXml(XElement element)
		{
			string text = (element.HasAttribute("tint") ? element.GetAttribute("tint") : element.GetAttribute("color"));
			if (!ColorUtility.TryParseHtmlString(text, out this.color))
			{
				Log.Error("Invalid color string: " + text);
				this.color = Color.white;
			}
			if (element.HasAttribute("mode"))
			{
				string attribute = element.GetAttribute("mode");
				if (!Enum.TryParse<SignData.SignRenderSettings.Mode>(attribute, true, out this.mode))
				{
					Log.Error("Failed to parse SignRenderSettings mode from string: \"" + attribute + "\"");
					this.mode = SignData.SignRenderSettings.Mode.ColorAndMask;
				}
			}
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001ECFA9 File Offset: 0x001EB1A9
		public void Write(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.color);
			bw.Write((byte)this.mode);
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001ECFC4 File Offset: 0x001EB1C4
		public static SignData.SignRenderSettings Read(BinaryReader br)
		{
			Color color = StreamUtils.ReadColor(br);
			SignData.SignRenderSettings.Mode mode = (SignData.SignRenderSettings.Mode)br.ReadByte();
			return new SignData.SignRenderSettings(color, mode);
		}

		// Token: 0x04003DBE RID: 15806
		public Color color;

		// Token: 0x04003DBF RID: 15807
		public SignData.SignRenderSettings.Mode mode;

		// Token: 0x02000A63 RID: 2659
		public enum Mode : byte
		{
			// Token: 0x04003DC1 RID: 15809
			ColorOnly,
			// Token: 0x04003DC2 RID: 15810
			ColorAndMask,
			// Token: 0x04003DC3 RID: 15811
			MaskOnly,
			// Token: 0x04003DC4 RID: 15812
			PunchOut
		}
	}

	// Token: 0x02000A64 RID: 2660
	public abstract class SignWarp
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x0600509E RID: 20638
		public abstract SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x0600509F RID: 20639
		public abstract SignData.SignWarp Clone();

		// Token: 0x060050A0 RID: 20640
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void ReadXml(XElement element);

		// Token: 0x060050A1 RID: 20641
		public abstract void WriteXml(XmlElement xmlElement);

		// Token: 0x060050A2 RID: 20642
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalRead(BinaryReader br);

		// Token: 0x060050A3 RID: 20643
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalWrite(BinaryWriter bw);

		// Token: 0x060050A4 RID: 20644 RVA: 0x001ECFE4 File Offset: 0x001EB1E4
		public void Write(BinaryWriter bw)
		{
			bw.Write((byte)this.TypeId);
			this.InternalWrite(bw);
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x001ECFFC File Offset: 0x001EB1FC
		public static SignData.SignWarp Read(BinaryReader br)
		{
			SignData.SignWarp.WarpType warpType = (SignData.SignWarp.WarpType)br.ReadByte();
			SignData.SignWarp signWarp;
			switch (warpType)
			{
			case SignData.SignWarp.WarpType.Skew:
				signWarp = new SignData.SkewWarp();
				break;
			case SignData.SignWarp.WarpType.Bulge:
				signWarp = new SignData.BulgeWarp();
				break;
			case SignData.SignWarp.WarpType.Twirl:
				signWarp = new SignData.TwirlWarp();
				break;
			case SignData.SignWarp.WarpType.Kaleido:
				signWarp = new SignData.KaleidoWarp();
				break;
			case SignData.SignWarp.WarpType.Perspective:
				signWarp = new SignData.PerspectiveWarp();
				break;
			case SignData.SignWarp.WarpType.Arc:
				signWarp = new SignData.ArcWarp();
				break;
			case SignData.SignWarp.WarpType.Stretch:
				signWarp = new SignData.StretchWarp();
				break;
			case SignData.SignWarp.WarpType.Grid:
				signWarp = new SignData.GridWarp();
				break;
			default:
				throw new Exception(string.Format("Unknown warp type id: {0}", (byte)warpType));
			}
			SignData.SignWarp signWarp2 = signWarp;
			signWarp2.InternalRead(br);
			return signWarp2;
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x001ED098 File Offset: 0x001EB298
		public static SignData.SignWarp WarpFromXml(XElement element)
		{
			string attribute = element.GetAttribute("type");
			uint num = <PrivateImplementationDetails>.ComputeStringHash(attribute);
			SignData.SignWarp signWarp;
			if (num <= 2239152927U)
			{
				if (num <= 812224709U)
				{
					if (num != 542699362U)
					{
						if (num == 812224709U)
						{
							if (attribute == "ArcWarp")
							{
								signWarp = new SignData.ArcWarp();
								goto IL_015C;
							}
						}
					}
					else if (attribute == "KaleidoWarp")
					{
						signWarp = new SignData.KaleidoWarp();
						goto IL_015C;
					}
				}
				else if (num != 1765275369U)
				{
					if (num == 2239152927U)
					{
						if (attribute == "PerspectiveWarp")
						{
							signWarp = new SignData.PerspectiveWarp();
							goto IL_015C;
						}
					}
				}
				else if (attribute == "SkewWarp")
				{
					signWarp = new SignData.SkewWarp();
					goto IL_015C;
				}
			}
			else if (num <= 3251058595U)
			{
				if (num != 2846920778U)
				{
					if (num == 3251058595U)
					{
						if (attribute == "TwirlWarp")
						{
							signWarp = new SignData.TwirlWarp();
							goto IL_015C;
						}
					}
				}
				else if (attribute == "BulgeWarp")
				{
					signWarp = new SignData.BulgeWarp();
					goto IL_015C;
				}
			}
			else if (num != 3544788227U)
			{
				if (num == 4106962192U)
				{
					if (attribute == "StretchWarp")
					{
						signWarp = new SignData.StretchWarp();
						goto IL_015C;
					}
				}
			}
			else if (attribute == "GridWarp")
			{
				signWarp = new SignData.GridWarp();
				goto IL_015C;
			}
			throw new Exception("Unknown warp type: " + attribute);
			IL_015C:
			SignData.SignWarp signWarp2 = signWarp;
			signWarp2.ReadXml(element);
			return signWarp2;
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SignWarp()
		{
		}

		// Token: 0x02000A65 RID: 2661
		public enum WarpType : byte
		{
			// Token: 0x04003DC6 RID: 15814
			Skew,
			// Token: 0x04003DC7 RID: 15815
			Bulge,
			// Token: 0x04003DC8 RID: 15816
			Twirl,
			// Token: 0x04003DC9 RID: 15817
			Kaleido,
			// Token: 0x04003DCA RID: 15818
			Perspective,
			// Token: 0x04003DCB RID: 15819
			Arc,
			// Token: 0x04003DCC RID: 15820
			Stretch,
			// Token: 0x04003DCD RID: 15821
			Grid
		}
	}

	// Token: 0x02000A66 RID: 2662
	public class SkewWarp : SignData.SignWarp
	{
		// Token: 0x060050A8 RID: 20648 RVA: 0x001ED209 File Offset: 0x001EB409
		public SkewWarp()
		{
			this.amount = new Vector2(0f, 0.1f);
			this.rotation = 0f;
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x001ED231 File Offset: 0x001EB431
		public SkewWarp(Vector2 amount, float rotation)
		{
			this.amount = amount;
			this.rotation = rotation;
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001ED247 File Offset: 0x001EB447
		public override SignData.SignWarp Clone()
		{
			return new SignData.SkewWarp(this.amount, this.rotation);
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x001ED25C File Offset: 0x001EB45C
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("amount", string.Format("{0}, {1}", this.amount.x, this.amount.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x001ED2D6 File Offset: 0x001EB4D6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.amount = StringParsers.ParseVector2(element.GetAttribute("amount"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060050AD RID: 20653 RVA: 0x00010E62 File Offset: 0x0000F062
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Skew;
			}
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x001ED313 File Offset: 0x001EB513
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.amount);
			bw.Write(this.rotation);
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x001ED32D File Offset: 0x001EB52D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.amount = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
		}

		// Token: 0x04003DCE RID: 15822
		public Vector2 amount;

		// Token: 0x04003DCF RID: 15823
		public float rotation;
	}

	// Token: 0x02000A67 RID: 2663
	public class BulgeWarp : SignData.SignWarp
	{
		// Token: 0x060050B0 RID: 20656 RVA: 0x001ED347 File Offset: 0x001EB547
		public BulgeWarp()
		{
			this.offset = Vector2.zero;
			this.amount = 2f;
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x001ED365 File Offset: 0x001EB565
		public BulgeWarp(Vector2 offset, float amount)
		{
			this.offset = offset;
			this.amount = amount;
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001ED37B File Offset: 0x001EB57B
		public override SignData.SignWarp Clone()
		{
			return new SignData.BulgeWarp(this.offset, this.amount);
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001ED390 File Offset: 0x001EB590
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("amount", this.amount.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x001ED40A File Offset: 0x001EB60A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.amount = float.Parse(element.GetAttribute("amount"), Utils.StandardCulture);
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Bulge;
			}
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x001ED447 File Offset: 0x001EB647
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.amount);
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x001ED461 File Offset: 0x001EB661
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.amount = br.ReadSingle();
		}

		// Token: 0x04003DD0 RID: 15824
		public Vector2 offset;

		// Token: 0x04003DD1 RID: 15825
		public float amount;
	}

	// Token: 0x02000A68 RID: 2664
	public class TwirlWarp : SignData.SignWarp
	{
		// Token: 0x060050B8 RID: 20664 RVA: 0x001ED47B File Offset: 0x001EB67B
		public TwirlWarp()
		{
			this.offset = Vector2.zero;
			this.amount = 0.2f;
			this.frequency = 0.2f;
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x001ED4A4 File Offset: 0x001EB6A4
		public TwirlWarp(Vector2 offset, float amount, float frequency)
		{
			this.offset = offset;
			this.amount = amount;
			this.frequency = frequency;
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001ED4C1 File Offset: 0x001EB6C1
		public override SignData.SignWarp Clone()
		{
			return new SignData.TwirlWarp(this.offset, this.amount, this.frequency);
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x001ED4DC File Offset: 0x001EB6DC
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("amount", this.amount.ToCultureInvariantString() ?? "");
			element.SetAttrib("frequency", this.frequency.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x001ED578 File Offset: 0x001EB778
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.amount = float.Parse(element.GetAttribute("amount"), Utils.StandardCulture);
			this.frequency = float.Parse(element.GetAttribute("frequency"), Utils.StandardCulture);
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Twirl;
			}
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x001ED5E0 File Offset: 0x001EB7E0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.amount);
			bw.Write(this.frequency);
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x001ED606 File Offset: 0x001EB806
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.amount = br.ReadSingle();
			this.frequency = br.ReadSingle();
		}

		// Token: 0x04003DD2 RID: 15826
		public Vector2 offset;

		// Token: 0x04003DD3 RID: 15827
		public float amount;

		// Token: 0x04003DD4 RID: 15828
		public float frequency;
	}

	// Token: 0x02000A69 RID: 2665
	public class KaleidoWarp : SignData.SignWarp
	{
		// Token: 0x060050C0 RID: 20672 RVA: 0x001ED62C File Offset: 0x001EB82C
		public KaleidoWarp()
		{
			this.offset = Vector2.zero;
			this.sides = 6;
			this.rotation = 0f;
			this.offsetScale = 1f;
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x001ED65C File Offset: 0x001EB85C
		public KaleidoWarp(Vector2 offset, int sides, float rotation, float offsetScale)
		{
			this.offset = offset;
			this.sides = sides;
			this.rotation = rotation;
			this.offsetScale = offsetScale;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x001ED681 File Offset: 0x001EB881
		public override SignData.SignWarp Clone()
		{
			return new SignData.KaleidoWarp(this.offset, this.sides, this.rotation, this.offsetScale);
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x001ED6A0 File Offset: 0x001EB8A0
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("sides", string.Format("{0}", this.sides));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("offsetScale", this.offsetScale.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x001ED75C File Offset: 0x001EB95C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.sides = int.Parse(element.GetAttribute("sides"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.offsetScale = (element.HasAttribute("offsetScale") ? float.Parse(element.GetAttribute("offsetScale"), Utils.StandardCulture) : 1f);
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060050C5 RID: 20677 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Kaleido;
			}
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x001ED7F8 File Offset: 0x001EB9F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.sides);
			bw.Write(this.rotation);
			bw.Write(this.offsetScale);
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x001ED82A File Offset: 0x001EBA2A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.sides = br.ReadInt32();
			this.rotation = br.ReadSingle();
			this.offsetScale = br.ReadSingle();
		}

		// Token: 0x04003DD5 RID: 15829
		public Vector2 offset;

		// Token: 0x04003DD6 RID: 15830
		public int sides;

		// Token: 0x04003DD7 RID: 15831
		public float rotation;

		// Token: 0x04003DD8 RID: 15832
		public float offsetScale;
	}

	// Token: 0x02000A6A RID: 2666
	public class PerspectiveWarp : SignData.SignWarp
	{
		// Token: 0x060050C8 RID: 20680 RVA: 0x001ED85C File Offset: 0x001EBA5C
		public PerspectiveWarp()
		{
			this.rotation = new Vector3(0f, 30f, 0f);
			this.strength = 0.1f;
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x001ED889 File Offset: 0x001EBA89
		public PerspectiveWarp(Vector3 rotation, float strength)
		{
			this.rotation = rotation;
			this.strength = strength;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x001ED89F File Offset: 0x001EBA9F
		public override SignData.SignWarp Clone()
		{
			return new SignData.PerspectiveWarp(this.rotation, this.strength);
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x001ED8B4 File Offset: 0x001EBAB4
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("rotation", string.Format("{0}, {1}, {2}", this.rotation.x, this.rotation.y, this.rotation.z));
			element.SetAttrib("strength", this.strength.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001ED93E File Offset: 0x001EBB3E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.rotation = StringParsers.ParseVector3(element.GetAttribute("rotation"), 0, -1);
			this.strength = float.Parse(element.GetAttribute("strength"), Utils.StandardCulture);
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Perspective;
			}
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x001ED97D File Offset: 0x001EBB7D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.rotation);
			bw.Write(this.strength);
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x001ED997 File Offset: 0x001EBB97
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.rotation = StreamUtils.ReadVector3(br);
			this.strength = br.ReadSingle();
		}

		// Token: 0x04003DD9 RID: 15833
		public Vector3 rotation;

		// Token: 0x04003DDA RID: 15834
		public float strength;
	}

	// Token: 0x02000A6B RID: 2667
	public class ArcWarp : SignData.SignWarp
	{
		// Token: 0x060050D0 RID: 20688 RVA: 0x001ED9B1 File Offset: 0x001EBBB1
		public ArcWarp()
		{
			this.rotation = 0f;
			this.radius = 2f;
			this.width = 0.1f;
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001ED9DA File Offset: 0x001EBBDA
		public ArcWarp(float rotation, float radius, float width)
		{
			this.rotation = rotation;
			this.radius = radius;
			this.width = width;
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001ED9F7 File Offset: 0x001EBBF7
		public override SignData.SignWarp Clone()
		{
			return new SignData.ArcWarp(this.rotation, this.radius, this.width);
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x001EDA10 File Offset: 0x001EBC10
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("radius", this.radius.ToCultureInvariantString() ?? "");
			element.SetAttrib("width", this.width.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x001EDA94 File Offset: 0x001EBC94
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.radius = float.Parse(element.GetAttribute("radius"), Utils.StandardCulture);
			this.width = float.Parse(element.GetAttribute("width"), Utils.StandardCulture);
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060050D5 RID: 20693 RVA: 0x00080065 File Offset: 0x0007E265
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Arc;
			}
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x001EDB01 File Offset: 0x001EBD01
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.rotation);
			bw.Write(this.radius);
			bw.Write(this.width);
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x001EDB27 File Offset: 0x001EBD27
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.rotation = br.ReadSingle();
			this.radius = br.ReadSingle();
			this.width = br.ReadSingle();
		}

		// Token: 0x04003DDB RID: 15835
		public float rotation;

		// Token: 0x04003DDC RID: 15836
		public float radius;

		// Token: 0x04003DDD RID: 15837
		public float width;
	}

	// Token: 0x02000A6C RID: 2668
	public class StretchWarp : SignData.SignWarp
	{
		// Token: 0x060050D8 RID: 20696 RVA: 0x001EDB50 File Offset: 0x001EBD50
		public StretchWarp()
		{
			this.offset = new Vector2(0f, 0f);
			this.rotation = 0f;
			this.distance = 0f;
			this.width = 0f;
			this.exponent = 0f;
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x001EDBA4 File Offset: 0x001EBDA4
		public StretchWarp(Vector2 offset, float rotation, float distance, float width, float exponent)
		{
			this.offset = offset;
			this.rotation = rotation;
			this.distance = distance;
			this.width = width;
			this.exponent = exponent;
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x001EDBD1 File Offset: 0x001EBDD1
		public override SignData.SignWarp Clone()
		{
			return new SignData.StretchWarp(this.offset, this.rotation, this.distance, this.width, this.exponent);
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x001EDBF8 File Offset: 0x001EBDF8
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("distance", this.distance.ToCultureInvariantString() ?? "");
			element.SetAttrib("width", this.width.ToCultureInvariantString() ?? "");
			element.SetAttrib("exponent", this.exponent.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x001EDCD4 File Offset: 0x001EBED4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.distance = float.Parse(element.GetAttribute("distance"), Utils.StandardCulture);
			this.width = float.Parse(element.GetAttribute("width"), Utils.StandardCulture);
			this.exponent = float.Parse(element.GetAttribute("exponent"), Utils.StandardCulture);
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060050DD RID: 20701 RVA: 0x00081148 File Offset: 0x0007F348
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Stretch;
			}
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x001EDD7C File Offset: 0x001EBF7C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.rotation);
			bw.Write(this.distance);
			bw.Write(this.width);
			bw.Write(this.exponent);
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x001EDDBA File Offset: 0x001EBFBA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offset = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
			this.distance = br.ReadSingle();
			this.width = br.ReadSingle();
			this.exponent = br.ReadSingle();
		}

		// Token: 0x04003DDE RID: 15838
		public Vector2 offset;

		// Token: 0x04003DDF RID: 15839
		public float rotation;

		// Token: 0x04003DE0 RID: 15840
		public float distance;

		// Token: 0x04003DE1 RID: 15841
		public float width;

		// Token: 0x04003DE2 RID: 15842
		public float exponent;
	}

	// Token: 0x02000A6D RID: 2669
	public class GridWarp : SignData.SignWarp
	{
		// Token: 0x060050E0 RID: 20704 RVA: 0x001EDDF8 File Offset: 0x001EBFF8
		public GridWarp()
		{
			this.mode = SignData.GridWarp.Mode.Rectangle;
			this.offset = Vector2.zero;
			this.rotation = 0f;
			this.scale = 1f;
			this.aspect = 1f;
			this.shift = 0f;
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x001EDE49 File Offset: 0x001EC049
		public GridWarp(SignData.GridWarp.Mode mode, Vector2 offset, float rotation, float scale, float aspect, float shift)
		{
			this.mode = mode;
			this.offset = offset;
			this.rotation = rotation;
			this.scale = scale;
			this.aspect = aspect;
			this.shift = shift;
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x001EDE7E File Offset: 0x001EC07E
		public override SignData.SignWarp Clone()
		{
			return new SignData.GridWarp(this.mode, this.offset, this.rotation, this.scale, this.aspect, this.shift);
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x001EDEAC File Offset: 0x001EC0AC
		public override void WriteXml(XmlElement element)
		{
			element.SetAttrib("type", base.GetType().Name);
			element.SetAttrib("mode", string.Format("{0}", (int)this.mode));
			element.SetAttrib("offset", string.Format("{0}, {1}", this.offset.x, this.offset.y));
			element.SetAttrib("rotation", this.rotation.ToCultureInvariantString() ?? "");
			element.SetAttrib("scale", this.scale.ToCultureInvariantString() ?? "");
			element.SetAttrib("aspect", this.aspect.ToCultureInvariantString() ?? "");
			element.SetAttrib("shift", this.shift.ToCultureInvariantString() ?? "");
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x001EDFA8 File Offset: 0x001EC1A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			this.mode = (SignData.GridWarp.Mode)int.Parse(element.GetAttribute("mode"));
			this.offset = StringParsers.ParseVector2(element.GetAttribute("offset"));
			this.rotation = float.Parse(element.GetAttribute("rotation"), Utils.StandardCulture);
			this.scale = float.Parse(element.GetAttribute("scale"), Utils.StandardCulture);
			this.aspect = (element.HasAttribute("aspect") ? float.Parse(element.GetAttribute("aspect"), Utils.StandardCulture) : 1f);
			this.shift = (element.HasAttribute("shift") ? float.Parse(element.GetAttribute("shift"), Utils.StandardCulture) : 0f);
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060050E5 RID: 20709 RVA: 0x0005FC95 File Offset: 0x0005DE95
		public override SignData.SignWarp.WarpType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignWarp.WarpType.Grid;
			}
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x001EE0A0 File Offset: 0x001EC2A0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write((int)this.mode);
			StreamUtils.Write(bw, this.offset);
			bw.Write(this.rotation);
			bw.Write(this.scale);
			bw.Write(this.aspect);
			bw.Write(this.shift);
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x001EE0F8 File Offset: 0x001EC2F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.mode = (SignData.GridWarp.Mode)br.ReadInt32();
			this.offset = StreamUtils.ReadVector2(br);
			this.rotation = br.ReadSingle();
			this.scale = br.ReadSingle();
			this.aspect = br.ReadSingle();
			this.shift = br.ReadSingle();
		}

		// Token: 0x04003DE3 RID: 15843
		public SignData.GridWarp.Mode mode;

		// Token: 0x04003DE4 RID: 15844
		public Vector2 offset;

		// Token: 0x04003DE5 RID: 15845
		public float rotation;

		// Token: 0x04003DE6 RID: 15846
		public float scale;

		// Token: 0x04003DE7 RID: 15847
		public float aspect;

		// Token: 0x04003DE8 RID: 15848
		public float shift;

		// Token: 0x02000A6E RID: 2670
		public enum Mode
		{
			// Token: 0x04003DEA RID: 15850
			Column,
			// Token: 0x04003DEB RID: 15851
			Rectangle,
			// Token: 0x04003DEC RID: 15852
			Hex
		}
	}

	// Token: 0x02000A6F RID: 2671
	public abstract class SignLayer
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060050E8 RID: 20712 RVA: 0x001EE14D File Offset: 0x001EC34D
		public bool HasWarps
		{
			get
			{
				List<SignData.SignWarp> list = this.warps;
				return list != null && list.Count > 0;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060050E9 RID: 20713 RVA: 0x001EE163 File Offset: 0x001EC363
		public bool HasTransform
		{
			get
			{
				return this.transform != SignData.SignTransform.Identity;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060050EA RID: 20714 RVA: 0x001EE175 File Offset: 0x001EC375
		public bool HasTransformOrWarps
		{
			get
			{
				return this.HasTransform || this.HasWarps;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060050EB RID: 20715
		public abstract SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060050EC RID: 20716
		public abstract string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x060050ED RID: 20717
		public abstract SignData.SignLayer Clone();

		// Token: 0x060050EE RID: 20718
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalRead(BinaryReader br);

		// Token: 0x060050EF RID: 20719
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void InternalWrite(BinaryWriter bw);

		// Token: 0x060050F0 RID: 20720 RVA: 0x001EE188 File Offset: 0x001EC388
		public SignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps)
		{
			this.name = name;
			this.transform.position = position;
			this.transform.rotation = rotation;
			this.transform.scale = scale;
			this.renderSettings = renderSettings;
			this.warps = warps;
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x001EE1D8 File Offset: 0x001EC3D8
		public SignLayer()
		{
			this.name = string.Empty;
			this.transform.position = Vector2.zero;
			this.transform.rotation = 0f;
			this.transform.scale = new Vector2(0.3f, 0.3f);
			this.renderSettings = new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly);
			this.warps = new List<SignData.SignWarp>();
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x001EE24C File Offset: 0x001EC44C
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<SignData.SignWarp> CloneWarps()
		{
			List<SignData.SignWarp> list = new List<SignData.SignWarp>();
			foreach (SignData.SignWarp signWarp in this.warps)
			{
				list.Add(signWarp.Clone());
			}
			return list;
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x001EE2AC File Offset: 0x001EC4AC
		public void Write(BinaryWriter bw)
		{
			bw.Write((byte)this.TypeId);
			this.InternalWrite(bw);
			bw.Write(this.name ?? "");
			this.transform.Write(bw);
			this.renderSettings.Write(bw);
			List<SignData.SignWarp> list = this.warps;
			bw.Write((list != null) ? list.Count : 0);
			if (this.warps != null)
			{
				foreach (SignData.SignWarp signWarp in this.warps)
				{
					signWarp.Write(bw);
				}
			}
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x001EE360 File Offset: 0x001EC560
		public static SignData.SignLayer Read(BinaryReader br)
		{
			SignData.SignLayer.LayerType layerType = (SignData.SignLayer.LayerType)br.ReadByte();
			SignData.SignLayer signLayer;
			switch (layerType)
			{
			case SignData.SignLayer.LayerType.Group:
				signLayer = new SignData.GroupSignLayer();
				break;
			case SignData.SignLayer.LayerType.Text:
				signLayer = new SignData.TextSignLayer();
				break;
			case SignData.SignLayer.LayerType.Polygon:
				signLayer = new SignData.PolygonSignLayer();
				break;
			case SignData.SignLayer.LayerType.Noise:
				signLayer = new SignData.NoiseSignLayer();
				break;
			default:
				throw new Exception(string.Format("Unknown layer type id: {0}", (byte)layerType));
			}
			SignData.SignLayer signLayer2 = signLayer;
			signLayer2.InternalRead(br);
			signLayer2.name = br.ReadString();
			signLayer2.transform = SignData.SignTransform.Read(br);
			signLayer2.renderSettings = SignData.SignRenderSettings.Read(br);
			int num = br.ReadInt32();
			signLayer2.warps = new List<SignData.SignWarp>(num);
			for (int i = 0; i < num; i++)
			{
				signLayer2.warps.Add(SignData.SignWarp.Read(br));
			}
			return signLayer2;
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x001EE424 File Offset: 0x001EC624
		public static SignData.SignLayer LayerFromXml(XElement element)
		{
			string attribute = element.GetAttribute("type");
			SignData.SignLayer signLayer;
			if (!(attribute == "TextSignLayer"))
			{
				if (!(attribute == "PolygonSignLayer"))
				{
					if (!(attribute == "GroupSignLayer"))
					{
						if (!(attribute == "NoiseSignLayer"))
						{
							throw new Exception("Unknown layer type: " + attribute);
						}
						signLayer = new SignData.NoiseSignLayer();
					}
					else
					{
						signLayer = new SignData.GroupSignLayer();
					}
				}
				else
				{
					signLayer = new SignData.PolygonSignLayer();
				}
			}
			else
			{
				signLayer = new SignData.TextSignLayer();
			}
			SignData.SignLayer signLayer2 = signLayer;
			signLayer2.ReadXml(element);
			return signLayer2;
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x001EE4B4 File Offset: 0x001EC6B4
		public virtual void WriteXml(XmlElement element)
		{
			element.SetAttribute("name", this.name);
			element.SetAttrib("type", base.GetType().Name);
			this.transform.WriteXml(element);
			this.renderSettings.WriteXml(element);
			foreach (SignData.SignWarp signWarp in this.warps)
			{
				signWarp.WriteXml(element.AddXmlElement("warp"));
			}
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x001EE550 File Offset: 0x001EC750
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void ReadXml(XElement element)
		{
			element.TryGetAttribute("name", out this.name);
			this.transform.ReadXml(element);
			this.renderSettings.ReadXml(element);
			foreach (XElement xelement in element.Elements("warp"))
			{
				this.warps.Add(SignData.SignWarp.WarpFromXml(xelement));
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x001EE5E0 File Offset: 0x001EC7E0
		public void SetDefaultName(int idx)
		{
			this.name = string.Format(this.defaultName + " " + idx.ToString("00"), Array.Empty<object>());
		}

		// Token: 0x04003DED RID: 15853
		public string name;

		// Token: 0x04003DEE RID: 15854
		public SignData.SignTransform transform;

		// Token: 0x04003DEF RID: 15855
		public SignData.SignRenderSettings renderSettings;

		// Token: 0x04003DF0 RID: 15856
		public List<SignData.SignWarp> warps;

		// Token: 0x02000A70 RID: 2672
		public enum LayerType : byte
		{
			// Token: 0x04003DF2 RID: 15858
			Group,
			// Token: 0x04003DF3 RID: 15859
			Text,
			// Token: 0x04003DF4 RID: 15860
			Polygon,
			// Token: 0x04003DF5 RID: 15861
			Noise
		}
	}

	// Token: 0x02000A71 RID: 2673
	public class GroupSignLayer : SignData.SignLayer
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x001EE60E File Offset: 0x001EC80E
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Group";
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x00010E62 File Offset: 0x0000F062
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Group;
			}
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x001EE615 File Offset: 0x001EC815
		public GroupSignLayer()
		{
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x001EE61D File Offset: 0x001EC81D
		public GroupSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, List<SignData.SignLayer> layers, SignData.GroupSignLayer.OffsetTarget offsetTarget = SignData.GroupSignLayer.OffsetTarget.All, float softnessOffset = 0f, float dilateOffset = 0f, SignData.GroupSignLayer.ColorMode colorMode = SignData.GroupSignLayer.ColorMode.Multiply)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.layers = layers;
			this.offsetTarget = offsetTarget;
			this.softnessOffset = softnessOffset;
			this.dilateOffset = dilateOffset;
			this.colorMode = colorMode;
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001EE658 File Offset: 0x001EC858
		public override SignData.SignLayer Clone()
		{
			List<SignData.SignLayer> list = null;
			if (this.layers != null)
			{
				list = new List<SignData.SignLayer>();
				foreach (SignData.SignLayer signLayer in this.layers)
				{
					list.Add(signLayer.Clone());
				}
			}
			return new SignData.GroupSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), list, this.offsetTarget, this.softnessOffset, this.dilateOffset, this.colorMode);
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x001EE714 File Offset: 0x001EC914
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write((byte)this.offsetTarget);
			bw.Write(this.softnessOffset);
			bw.Write(this.dilateOffset);
			bw.Write((byte)this.colorMode);
			List<SignData.SignLayer> list = this.layers;
			bw.Write((list != null) ? list.Count : 0);
			if (this.layers != null)
			{
				foreach (SignData.SignLayer signLayer in this.layers)
				{
					signLayer.Write(bw);
				}
			}
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x001EE7B8 File Offset: 0x001EC9B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.offsetTarget = (SignData.GroupSignLayer.OffsetTarget)br.ReadByte();
			this.softnessOffset = br.ReadSingle();
			this.dilateOffset = br.ReadSingle();
			this.colorMode = (SignData.GroupSignLayer.ColorMode)br.ReadByte();
			int num = br.ReadInt32();
			this.layers = new List<SignData.SignLayer>(num);
			for (int i = 0; i < num; i++)
			{
				this.layers.Add(SignData.SignLayer.Read(br));
			}
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x001EE828 File Offset: 0x001ECA28
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			if (this.offsetTarget != SignData.GroupSignLayer.OffsetTarget.All)
			{
				element.SetAttrib("offsetTarget", this.offsetTarget.ToString());
			}
			if (this.softnessOffset != 0f)
			{
				element.SetAttrib("softnessOffset", this.softnessOffset.ToCultureInvariantString());
			}
			if (this.dilateOffset != 0f)
			{
				element.SetAttrib("dilateOffset", this.dilateOffset.ToCultureInvariantString());
			}
			if (this.colorMode != SignData.GroupSignLayer.ColorMode.Multiply)
			{
				element.SetAttrib("colorMode", this.colorMode.ToString());
			}
			foreach (SignData.SignLayer signLayer in this.layers)
			{
				signLayer.WriteXml(element.AddXmlElement("layer"));
			}
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x001EE91C File Offset: 0x001ECB1C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			string text;
			SignData.GroupSignLayer.OffsetTarget offsetTarget;
			if (element.TryGetAttribute("offsetTarget", out text) && Enum.TryParse<SignData.GroupSignLayer.OffsetTarget>(text, out offsetTarget))
			{
				this.offsetTarget = offsetTarget;
			}
			else
			{
				this.offsetTarget = SignData.GroupSignLayer.OffsetTarget.All;
			}
			string text2;
			if (element.TryGetAttribute("softnessOffset", out text2))
			{
				this.softnessOffset = float.Parse(text2, Utils.StandardCulture);
			}
			string text3;
			if (element.TryGetAttribute("dilateOffset", out text3))
			{
				this.dilateOffset = float.Parse(text3, Utils.StandardCulture);
			}
			string text4;
			SignData.GroupSignLayer.ColorMode colorMode;
			if (element.TryGetAttribute("colorMode", out text4) && Enum.TryParse<SignData.GroupSignLayer.ColorMode>(text4, out colorMode))
			{
				this.colorMode = colorMode;
			}
			else
			{
				this.colorMode = SignData.GroupSignLayer.ColorMode.Multiply;
			}
			this.layers = new List<SignData.SignLayer>();
			foreach (XElement xelement in element.Elements("layer"))
			{
				this.layers.Add(SignData.SignLayer.LayerFromXml(xelement));
			}
		}

		// Token: 0x04003DF6 RID: 15862
		public List<SignData.SignLayer> layers;

		// Token: 0x04003DF7 RID: 15863
		public SignData.GroupSignLayer.OffsetTarget offsetTarget;

		// Token: 0x04003DF8 RID: 15864
		public float softnessOffset;

		// Token: 0x04003DF9 RID: 15865
		public float dilateOffset;

		// Token: 0x04003DFA RID: 15866
		public SignData.GroupSignLayer.ColorMode colorMode;

		// Token: 0x02000A72 RID: 2674
		public enum OffsetTarget : byte
		{
			// Token: 0x04003DFC RID: 15868
			All,
			// Token: 0x04003DFD RID: 15869
			Shapes,
			// Token: 0x04003DFE RID: 15870
			Text,
			// Token: 0x04003DFF RID: 15871
			Noise
		}

		// Token: 0x02000A73 RID: 2675
		public enum ColorMode : byte
		{
			// Token: 0x04003E01 RID: 15873
			Multiply,
			// Token: 0x04003E02 RID: 15874
			Blend,
			// Token: 0x04003E03 RID: 15875
			Override
		}
	}

	// Token: 0x02000A74 RID: 2676
	public class TextSignLayer : SignData.SignLayer
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06005102 RID: 20738 RVA: 0x001EEA40 File Offset: 0x001ECC40
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Text";
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06005103 RID: 20739 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Text;
			}
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x001EEA48 File Offset: 0x001ECC48
		public TextSignLayer()
		{
			this.name = "Text";
			this.font = "LiberationSans";
			this.text = Localization.Get("lblSignLayerTypeText", false, null);
			this.direction = 0f;
			this.spacing = 1f;
			this.softness = 0f;
			this.dilate = 0f;
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x001EEAB0 File Offset: 0x001ECCB0
		public TextSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, string font, string text, float direction, float spacing, float softness, float dilate)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.font = font;
			this.text = text;
			this.direction = direction;
			this.spacing = spacing;
			this.softness = softness;
			this.dilate = dilate;
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x001EEAFC File Offset: 0x001ECCFC
		public override SignData.SignLayer Clone()
		{
			return new SignData.TextSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.font, this.text, this.direction, this.spacing, this.softness, this.dilate);
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x001EEB68 File Offset: 0x001ECD68
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.font);
			bw.Write(this.text);
			bw.Write(this.direction);
			bw.Write(this.spacing);
			bw.Write(this.softness);
			bw.Write(this.dilate);
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x001EEBC0 File Offset: 0x001ECDC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.font = br.ReadString();
			this.text = br.ReadString();
			this.direction = br.ReadSingle();
			this.spacing = br.ReadSingle();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x001EEC18 File Offset: 0x001ECE18
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("font", this.font);
			element.SetAttrib("text", this.text);
			element.SetAttrib("direction", this.direction.ToCultureInvariantString());
			element.SetAttrib("spacing", this.spacing.ToCultureInvariantString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x001EECAC File Offset: 0x001ECEAC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			this.font = element.GetAttribute("font");
			this.text = element.GetAttribute("text");
			this.direction = float.Parse(element.GetAttribute("direction"), Utils.StandardCulture);
			this.spacing = float.Parse(element.GetAttribute("spacing"), Utils.StandardCulture);
			this.softness = float.Parse(element.GetAttribute("softness"), Utils.StandardCulture);
			this.dilate = float.Parse(element.GetAttribute("dilate"), Utils.StandardCulture);
		}

		// Token: 0x04003E04 RID: 15876
		public string font;

		// Token: 0x04003E05 RID: 15877
		public string text;

		// Token: 0x04003E06 RID: 15878
		public float direction;

		// Token: 0x04003E07 RID: 15879
		public float spacing;

		// Token: 0x04003E08 RID: 15880
		public float softness;

		// Token: 0x04003E09 RID: 15881
		public float dilate;
	}

	// Token: 0x02000A75 RID: 2677
	public class PolygonSignLayer : SignData.SignLayer
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x001EED6C File Offset: 0x001ECF6C
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Poly";
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Polygon;
			}
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x001EED74 File Offset: 0x001ECF74
		public PolygonSignLayer()
		{
			this.name = "Polygon";
			this.sides = 4;
			this.smoothness = 0f;
			this.starify = 0f;
			this.softness = 0f;
			this.dilate = 0f;
			this.frequency = 5f;
			this.shapeMode = SignData.PolygonSignLayer.ShapeMode.Normal;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x001EEDD8 File Offset: 0x001ECFD8
		public PolygonSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, int sides, float smoothness, float starify, float softness, float dilate, float frequency, SignData.PolygonSignLayer.ShapeMode shapeMode)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.sides = sides;
			this.smoothness = smoothness;
			this.starify = starify;
			this.softness = softness;
			this.dilate = dilate;
			this.frequency = frequency;
			this.shapeMode = shapeMode;
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001EEE2C File Offset: 0x001ED02C
		public override SignData.SignLayer Clone()
		{
			return new SignData.PolygonSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.sides, this.smoothness, this.starify, this.softness, this.dilate, this.frequency, this.shapeMode);
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x001EEE9C File Offset: 0x001ED09C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.sides);
			bw.Write(this.smoothness);
			bw.Write(this.starify);
			bw.Write(this.softness);
			bw.Write(this.dilate);
			bw.Write(this.frequency);
			bw.Write((byte)this.shapeMode);
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x001EEF00 File Offset: 0x001ED100
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.sides = br.ReadInt32();
			this.smoothness = br.ReadSingle();
			this.starify = br.ReadSingle();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
			this.frequency = br.ReadSingle();
			this.shapeMode = (SignData.PolygonSignLayer.ShapeMode)br.ReadByte();
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x001EEF64 File Offset: 0x001ED164
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("sides", this.sides.ToString());
			element.SetAttrib("smoothness", this.smoothness.ToCultureInvariantString());
			element.SetAttrib("starify", this.starify.ToCultureInvariantString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
			element.SetAttrib("frequency", this.frequency.ToCultureInvariantString());
			element.SetAttrib("shapeMode", this.shapeMode.ToString());
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x001EF020 File Offset: 0x001ED220
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			this.sides = int.Parse(element.GetAttribute("sides"));
			this.smoothness = float.Parse(element.GetAttribute("smoothness"), Utils.StandardCulture);
			this.starify = float.Parse(element.GetAttribute("starify"), Utils.StandardCulture);
			string text;
			string text2;
			if (element.TryGetAttribute("softness", out text) && element.TryGetAttribute("dilate", out text2))
			{
				this.softness = float.Parse(text, Utils.StandardCulture);
				this.dilate = float.Parse(text2, Utils.StandardCulture);
			}
			else
			{
				this.softness = 0f;
				this.dilate = 0f;
			}
			string text3;
			if (element.TryGetAttribute("frequency", out text3))
			{
				this.frequency = float.Parse(text3, Utils.StandardCulture);
			}
			else
			{
				this.frequency = 5f;
			}
			string text4;
			SignData.PolygonSignLayer.ShapeMode shapeMode;
			if (element.TryGetAttribute("shapeMode", out text4) && Enum.TryParse<SignData.PolygonSignLayer.ShapeMode>(text4, out shapeMode))
			{
				this.shapeMode = shapeMode;
				return;
			}
			this.shapeMode = SignData.PolygonSignLayer.ShapeMode.Normal;
		}

		// Token: 0x04003E0A RID: 15882
		public int sides;

		// Token: 0x04003E0B RID: 15883
		public float smoothness;

		// Token: 0x04003E0C RID: 15884
		public float starify;

		// Token: 0x04003E0D RID: 15885
		public float softness;

		// Token: 0x04003E0E RID: 15886
		public float dilate;

		// Token: 0x04003E0F RID: 15887
		public float frequency;

		// Token: 0x04003E10 RID: 15888
		public SignData.PolygonSignLayer.ShapeMode shapeMode;

		// Token: 0x02000A76 RID: 2678
		public enum ShapeMode : byte
		{
			// Token: 0x04003E12 RID: 15890
			Normal,
			// Token: 0x04003E13 RID: 15891
			Invert,
			// Token: 0x04003E14 RID: 15892
			Line,
			// Token: 0x04003E15 RID: 15893
			Ripple
		}
	}

	// Token: 0x02000A77 RID: 2679
	public class NoiseSignLayer : SignData.SignLayer
	{
		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06005114 RID: 20756 RVA: 0x001EF151 File Offset: 0x001ED351
		public override string defaultName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return "Noise";
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06005115 RID: 20757 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override SignData.SignLayer.LayerType TypeId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SignData.SignLayer.LayerType.Noise;
			}
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001EF158 File Offset: 0x001ED358
		public NoiseSignLayer()
		{
			this.name = "Noise";
			this.seed = 0;
			this.detail = 4;
			this.softness = 0f;
			this.dilate = 0f;
			this.fade = 0f;
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x001EF1A5 File Offset: 0x001ED3A5
		public NoiseSignLayer(string name, Vector2 position, float rotation, Vector2 scale, SignData.SignRenderSettings renderSettings, List<SignData.SignWarp> warps, int seed, int detail, float softness, float dilate, float fade)
			: base(name, position, rotation, scale, renderSettings, warps)
		{
			this.seed = seed;
			this.detail = detail;
			this.softness = softness;
			this.dilate = dilate;
			this.fade = fade;
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001EF1E0 File Offset: 0x001ED3E0
		public override SignData.SignLayer Clone()
		{
			return new SignData.NoiseSignLayer(this.name, this.transform.position, this.transform.rotation, this.transform.scale, this.renderSettings, base.CloneWarps(), this.seed, this.detail, this.softness, this.dilate, this.fade);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001EF244 File Offset: 0x001ED444
		public override void WriteXml(XmlElement element)
		{
			base.WriteXml(element);
			element.SetAttrib("seed", this.seed.ToString());
			element.SetAttrib("detail", this.detail.ToString());
			element.SetAttrib("softness", this.softness.ToCultureInvariantString());
			element.SetAttrib("dilate", this.dilate.ToCultureInvariantString());
			element.SetAttrib("fade", this.fade.ToCultureInvariantString());
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x001EF2CC File Offset: 0x001ED4CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ReadXml(XElement element)
		{
			base.ReadXml(element);
			this.seed = int.Parse(element.GetAttribute("seed"));
			this.detail = int.Parse(element.GetAttribute("detail"));
			this.softness = float.Parse(element.GetAttribute("softness"), Utils.StandardCulture);
			this.dilate = float.Parse(element.GetAttribute("dilate"), Utils.StandardCulture);
			string text;
			if (element.TryGetAttribute("fade", out text))
			{
				this.fade = float.Parse(text, Utils.StandardCulture);
				return;
			}
			this.fade = 0f;
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001EF387 File Offset: 0x001ED587
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalWrite(BinaryWriter bw)
		{
			bw.Write(this.seed);
			bw.Write(this.detail);
			bw.Write(this.softness);
			bw.Write(this.dilate);
			bw.Write(this.fade);
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x001EF3C5 File Offset: 0x001ED5C5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void InternalRead(BinaryReader br)
		{
			this.seed = br.ReadInt32();
			this.detail = br.ReadInt32();
			this.softness = br.ReadSingle();
			this.dilate = br.ReadSingle();
			this.fade = br.ReadSingle();
		}

		// Token: 0x04003E16 RID: 15894
		public int seed;

		// Token: 0x04003E17 RID: 15895
		public int detail;

		// Token: 0x04003E18 RID: 15896
		public float softness;

		// Token: 0x04003E19 RID: 15897
		public float dilate;

		// Token: 0x04003E1A RID: 15898
		public float fade;
	}

	// Token: 0x02000A78 RID: 2680
	[PublicizedFrom(EAccessModifier.Private)]
	public static class DefaultSignData
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600511D RID: 20765 RVA: 0x001EF404 File Offset: 0x001ED604
		public static List<SignData.SignLayer> PolygonGroupLayers
		{
			get
			{
				return new List<SignData.SignLayer>
				{
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero + new Vector2(0.005f, -0.005f), 0f, 0.25f * Vector2.one, new SignData.SignRenderSettings(Color.black, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.2f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.25f * Vector2.one, new SignData.SignRenderSettings(Color.white, SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.2f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.23f * Vector2.one, new SignData.SignRenderSettings(Color.HSVToRGB(0.05f, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.165f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal),
					new SignData.PolygonSignLayer("Polygon Sign Layer", Vector2.zero, 0f, 0.18f * Vector2.one, new SignData.SignRenderSettings(Color.HSVToRGB(0.13f, 1f, 1f), SignData.SignRenderSettings.Mode.ColorOnly), new List<SignData.SignWarp>(), 5, 0.15f, 0.5f, 0f, 0f, 20f, SignData.PolygonSignLayer.ShapeMode.Normal)
				};
			}
		}

		// Token: 0x04003E1B RID: 15899
		public static readonly Color color1 = new Color(0.318f, 0.396f, 0.686f);

		// Token: 0x04003E1C RID: 15900
		public static readonly Color color2 = new Color(0.286f, 0.518f, 1f);

		// Token: 0x04003E1D RID: 15901
		public static readonly Color color3 = new Color(0.514f, 0.671f, 1f);

		// Token: 0x04003E1E RID: 15902
		public static readonly Vector2 textSize = 0.65f * Vector2.one;

		// Token: 0x04003E1F RID: 15903
		public const float textRotation = 8f;

		// Token: 0x04003E20 RID: 15904
		public const string font1 = "LiberationSans";

		// Token: 0x04003E21 RID: 15905
		public const string font2 = "AmaticSC";

		// Token: 0x04003E22 RID: 15906
		public const string text = "Hello World!";

		// Token: 0x04003E23 RID: 15907
		public const int polygonSides = 5;

		// Token: 0x04003E24 RID: 15908
		public const float polygonSmoothness = 0.2f;

		// Token: 0x04003E25 RID: 15909
		public const float polygonStarify = 0.5f;
	}
}
