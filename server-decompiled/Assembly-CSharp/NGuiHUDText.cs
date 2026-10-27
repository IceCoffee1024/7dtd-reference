using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012C2 RID: 4802
public class NGuiHUDText : MonoBehaviour
{
	// Token: 0x060097FF RID: 38911 RVA: 0x0039307C File Offset: 0x0039127C
	[PublicizedFrom(EAccessModifier.Private)]
	public static int Comparison(NGuiHUDText.Entry a, NGuiHUDText.Entry b)
	{
		if (a.movementStart < b.movementStart)
		{
			return -1;
		}
		if (a.movementStart > b.movementStart)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x06009800 RID: 38912 RVA: 0x0039309F File Offset: 0x0039129F
	public bool isVisible
	{
		get
		{
			return this.mList.Count != 0;
		}
	}

	// Token: 0x17001246 RID: 4678
	// (get) Token: 0x06009801 RID: 38913 RVA: 0x003930AF File Offset: 0x003912AF
	// (set) Token: 0x06009802 RID: 38914 RVA: 0x003930B7 File Offset: 0x003912B7
	public INGUIFont ambigiousFont
	{
		get
		{
			return this.font;
		}
		set
		{
			this.font = value;
		}
	}

	// Token: 0x06009803 RID: 38915 RVA: 0x003930C0 File Offset: 0x003912C0
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiHUDText.Entry Create()
	{
		if (this.mUnused.Count > 0)
		{
			NGuiHUDText.Entry entry = this.mUnused[this.mUnused.Count - 1];
			this.mUnused.RemoveAt(this.mUnused.Count - 1);
			entry.time = Time.realtimeSinceStartup;
			entry.label.depth = NGUITools.CalculateNextDepth(base.gameObject);
			NGUITools.SetActive(entry.label.gameObject, true);
			entry.intialOffset = default(Vector3);
			entry.curveOffset = 0f;
			this.mList.Add(entry);
			return entry;
		}
		NGuiHUDText.Entry entry2 = new NGuiHUDText.Entry();
		entry2.time = Time.realtimeSinceStartup;
		entry2.label = base.gameObject.AddWidget<UILabel>(int.MaxValue);
		entry2.label.name = string.Format("Entry_{0}_Label", this.counter);
		entry2.label.font = this.ambigiousFont;
		entry2.label.fontSize = this.fontSize;
		entry2.label.fontStyle = this.fontStyle;
		entry2.label.applyGradient = this.applyGradient;
		entry2.label.gradientTop = this.gradientTop;
		entry2.label.gradientBottom = this.gradienBottom;
		entry2.label.effectStyle = this.effect;
		entry2.label.effectColor = this.effectColor;
		entry2.label.overflowMethod = UILabel.Overflow.ResizeFreely;
		entry2.label.cachedTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		entry2.isLabel = true;
		entry2.sprite = base.gameObject.AddWidget<UISprite>(int.MaxValue);
		entry2.sprite.name = string.Format("Entry_{0}_Sprite", this.counter);
		entry2.sprite.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
		this.mList.Add(entry2);
		this.counter++;
		return entry2;
	}

	// Token: 0x06009804 RID: 38916 RVA: 0x003932C9 File Offset: 0x003914C9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Delete(NGuiHUDText.Entry ent)
	{
		this.mList.Remove(ent);
		this.mUnused.Add(ent);
		NGUITools.SetActive(ent.label.gameObject, false);
	}

	// Token: 0x06009805 RID: 38917 RVA: 0x003932F8 File Offset: 0x003914F8
	public void Add(object obj, Color c, float stayDuration)
	{
		if (!base.enabled)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		bool flag = false;
		float num = 0f;
		if (obj is float)
		{
			flag = true;
			num = (float)obj;
		}
		else if (obj is int)
		{
			flag = true;
			num = (float)((int)obj);
		}
		if (flag)
		{
			if (num == 0f)
			{
				return;
			}
			int i = this.mList.Count;
			while (i > 0)
			{
				NGuiHUDText.Entry entry = this.mList[--i];
				if (entry.time + 1f >= realtimeSinceStartup && entry.val != 0f)
				{
					if (entry.val < 0f && num < 0f)
					{
						entry.val += num;
						entry.label.text = Mathf.RoundToInt(entry.val).ToString();
						return;
					}
					if (entry.val > 0f && num > 0f)
					{
						entry.val += num;
						entry.label.text = "+" + Mathf.RoundToInt(entry.val).ToString();
						return;
					}
				}
			}
		}
		NGuiHUDText.Entry entry2 = this.Create();
		entry2.stay = stayDuration;
		entry2.label.color = c;
		entry2.label.alpha = 0f;
		entry2.sprite.color = c;
		entry2.val = num;
		if (flag)
		{
			entry2.label.text = ((num < 0f) ? Mathf.RoundToInt(entry2.val).ToString() : ("+" + Mathf.RoundToInt(entry2.val).ToString()));
		}
		else
		{
			entry2.label.text = obj.ToString();
		}
		this.mList.Sort(new Comparison<NGuiHUDText.Entry>(NGuiHUDText.Comparison));
	}

	// Token: 0x06009806 RID: 38918 RVA: 0x003934EC File Offset: 0x003916EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		if (this.ambigiousFont == null)
		{
			foreach (NGUIFont nguifont in Resources.FindObjectsOfTypeAll<NGUIFont>())
			{
				if (nguifont.name.EqualsCaseInsensitive("ReferenceFont"))
				{
					this.ambigiousFont = nguifont;
					break;
				}
			}
			if (this.ambigiousFont == null)
			{
				Log.Error("NGuiHUDText font not found");
			}
		}
		this.fontStyle = this.font.dynamicFontStyle;
	}

	// Token: 0x06009807 RID: 38919 RVA: 0x00393558 File Offset: 0x00391758
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnValidate()
	{
		INGUIFont ambigiousFont = this.ambigiousFont;
		if (ambigiousFont != null && ambigiousFont.isDynamic)
		{
			this.fontStyle = ambigiousFont.dynamicFontStyle;
			this.fontSize = ambigiousFont.defaultSize;
		}
	}

	// Token: 0x06009808 RID: 38920 RVA: 0x00393590 File Offset: 0x00391790
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		int i = this.mList.Count;
		while (i > 0)
		{
			NGuiHUDText.Entry entry = this.mList[--i];
			if (entry.label != null)
			{
				entry.label.enabled = false;
			}
			else
			{
				this.mList.RemoveAt(i);
			}
		}
	}

	// Token: 0x06009809 RID: 38921 RVA: 0x003935E8 File Offset: 0x003917E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		float time = RealTime.time;
		Keyframe[] keys = this.offsetCurve.keys;
		Keyframe[] keys2 = this.alphaCurve.keys;
		Keyframe[] keys3 = this.scaleCurve.keys;
		float time2 = keys[keys.Length - 1].time;
		float time3 = keys2[keys2.Length - 1].time;
		float num = Mathf.Max(keys3[keys3.Length - 1].time, Mathf.Max(time2, time3));
		int i = this.mList.Count;
		while (i > 0)
		{
			NGuiHUDText.Entry entry = this.mList[--i];
			float num2 = time - entry.movementStart;
			entry.curveOffset = this.offsetCurve.Evaluate(num2);
			entry.label.alpha = this.alphaCurve.Evaluate(num2);
			float num3 = this.scaleCurve.Evaluate(time - entry.time);
			if (num3 < 0.001f)
			{
				num3 = 0.001f;
			}
			entry.label.cachedTransform.localScale = new Vector3(num3, num3, num3);
			if (num2 > num)
			{
				this.Delete(entry);
			}
			else
			{
				entry.label.enabled = true;
			}
		}
		float num4 = 0f;
		float num5 = 0f;
		for (int j = 0; j < this.mList.Count; j++)
		{
			NGuiHUDText.Entry entry2 = this.mList[j];
			if (this.verticalStack)
			{
				num5 += (float)(entry2.isLabel ? entry2.label.height : entry2.sprite.height);
				num4 = Mathf.Max(num4, (float)(entry2.isLabel ? entry2.label.width : entry2.sprite.width));
			}
			else
			{
				num4 += (float)(entry2.isLabel ? entry2.label.width : entry2.sprite.width);
				num5 = Mathf.Max(num5, (float)(entry2.isLabel ? entry2.label.height : entry2.sprite.height));
			}
		}
		if (this.verticalStack)
		{
			float num6 = 0f;
			for (int k = 0; k < this.mList.Count; k++)
			{
				NGuiHUDText.Entry entry3 = this.mList[k];
				num6 = Mathf.Max(num6, entry3.curveOffset);
				if (entry3.isLabel)
				{
					entry3.label.cachedTransform.localPosition = new Vector3(0f, num6, 0f) + entry3.intialOffset * num5;
					num6 += Mathf.Round(entry3.label.cachedTransform.localScale.y * (float)entry3.label.fontSize);
				}
				else
				{
					entry3.sprite.cachedTransform.localPosition = new Vector3(0f, num6, 0f) + entry3.intialOffset * num5;
					num6 += Mathf.Round(entry3.sprite.cachedTransform.localScale.y * (float)entry3.sprite.height);
				}
			}
			return;
		}
		float num7 = 0f;
		for (int l = 0; l < this.mList.Count; l++)
		{
			NGuiHUDText.Entry entry4 = this.mList[l];
			if (entry4.isLabel)
			{
				entry4.label.cachedTransform.localPosition = new Vector3(num7 + ((float)entry4.label.width - num4) / 2f, entry4.curveOffset, 0f) + entry4.intialOffset * num5;
				num7 += (float)entry4.label.width;
			}
			else
			{
				entry4.sprite.cachedTransform.localPosition = new Vector3(num7 + ((float)entry4.sprite.width - num4) / 2f, entry4.curveOffset, 0f) + entry4.intialOffset * num5;
				num7 += (float)entry4.sprite.width;
			}
		}
	}

	// Token: 0x0600980A RID: 38922 RVA: 0x00393A3C File Offset: 0x00391C3C
	public void SetEntry(int _index, string _input, bool _isSprite, INGUIAtlas _spriteAtlas = null)
	{
		if (this.mList.Count <= _index)
		{
			return;
		}
		this.mList[_index].isLabel = !_isSprite;
		if (!_isSprite)
		{
			this.mList[_index].label.text = _input;
			this.mList[_index].sprite.atlas = null;
			this.mList[_index].sprite.spriteName = string.Empty;
			return;
		}
		this.mList[_index].label.text = string.Empty;
		this.mList[_index].sprite.atlas = (string.IsNullOrEmpty(_input) ? null : _spriteAtlas);
		this.mList[_index].sprite.spriteName = _input;
	}

	// Token: 0x0600980B RID: 38923 RVA: 0x00393B0F File Offset: 0x00391D0F
	public void SetEntrySize(int _index, int _size)
	{
		this.mList[_index].label.fontSize = _size;
		this.mList[_index].sprite.height = _size;
	}

	// Token: 0x0600980C RID: 38924 RVA: 0x00393B3F File Offset: 0x00391D3F
	public void SetEntryOffset(int _index, Vector3 _offset)
	{
		this.mList[_index].intialOffset = _offset;
	}

	// Token: 0x0600980D RID: 38925 RVA: 0x00393B53 File Offset: 0x00391D53
	public void SetEntryColor(int _index, Color _c)
	{
		this.mList[_index].label.color = _c;
		this.mList[_index].sprite.color = _c;
	}

	// Token: 0x040071BB RID: 29115
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string uiFontName = "ReferenceFont";

	// Token: 0x040071BC RID: 29116
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public INGUIFont font;

	// Token: 0x040071BD RID: 29117
	public int fontSize = 20;

	// Token: 0x040071BE RID: 29118
	public FontStyle fontStyle;

	// Token: 0x040071BF RID: 29119
	public bool applyGradient;

	// Token: 0x040071C0 RID: 29120
	public Color gradientTop = Color.white;

	// Token: 0x040071C1 RID: 29121
	public Color gradienBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x040071C2 RID: 29122
	public UILabel.Effect effect;

	// Token: 0x040071C3 RID: 29123
	public Color effectColor = Color.black;

	// Token: 0x040071C4 RID: 29124
	public bool verticalStack;

	// Token: 0x040071C5 RID: 29125
	public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	// Token: 0x040071C6 RID: 29126
	public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	// Token: 0x040071C7 RID: 29127
	public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	// Token: 0x040071C8 RID: 29128
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<NGuiHUDText.Entry> mList = new List<NGuiHUDText.Entry>();

	// Token: 0x040071C9 RID: 29129
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<NGuiHUDText.Entry> mUnused = new List<NGuiHUDText.Entry>();

	// Token: 0x040071CA RID: 29130
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int counter;

	// Token: 0x020012C3 RID: 4803
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Entry
	{
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x0600980F RID: 38927 RVA: 0x00393C9C File Offset: 0x00391E9C
		public float movementStart
		{
			get
			{
				return this.time + this.stay;
			}
		}

		// Token: 0x040071CB RID: 29131
		public float time;

		// Token: 0x040071CC RID: 29132
		public float stay;

		// Token: 0x040071CD RID: 29133
		public Vector3 intialOffset;

		// Token: 0x040071CE RID: 29134
		public float curveOffset;

		// Token: 0x040071CF RID: 29135
		public float val;

		// Token: 0x040071D0 RID: 29136
		public UILabel label;

		// Token: 0x040071D1 RID: 29137
		public UISprite sprite;

		// Token: 0x040071D2 RID: 29138
		public bool isLabel;
	}
}
