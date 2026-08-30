using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x020018B2 RID: 6322
	public class PrefabInfoVolumeList : PrefabVolumeListAbs<PrefabInfoVolumeList, PrefabInfoVolume>
	{
		// Token: 0x0600C32E RID: 49966 RVA: 0x00482377 File Offset: 0x00480577
		public PrefabInfoVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x0600C32F RID: 49967 RVA: 0x0002F168 File Offset: 0x0002D368
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x0600C330 RID: 49968 RVA: 0x00482380 File Offset: 0x00480580
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryInfoVolume;
			}
		}

		// Token: 0x0600C331 RID: 49969 RVA: 0x0048238C File Offset: 0x0048058C
		public override void ReadFromProperties(DynamicProperties _properties)
		{
			this.List.Clear();
			Dictionary<string, string> values = _properties.Values;
			if (!values.ContainsKey("InfoVolumeSize") || !values.ContainsKey("InfoVolumeStart"))
			{
				return;
			}
			List<Vector3i> list = StringParsers.ParseList<Vector3i>(values["InfoVolumeSize"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["InfoVolumeStart"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			for (int i = 0; i < list2.Count; i++)
			{
				Vector3i vector3i = list2[i];
				Vector3i vector3i2 = ((i < list.Count) ? list[i] : Vector3i.one);
				PrefabInfoVolume prefabInfoVolume = new PrefabInfoVolume();
				prefabInfoVolume.Use(vector3i, vector3i2);
				this.List.Add(prefabInfoVolume);
			}
		}

		// Token: 0x0600C332 RID: 49970 RVA: 0x0048247C File Offset: 0x0048067C
		public override void WriteToProperties(DynamicProperties _properties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (PrefabInfoVolume prefabInfoVolume in this.List)
			{
				if (prefabInfoVolume.Used)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('#');
						stringBuilder2.Append('#');
					}
					stringBuilder.Append(prefabInfoVolume.size.ToString());
					stringBuilder2.Append(prefabInfoVolume.startPos.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				_properties.Values["InfoVolumeSize"] = stringBuilder.ToString();
				_properties.Values["InfoVolumeStart"] = stringBuilder2.ToString();
				return;
			}
			_properties.Values.Remove("InfoVolumeSize");
			_properties.Values.Remove("InfoVolumeStart");
		}
	}
}
