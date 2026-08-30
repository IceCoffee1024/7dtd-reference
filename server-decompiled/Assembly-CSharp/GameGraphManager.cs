using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200133C RID: 4924
public class GameGraphManager
{
	// Token: 0x06009B86 RID: 39814 RVA: 0x003AAE12 File Offset: 0x003A9012
	public static GameGraphManager Create(EntityPlayerLocal player)
	{
		GameGraphManager gameGraphManager = new GameGraphManager();
		gameGraphManager.player = player;
		gameGraphManager.Init();
		return gameGraphManager;
	}

	// Token: 0x06009B87 RID: 39815 RVA: 0x003AAE26 File Offset: 0x003A9026
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		if (!GameGraphManager.whiteTex)
		{
			GameGraphManager.whiteTex = new Texture2D(1, 1);
			GameGraphManager.whiteTex.FillTexture(Color.white, true, false);
		}
	}

	// Token: 0x06009B88 RID: 39816 RVA: 0x003AAE51 File Offset: 0x003A9051
	public void Destroy()
	{
		if (GameGraphManager.whiteTex)
		{
			global::UnityEngine.Object.Destroy(GameGraphManager.whiteTex);
		}
	}

	// Token: 0x06009B89 RID: 39817 RVA: 0x003AAE6C File Offset: 0x003A906C
	public void Add(string name, GameGraphManager.Graph.Callback callback, int sampleCount, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (sampleCount > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, sampleCount, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.callback = callback;
		}
	}

	// Token: 0x06009B8A RID: 39818 RVA: 0x003AAEB8 File Offset: 0x003A90B8
	public void AddCVar(string name, int count, string cvarName, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.cvarName = cvarName;
		}
	}

	// Token: 0x06009B8B RID: 39819 RVA: 0x003AAF04 File Offset: 0x003A9104
	public void AddPassiveEffect(string name, int count, PassiveEffects passiveEffect, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.passiveEffect = passiveEffect;
		}
	}

	// Token: 0x06009B8C RID: 39820 RVA: 0x003AAF50 File Offset: 0x003A9150
	public void AddStat(string name, int count, string statName, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.statName = statName.ToLower();
		}
	}

	// Token: 0x06009B8D RID: 39821 RVA: 0x003AAF9E File Offset: 0x003A919E
	public void RemoveAll()
	{
		this.graphs.Clear();
	}

	// Token: 0x06009B8E RID: 39822 RVA: 0x003AAFAC File Offset: 0x003A91AC
	public GameGraphManager.Graph FindGraph(string name)
	{
		for (int i = 0; i < this.graphs.Count; i++)
		{
			GameGraphManager.Graph graph = this.graphs[i];
			if (graph.name == name)
			{
				return graph;
			}
		}
		return null;
	}

	// Token: 0x06009B8F RID: 39823 RVA: 0x003AAFF0 File Offset: 0x003A91F0
	public void Draw()
	{
		bool flag = Event.current.type == EventType.Repaint;
		float num = 1f;
		for (int i = 0; i < this.graphs.Count; i++)
		{
			GameGraphManager.Graph graph = this.graphs[i];
			if (flag)
			{
				graph.UpdateValues();
			}
			graph.pos.x = 2f;
			graph.pos.y = num;
			graph.Draw();
			num += (float)(this.graphHeight + 2);
		}
	}

	// Token: 0x06009B90 RID: 39824 RVA: 0x003AB06B File Offset: 0x003A926B
	public void SetHeight(int _height)
	{
		this.graphHeight = _height;
		this.graphHeight = Mathf.Clamp(this.graphHeight, 1, 2100);
	}

	// Token: 0x04007540 RID: 30016
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D whiteTex;

	// Token: 0x04007541 RID: 30017
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04007542 RID: 30018
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameGraphManager.Graph> graphs = new List<GameGraphManager.Graph>();

	// Token: 0x04007543 RID: 30019
	[PublicizedFrom(EAccessModifier.Private)]
	public int graphHeight = 100;

	// Token: 0x0200133D RID: 4925
	public class Graph
	{
		// Token: 0x06009B92 RID: 39826 RVA: 0x003AB0A8 File Offset: 0x003A92A8
		public Graph(GameGraphManager _manager, string _name, int _count, float _maxValue, float _markerValue)
		{
			this.manager = _manager;
			this.name = _name;
			this.count = _count;
			this.count = Mathf.Clamp(this.count, 1, 4096);
			this.maxValue = _maxValue;
			this.markerValue = _markerValue;
			this.values = new float[this.count];
		}

		// Token: 0x06009B93 RID: 39827 RVA: 0x003AB108 File Offset: 0x003A9308
		public void AddValue(float value)
		{
			this.index = (this.index + 1) % this.count;
			this.values[this.index] = value;
		}

		// Token: 0x06009B94 RID: 39828 RVA: 0x003AB130 File Offset: 0x003A9330
		public void Draw()
		{
			Texture whiteTex = GameGraphManager.whiteTex;
			float num = (float)this.count * 2f + 2f;
			int graphHeight = this.manager.graphHeight;
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(this.pos.x, this.pos.y, num, (float)(graphHeight + 2)), whiteTex, ScaleMode.StretchToFill, false, 0f, new Color(0f, 0f, 0f, 0.9f), 0f, 0f);
			int num2 = this.index + 1;
			for (int i = 0; i < this.count; i++)
			{
				float num3 = this.values[num2 % this.count];
				num3 /= this.maxValue;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				float num4 = (float)graphHeight * num3;
				num4 = (float)((int)(num4 + 0.5f));
				Color color = new Color(1f, num3, num3 * 0.6f + 0.4f);
				GUI.DrawTexture(new Rect(this.pos.x + 1f + (float)i * 2f, this.pos.y + 1f + (float)graphHeight - num4, 2f, num4), whiteTex, ScaleMode.StretchToFill, false, 0f, color, 0f, 0f);
				num2++;
			}
			if (this.markerValue > 0f)
			{
				GUI.DrawTexture(new Rect(this.pos.x, this.pos.y + (float)graphHeight - this.markerValue / this.maxValue * (float)graphHeight, num, 1f), whiteTex, ScaleMode.StretchToFill, true, 0f, new Color(1f, 1f, 0f, 0.6f), 0f, 0f);
			}
			GUI.color = new Color(0.6f, 0.6f, 1f);
			GUI.Label(new Rect(this.pos.x + 1f, this.pos.y + 1f, 256f, 256f), string.Format("{0} {1}", this.name, this.values[this.index]));
		}

		// Token: 0x06009B95 RID: 39829 RVA: 0x003AB37C File Offset: 0x003A957C
		public void UpdateValues()
		{
			if (GameManager.Instance.World == null)
			{
				return;
			}
			EntityPlayerLocal player = this.manager.player;
			if (this.callback != null)
			{
				float num = this.values[this.index];
				if (this.callback(ref num))
				{
					this.AddValue(num);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.cvarName))
			{
				float cvar = player.GetCVar(this.cvarName);
				if (cvar != this.values[this.index])
				{
					this.AddValue(cvar);
					return;
				}
			}
			else if (this.passiveEffect != PassiveEffects.None)
			{
				float value = EffectManager.GetValue(this.passiveEffect, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				if (value != this.values[this.index])
				{
					this.AddValue(value);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.statName))
			{
				float num2 = 0f;
				string text = this.statName;
				if (!(text == "health"))
				{
					if (!(text == "stamina"))
					{
						if (!(text == "coretemp"))
						{
							if (text == "water")
							{
								num2 = player.PlayerStats.Water.Value;
							}
						}
						else
						{
							num2 = player.PlayerStats.CoreTemp;
						}
					}
					else
					{
						num2 = player.PlayerStats.Stamina.Value;
					}
				}
				else
				{
					num2 = player.PlayerStats.Health.Value;
				}
				if (num2 != this.values[this.index])
				{
					this.AddValue(num2);
				}
			}
		}

		// Token: 0x04007544 RID: 30020
		public string name;

		// Token: 0x04007545 RID: 30021
		public GameGraphManager.Graph.Callback callback;

		// Token: 0x04007546 RID: 30022
		public string cvarName;

		// Token: 0x04007547 RID: 30023
		public PassiveEffects passiveEffect;

		// Token: 0x04007548 RID: 30024
		public string statName;

		// Token: 0x04007549 RID: 30025
		public Vector2 pos;

		// Token: 0x0400754A RID: 30026
		[PublicizedFrom(EAccessModifier.Private)]
		public GameGraphManager manager;

		// Token: 0x0400754B RID: 30027
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x0400754C RID: 30028
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxValue;

		// Token: 0x0400754D RID: 30029
		[PublicizedFrom(EAccessModifier.Private)]
		public float markerValue;

		// Token: 0x0400754E RID: 30030
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] values;

		// Token: 0x0400754F RID: 30031
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x0200133E RID: 4926
		// (Invoke) Token: 0x06009B97 RID: 39831
		public delegate bool Callback(ref float value);
	}
}
