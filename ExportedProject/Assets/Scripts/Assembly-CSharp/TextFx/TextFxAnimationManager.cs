using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class TextFxAnimationManager
	{
		public enum PRESET_ANIMATION_SECTION
		{
			NONE = -1,
			INTRO = 0,
			MAIN = 1,
			OUTRO = 2
		}

		[Serializable]
		public class PresetAnimationSection
		{
			public List<PresetEffectSetting> m_preset_effect_settings;

			public bool m_active;

			public int m_start_action;

			public int m_num_actions;

			public int m_start_loop;

			public int m_num_loops;

			public bool m_exit_pause;

			public float m_exit_pause_duration = 1f;

			public bool m_repeat;

			public int m_repeat_count;

			public int ExitPauseIndex
			{
				get
				{
					return m_start_action + m_num_actions;
				}
			}

			public void SetExitPauseState(TextFxAnimationManager animationManager, bool state)
			{
				m_exit_pause = state;
				LetterAction action = animationManager.m_master_animations[0].GetAction(ExitPauseIndex);
				action.m_duration_progression.SetConstant((!m_exit_pause) ? 1E-06f : m_exit_pause_duration);
				animationManager.PrepareAnimationData(ANIMATION_DATA_TYPE.DURATION);
				animationManager.UpdatePresetAnimSectionActionIndexes();
			}

			public void SetExitPauseDuration(TextFxAnimationManager animationManager, float duration)
			{
				m_exit_pause_duration = duration;
				LetterAction action = animationManager.m_master_animations[0].GetAction(ExitPauseIndex);
				action.m_duration_progression.SetConstant(duration);
				animationManager.PrepareAnimationData(ANIMATION_DATA_TYPE.DURATION);
			}

			public void Reset()
			{
				m_preset_effect_settings = new List<PresetEffectSetting>();
				m_start_action = 0;
				m_num_actions = 0;
				m_start_loop = 0;
				m_num_loops = 0;
				m_exit_pause = false;
				m_exit_pause_duration = 1f;
				m_repeat = false;
				m_repeat_count = 0;
				m_active = false;
			}
		}

		public interface GuiTextDataHandler
		{
			int NumVerts { get; }

			int NumVertsPerLetter { get; }

			int ExtraVertsPerLetter { get; }

			Vector3[] GetLetterBaseVerts(int letterIndex);

			Color[] GetLetterBaseCols(int letterIndex);

			Vector3[] GetLetterExtraVerts(int letterIndex);

			Color[] GetLetterExtraCols(int letterIndex);
		}

		public static string[] m_animation_section_names = new string[3] { "Intro", "Main", "Outro" };

		public static string[] m_animation_section_folders = new string[3] { "Intros", "Mains", "Outros" };

		public const float INACTIVE_EXIT_PAUSE_DURATION = 1E-06f;

		public const string ANIM_INTROS_FOLDER_NAME = "Intros";

		public const string ANIM_MAINS_FOLDER_NAME = "Mains";

		public const string ANIM_OUTROS_FOLDER_NAME = "Outros";

		private const int JSON_EXPORTER_VERSION = 1;

		public List<LetterAnimation> m_master_animations;

		public bool m_begin_on_start;

		public ON_FINISH_ACTION m_on_finish_action;

		public float m_animation_speed_factor = 1f;

		public float m_begin_delay;

		public AnimatePerOptions m_animate_per;

		public AnimationTime m_time_type;

		[SerializeField]
		private LetterSetup[] m_letters;

		[SerializeField]
		private List<AudioSource> m_audio_sources;

		[SerializeField]
		private List<ParticleSystem> m_particle_systems;

		[SerializeField]
		private List<ParticleEffectInstanceManager> m_particle_effect_managers;

		[SerializeField]
		private GameObject m_gameObect;

		[SerializeField]
		private Transform m_transform;

		[SerializeField]
		private TextFxAnimationInterface m_animation_interface_reference;

		[SerializeField]
		private MonoBehaviour m_monobehaviour;

		[SerializeField]
		private int m_num_letters;

		[SerializeField]
		private int m_num_extra_verts_per_letter;

		[SerializeField]
		private List<float> m_textWidths;

		[SerializeField]
		private List<float> m_textHeights;

		[SerializeField]
		private Vector3[] m_current_mesh_verts;

		[SerializeField]
		private Color[] m_current_mesh_colours;

		[SerializeField]
		private string m_current_text = string.Empty;

		[SerializeField]
		private int m_num_words;

		[SerializeField]
		private int m_num_lines;

		private List<int> m_letters_not_animated;

		private float m_last_time;

		private float m_animation_timer;

		private int m_lowest_action_progress;

		private float m_runtime_animation_speed_factor = 1f;

		private bool m_running;

		private bool m_paused;

		private Action m_animation_callback;

		private ANIMATION_DATA_TYPE m_what_just_changed;

		private Action<int> m_animation_continue_callback;

		private int m_dataRebuildCallFrame = -1;

		public PresetAnimationSection m_preset_intro;

		public PresetAnimationSection m_preset_main;

		public PresetAnimationSection m_preset_outro;

		public bool m_repeat_all_sections;

		public int m_repeat_all_sections_count;

		private bool m_curveDataApplied;

		private bool all_letter_anims_finished = true;

		private bool all_letter_anims_waiting;

		private bool all_letter_anims_waiting_infinitely;

		private bool all_letter_anims_continuing_finished;

		private int lowest_action_progress = -1;

		private int last_letter_idx;

		private LetterSetup letter_setup;

		private LetterAnimation letterAnimation;

		private Vector3[] letter_verts;

		private Color[] letter_colours;

		public LetterSetup[] Letters
		{
			get
			{
				return m_letters;
			}
		}

		public string CurrentText
		{
			get
			{
				return m_current_text;
			}
		}

		public int IntroRepeatLoopStartIndex
		{
			get
			{
				return m_preset_outro.m_start_loop + m_preset_outro.m_num_loops;
			}
		}

		public int MainRepeatLoopStartIndex
		{
			get
			{
				return IntroRepeatLoopStartIndex + ((m_preset_intro.m_active && m_preset_intro.m_repeat) ? 1 : 0);
			}
		}

		public int OutroRepeatLoopStartIndex
		{
			get
			{
				return MainRepeatLoopStartIndex + ((m_preset_main.m_active && m_preset_main.m_repeat) ? 1 : 0);
			}
		}

		public int GlobalRepeatLoopStartIndex
		{
			get
			{
				return OutroRepeatLoopStartIndex + ((m_preset_outro.m_active && m_preset_outro.m_repeat) ? 1 : 0);
			}
		}

		public TextFxAnimationInterface AnimationInterface
		{
			get
			{
				return m_animation_interface_reference;
			}
		}

		public float MovementScale
		{
			get
			{
				return (m_animation_interface_reference == null) ? 1f : m_animation_interface_reference.MovementScale;
			}
		}

		public Transform Transform
		{
			get
			{
				return m_transform;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return m_gameObect;
			}
		}

		public Vector3[] MeshVerts
		{
			get
			{
				return m_current_mesh_verts;
			}
		}

		public Color[] MeshColours
		{
			get
			{
				return m_current_mesh_colours;
			}
		}

		public int NumAnimations
		{
			get
			{
				return (m_master_animations != null) ? m_master_animations.Count : 0;
			}
		}

		public ANIMATION_DATA_TYPE WhatJustChanged
		{
			get
			{
				return m_what_just_changed;
			}
			set
			{
				m_what_just_changed = value;
			}
		}

		public List<LetterAnimation> LetterAnimations
		{
			get
			{
				if (m_master_animations == null)
				{
					m_master_animations = new List<LetterAnimation>();
				}
				return m_master_animations;
			}
		}

		public bool HasAudioParticleChildInstances
		{
			get
			{
				return m_audio_sources != null && m_particle_systems != null && (m_audio_sources.Count > 0 || m_particle_systems.Count > 0);
			}
		}

		public List<ParticleEffectInstanceManager> ParticleEffectManagers
		{
			get
			{
				return m_particle_effect_managers;
			}
		}

		public int NumVertsPerLetter
		{
			get
			{
				return 4 + m_num_extra_verts_per_letter;
			}
		}

		public Vector3 Position
		{
			get
			{
				return m_transform.position;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return m_transform.lossyScale;
			}
		}

		public float AnimationTimer
		{
			get
			{
				return m_animation_timer;
			}
		}

		public bool Playing
		{
			get
			{
				return m_running;
			}
		}

		public bool Paused
		{
			get
			{
				return m_paused;
			}
			set
			{
				m_paused = value;
				PauseAllParticleEffects(m_paused);
			}
		}

		public bool UsingBezierCurve
		{
			get
			{
				return m_animation_interface_reference != null && m_animation_interface_reference.RenderToCurve;
			}
		}

		public float TextWidth(int lineIndex)
		{
			return m_textWidths[lineIndex];
		}

		public float TextHeight(int lineIndex)
		{
			return m_textHeights[lineIndex];
		}

		public float TextWidthScaled(int lineIndex)
		{
			return m_textWidths[lineIndex] / MovementScale;
		}

		public float TextHeightScaled(int lineIndex)
		{
			return m_textHeights[lineIndex] / MovementScale;
		}

		public void SetDataRebuildCallFrame()
		{
			m_dataRebuildCallFrame = Time.frameCount;
		}

		private void Init(MonoBehaviour monoInstance)
		{
			if (m_master_animations == null)
			{
				m_master_animations = new List<LetterAnimation>();
			}
		}

		public void OnStart()
		{
			if (m_master_animations != null && m_master_animations.Count > 0)
			{
				SetAnimationState(0, 0f, false, ANIMATION_DATA_TYPE.NONE, true);
			}
			if (m_begin_on_start)
			{
				PlayAnimation(m_begin_delay);
			}
		}

		public void SetParentObjectReferences(GameObject gameObject, Transform transform, TextFxAnimationInterface anim_interface)
		{
			m_gameObect = gameObject;
			m_transform = transform;
			m_animation_interface_reference = anim_interface;
			m_monobehaviour = (MonoBehaviour)anim_interface;
			Init(m_monobehaviour);
		}

		public void UpdatePresetAnimSectionActionIndexes()
		{
			int num = 0;
			int num2 = 0;
			m_preset_intro.m_start_action = 0;
			if (m_preset_intro.m_active)
			{
				num += m_preset_intro.m_num_actions + 1;
			}
			m_preset_main.m_start_action = num;
			if (m_preset_main.m_active)
			{
				num += m_preset_main.m_num_actions + 1;
			}
			m_preset_outro.m_start_action = num;
			m_preset_intro.m_start_loop = 0;
			num2 += m_preset_intro.m_num_loops;
			m_preset_main.m_start_loop = num2;
			num2 += m_preset_main.m_num_loops;
			m_preset_outro.m_start_loop = num2;
		}

		private PresetAnimationSection GetPresetAnimationSection(PRESET_ANIMATION_SECTION section)
		{
			switch (section)
			{
			case PRESET_ANIMATION_SECTION.INTRO:
				return m_preset_intro;
			case PRESET_ANIMATION_SECTION.MAIN:
				return m_preset_main;
			case PRESET_ANIMATION_SECTION.OUTRO:
				return m_preset_outro;
			default:
				return null;
			}
		}

		public void SetQuickSetupSection(PRESET_ANIMATION_SECTION section, string animationName = "None", bool setEditorSectionIndex = false, bool forceClearOldAudioParticles = true)
		{
			PresetAnimationSection presetAnimationSection = GetPresetAnimationSection(section);
			if (presetAnimationSection.m_num_actions > 0 && m_master_animations != null && m_master_animations.Count > 0 && m_master_animations[0].NumActions >= presetAnimationSection.m_num_actions + 1)
			{
				m_master_animations[0].RemoveActions(presetAnimationSection.m_start_action, presetAnimationSection.m_num_actions + 1);
				m_master_animations[0].RemoveLoops(presetAnimationSection.m_start_loop, presetAnimationSection.m_num_loops);
			}
			if (animationName != "None")
			{
				presetAnimationSection.m_repeat = false;
				presetAnimationSection.m_repeat_count = 0;
				string config = TextFxQuickSetupAnimConfigs.GetConfig(section, animationName);
				if (config != string.Empty)
				{
					presetAnimationSection.m_active = true;
					ImportData(config, presetAnimationSection, section, forceClearOldAudioParticles);
					LetterAction letterAction = new LetterAction();
					letterAction.m_action_type = ACTION_TYPE.BREAK;
					letterAction.m_duration_progression.SetConstant((!presetAnimationSection.m_exit_pause) ? 1E-06f : presetAnimationSection.m_exit_pause_duration);
					m_master_animations[0].InsertAction(presetAnimationSection.ExitPauseIndex, letterAction);
					PrepareAnimationData(ANIMATION_DATA_TYPE.DURATION);
				}
			}
			else
			{
				presetAnimationSection.Reset();
				PrepareAnimationData();
				ResetAnimation();
				m_animation_interface_reference.UpdateTextFxMesh();
			}
			UpdatePresetAnimSectionActionIndexes();
			if (m_repeat_all_sections)
			{
				ActionLoopCycle actionLoopCycle = m_master_animations[0].GetLoop(GlobalRepeatLoopStartIndex);
				if (actionLoopCycle == null)
				{
					actionLoopCycle = new ActionLoopCycle();
					actionLoopCycle.m_number_of_loops = m_repeat_all_sections_count;
					m_master_animations[0].InsertLoop(GlobalRepeatLoopStartIndex, actionLoopCycle, true);
				}
				actionLoopCycle.m_start_action_idx = 0;
				actionLoopCycle.m_end_action_idx = m_preset_outro.m_start_action + ((!m_preset_outro.m_active) ? (-1) : m_preset_outro.m_num_actions);
			}
		}

		public string GetRawTextString(string text)
		{
			string text2 = text.Replace("\n", string.Empty);
			text2 = text.Replace("\r", string.Empty);
			text2 = text.Replace("\t", string.Empty);
			return text.Replace(" ", string.Empty);
		}

		public void UpdateText(string text_string, GuiTextDataHandler textData, bool white_space_meshes)
		{
			List<LetterSetup> list = new List<LetterSetup>();
			char c = '\n';
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			bool flag2 = true;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			m_num_letters = ((textData.NumVertsPerLetter > 0) ? (textData.NumVerts / textData.NumVertsPerLetter) : 0);
			m_num_extra_verts_per_letter = textData.ExtraVertsPerLetter;
			if (m_textWidths == null)
			{
				m_textWidths = new List<float>();
				m_textHeights = new List<float>();
			}
			else
			{
				m_textWidths.Clear();
				m_textHeights.Clear();
			}
			LetterSetup cloneFrom = null;
			float item;
			for (int i = 0; i < text_string.Length; i++)
			{
				c = text_string[i];
				bool flag3 = !c.Equals(' ') && !c.Equals('\n') && !c.Equals('\r') && !c.Equals('\t');
				if (c.Equals('\n') || c.Equals('\r'))
				{
					item = num5 - num4;
					m_textWidths.Add(item);
					m_textHeights.Add(num7 - num6);
					num3++;
					flag2 = true;
				}
				if (flag && !flag3)
				{
					num2++;
				}
				flag = flag3;
				LetterSetup letterSetup;
				if (m_letters != null && i < m_letters.Length && !m_letters[i].StubInstance && m_letters[i].VisibleCharacter)
				{
					letterSetup = m_letters[i];
					if (flag3)
					{
						cloneFrom = letterSetup;
					}
				}
				else
				{
					letterSetup = new LetterSetup(this, cloneFrom);
				}
				letterSetup.SetWordLineIndex(num2, num3);
				if (!white_space_meshes && !flag3)
				{
					letterSetup.SetAsStubInstance();
				}
				else
				{
					if (num >= m_num_letters)
					{
						break;
					}
					letterSetup.SetLetterData(textData.GetLetterBaseVerts(num), textData.GetLetterBaseCols(num), textData.GetLetterExtraVerts(num), textData.GetLetterExtraCols(num), num);
					if (flag3)
					{
						if (flag2 || letterSetup.BaseVerticesBL.x < num4)
						{
							num4 = letterSetup.BaseVerticesBL.x;
						}
						if (flag2 || letterSetup.BaseVerticesBR.x > num5)
						{
							num5 = letterSetup.BaseVerticesBR.x;
						}
						if (flag2 || letterSetup.BaseVerticesBL.y < num6)
						{
							num6 = letterSetup.BaseVerticesBL.y;
						}
						if (flag2 || letterSetup.BaseVerticesTL.y > num7)
						{
							num7 = letterSetup.BaseVerticesTL.y;
						}
						flag2 = false;
					}
					num++;
				}
				letterSetup.VisibleCharacter = flag3;
				list.Add(letterSetup);
			}
			item = num5 - num4;
			m_textWidths.Add(item);
			m_textHeights.Add(num7 - num6);
			m_num_words = num2 + (flag ? 1 : 0);
			m_num_lines = num3 + ((c != '\n' && c != '\r') ? 1 : 0);
			int num8 = ((m_letters != null) ? m_letters.Length : 0);
			m_letters = list.ToArray();
			if (m_current_text != text_string || num8 != m_letters.Length)
			{
				PrepareAnimationData();
			}
			else
			{
				PrepareAnimationDefaultColour();
			}
			PopulateDefaultMeshData(true);
			m_current_text = text_string;
		}

		public bool CheckCurveData()
		{
			if (UsingBezierCurve)
			{
				TextFxBezierCurve bezierCurve = m_animation_interface_reference.BezierCurve;
				float[] letterProgressions = bezierCurve.GetLetterProgressions(this, ref m_letters, m_animation_interface_reference.TextAlignment);
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				for (int i = 0; i < m_letters.Length; i++)
				{
					LetterSetup letterSetup = m_letters[i];
					if (!letterSetup.StubInstance)
					{
						zero = Vector3.zero;
						zero2 = Vector3.zero;
						if (letterProgressions != null)
						{
							float progress = letterProgressions[i];
							float yOffset = (bezierCurve.m_baselineOffset + letterSetup.BaseVerticesCenter.y) * (1f / m_animation_interface_reference.MovementScale);
							zero = bezierCurve.GetCurvePoint(progress, 4, -1, yOffset);
							zero2 = bezierCurve.GetCurvePointRotation(letterProgressions[i]);
							letterSetup.OffsetLetterData(zero * m_animation_interface_reference.MovementScale, zero2);
						}
					}
				}
				m_curveDataApplied = true;
				return true;
			}
			if (m_curveDataApplied)
			{
				ClearCurveData();
			}
			return false;
		}

		public void ClearCurveData()
		{
			if (m_curveDataApplied)
			{
				for (int i = 0; i < m_letters.Length; i++)
				{
					m_letters[i].ClearOffsetData();
				}
				m_curveDataApplied = false;
			}
		}

		public void PrepareAnimationData(ANIMATION_DATA_TYPE what_to_update = ANIMATION_DATA_TYPE.ALL)
		{
			if (m_master_animations == null)
			{
				return;
			}
			if (m_letters_not_animated == null)
			{
				m_letters_not_animated = new List<int>();
			}
			m_letters_not_animated.Clear();
			for (int i = 0; i < m_letters.Length; i++)
			{
				if (m_letters[i].VisibleCharacter)
				{
					m_letters_not_animated.Add(i);
				}
			}
			foreach (LetterAnimation master_animation in m_master_animations)
			{
				master_animation.PrepareData(this, m_letters, what_to_update, m_num_words, m_num_lines, m_animate_per);
				for (int j = 0; j < master_animation.m_letters_to_animate.Count; j++)
				{
					m_letters_not_animated.Remove(master_animation.m_letters_to_animate[j]);
				}
			}
			if (m_letters_not_animated != null && m_letters_not_animated.Count > 0)
			{
				PopulateDefaultMeshData();
				foreach (int item in m_letters_not_animated)
				{
					LetterSetup letterSetup = m_letters[item];
					Vector3[] mesh_verts = new Vector3[NumVertsPerLetter];
					for (int k = 0; k < NumVertsPerLetter; k++)
					{
						mesh_verts[k] = m_current_mesh_verts[(k >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letterSetup.MeshIndex * 4 + (k - m_num_extra_verts_per_letter)) : (letterSetup.MeshIndex * m_num_extra_verts_per_letter + k)];
					}
					Color[] mesh_colours = new Color[NumVertsPerLetter];
					for (int l = 0; l < NumVertsPerLetter; l++)
					{
						mesh_colours[l] = m_current_mesh_colours[(l >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letterSetup.MeshIndex * 4 + (l - m_num_extra_verts_per_letter)) : (letterSetup.MeshIndex * m_num_extra_verts_per_letter + l)];
					}
					m_letters[item].SetMeshState(this, -1, 0f, null, m_animate_per, ref mesh_verts, ref mesh_colours);
					for (int m = 0; m < NumVertsPerLetter; m++)
					{
						m_current_mesh_verts[(m >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letterSetup.MeshIndex * 4 + (m - m_num_extra_verts_per_letter)) : (letterSetup.MeshIndex * m_num_extra_verts_per_letter + m)] = mesh_verts[m];
					}
					for (int n = 0; n < NumVertsPerLetter; n++)
					{
						int num = ((n >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letterSetup.MeshIndex * 4 + (n - m_num_extra_verts_per_letter)) : (letterSetup.MeshIndex * m_num_extra_verts_per_letter + n));
						m_current_mesh_colours[num] = mesh_colours[n];
					}
				}
			}
			if (Playing)
			{
				m_what_just_changed = what_to_update;
			}
		}

		private void PrepareAnimationDefaultColour()
		{
			if (m_master_animations == null)
			{
				return;
			}
			foreach (LetterAnimation master_animation in m_master_animations)
			{
				master_animation.RefreshDefaultTextColour(m_letters);
			}
		}

		public void PlayAnimation(Action animation_callback)
		{
			m_animation_callback = animation_callback;
			PlayAnimation();
		}

		public void PlayAnimation(float delay, Action animation_callback)
		{
			m_animation_callback = animation_callback;
			PlayAnimation(delay);
		}

		public void PlayAnimation(float delay = 0f, int starting_action_index = 0)
		{
			if (m_master_animations == null || m_master_animations.Count == 0)
			{
				Debug.LogWarning("PlayAnimation() called on '" + m_gameObect.name + "', but no animation defined.");
			}
			else
			{
				if (m_gameObect == null)
				{
					return;
				}
				if (!m_gameObect.activeSelf)
				{
					Debug.LogWarning("PlayAnimation() called on '" + m_gameObect.name + "', but the gameObject is inactive.");
					return;
				}
				int num = m_letters.Length;
				m_audio_sources = new List<AudioSource>(m_gameObect.GetComponentsInChildren<AudioSource>());
				m_particle_systems = new List<ParticleSystem>(m_gameObect.GetComponentsInChildren<ParticleSystem>());
				m_particle_effect_managers = new List<ParticleEffectInstanceManager>();
				foreach (AudioSource audio_source in m_audio_sources)
				{
					audio_source.Stop();
				}
				foreach (ParticleSystem particle_system in m_particle_systems)
				{
					particle_system.Stop();
					particle_system.Clear();
				}
				bool flag = false;
				foreach (LetterAnimation master_animation in m_master_animations)
				{
					master_animation.CurrentAnimationState = LETTER_ANIMATION_STATE.PLAYING;
					foreach (int item in master_animation.m_letters_to_animate)
					{
						if (item < num)
						{
							m_letters[item].Reset(master_animation, starting_action_index);
							m_letters[item].Active = true;
						}
					}
					if (flag || master_animation.NumActions <= 0 || master_animation.GetAction(starting_action_index).NumParticleEffectSetups <= 0)
					{
						continue;
					}
					foreach (ParticleEffectSetup particleEffectSetup in master_animation.GetAction(starting_action_index).ParticleEffectSetups)
					{
						if (particleEffectSetup.m_play_when == PLAY_ITEM_EVENTS.ON_START)
						{
							UpdateMesh(false, true, starting_action_index);
							flag = true;
						}
					}
				}
				m_lowest_action_progress = 0;
				m_animation_timer = 0f;
				m_runtime_animation_speed_factor = 1f;
				m_animation_continue_callback = null;
				if (m_dataRebuildCallFrame > -1)
				{
					if (m_dataRebuildCallFrame == Time.frameCount)
					{
						delay = 1E-05f;
					}
					m_dataRebuildCallFrame = -1;
				}
				if (delay > 0f)
				{
					m_monobehaviour.StartCoroutine(PlayAnimationAfterDelay(delay));
					return;
				}
				if (m_time_type == AnimationTime.REAL_TIME || !Application.isPlaying)
				{
					m_last_time = Time.realtimeSinceStartup;
				}
				else
				{
					m_last_time = Time.time;
				}
				m_running = true;
				m_paused = false;
			}
		}

		private IEnumerator PlayAnimationAfterDelay(float delay)
		{
			yield return m_monobehaviour.StartCoroutine(TimeDelay(delay, m_time_type));
			if (m_time_type == AnimationTime.REAL_TIME || !Application.isPlaying)
			{
				m_last_time = Time.realtimeSinceStartup;
			}
			else
			{
				m_last_time = Time.time;
			}
			m_running = true;
			m_paused = false;
		}

		public void ResetAnimation()
		{
			UpdateMesh(false, true);
			LetterSetup[] letters = m_letters;
			foreach (LetterSetup letterSetup in letters)
			{
				letterSetup.AnimStateVars.Reset();
			}
			m_running = false;
			m_paused = false;
			m_lowest_action_progress = 0;
			m_animation_timer = 0f;
			m_runtime_animation_speed_factor = 1f;
			m_animation_continue_callback = null;
			StopAllParticleEffects(true);
			if (m_animation_interface_reference != null)
			{
				m_animation_interface_reference.UpdateTextFxMesh();
			}
		}

		public void SetEndState()
		{
			int num = 0;
			int index = 0;
			for (int i = 0; i < m_master_animations.Count; i++)
			{
				if (m_master_animations[i].NumActions > num)
				{
					num = m_master_animations[i].NumActions;
					index = i;
				}
			}
			int action_idx = 0;
			LetterAnimation letterAnimation = m_master_animations[index];
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				if (letterAnimation.GetAction(num2).m_action_type == ACTION_TYPE.ANIM_SEQUENCE)
				{
					action_idx = num2;
					break;
				}
			}
			SetAnimationState(action_idx, 1f);
			m_running = false;
			m_paused = false;
			m_lowest_action_progress = 0;
			m_animation_timer = 0f;
			m_runtime_animation_speed_factor = 1f;
			m_animation_continue_callback = null;
			StopAllParticleEffects(true);
			m_animation_interface_reference.UpdateTextFxMesh();
		}

		public bool UpdateAnimation(float deltaTimeOverride = -1f)
		{
			float num = ((!(deltaTimeOverride > 0f)) ? ((m_time_type != 0 || !Application.isPlaying) ? (Time.realtimeSinceStartup - m_last_time) : (Time.time - m_last_time)) : deltaTimeOverride);
			if (num > 0.25f && deltaTimeOverride < 0f)
			{
				num = 1f / 30f;
			}
			if (m_time_type == AnimationTime.REAL_TIME || !Application.isPlaying)
			{
				m_last_time = Time.realtimeSinceStartup;
			}
			else
			{
				m_last_time = Time.time;
			}
			if (m_paused && deltaTimeOverride < 0f)
			{
				return m_running;
			}
			num *= m_runtime_animation_speed_factor * m_animation_speed_factor;
			m_animation_timer += num;
			if (m_running && UpdateMesh(true, false, 0, 0f, num))
			{
				m_running = false;
				if (m_animation_callback != null)
				{
					m_animation_callback();
				}
				if (Application.isPlaying)
				{
					if (m_on_finish_action == ON_FINISH_ACTION.DESTROY_OBJECT)
					{
						UnityEngine.Object.Destroy(m_gameObect);
					}
					else if (m_on_finish_action == ON_FINISH_ACTION.DISABLE_OBJECT)
					{
						m_gameObect.SetActive(false);
					}
					else if (m_on_finish_action == ON_FINISH_ACTION.RESET_ANIMATION)
					{
						ResetAnimation();
					}
				}
			}
			if (m_particle_effect_managers.Count > 0)
			{
				for (int i = 0; i < m_particle_effect_managers.Count; i++)
				{
					if (m_particle_effect_managers[i].Update(num))
					{
						m_particle_effect_managers.RemoveAt(i);
						i--;
					}
				}
			}
			return m_running;
		}

		public void SetAnimationState(int action_idx, float action_progress, bool update_action_values = false, ANIMATION_DATA_TYPE edited_data = ANIMATION_DATA_TYPE.ALL, bool update_mesh = false)
		{
			if (update_action_values)
			{
				PrepareAnimationData(edited_data);
			}
			UpdateMesh(false, true, action_idx, action_progress);
			if (update_mesh)
			{
				m_animation_interface_reference.UpdateTextFxMesh();
			}
		}

		public void ContinuePastBreak(bool onlyIfAllLettersWaiting = false)
		{
			ContinuePastBreak(0, onlyIfAllLettersWaiting);
		}

		public void ContinuePastBreak(int animationIndex, bool onlyIfAllLettersWaiting = false)
		{
			if (animationIndex < 0 || m_master_animations == null || animationIndex >= m_master_animations.Count)
			{
				return;
			}
			LetterAnimation letterAnimation = m_master_animations[animationIndex];
			if (onlyIfAllLettersWaiting && letterAnimation.CurrentAnimationState != LETTER_ANIMATION_STATE.WAITING && letterAnimation.CurrentAnimationState != LETTER_ANIMATION_STATE.WAITING_INFINITE)
			{
				return;
			}
			foreach (int item in letterAnimation.m_letters_to_animate)
			{
				if (m_letters[item].CurrentAnimationState == LETTER_ANIMATION_STATE.WAITING || m_letters[item].CurrentAnimationState == LETTER_ANIMATION_STATE.WAITING_INFINITE)
				{
					m_letters[item].ContinueAction(m_animation_timer, letterAnimation, m_animate_per);
				}
			}
		}

		public void ContinuePastLoop(ContinueType continueType = ContinueType.EndOfLoop, float lerpSyncDuration = 0.5f, bool passNextInfiniteLoop = true, bool trimInterimLoops = true, float animationSpeedOverride = 1f, Action<int> finishedCallback = null)
		{
			ContinuePastLoop(0, continueType, lerpSyncDuration, passNextInfiniteLoop, trimInterimLoops, animationSpeedOverride, finishedCallback);
		}

		public void ContinuePastLoop(int animation_index, ContinueType continueType = ContinueType.EndOfLoop, float lerpSyncDuration = 0.5f, bool passNextInfiniteLoop = true, bool trimInterimLoops = true, float animationSpeedOverride = 1f, Action<int> finishedCallback = null)
		{
			if (animation_index < 0 || m_master_animations == null || animation_index >= m_master_animations.Count)
			{
				return;
			}
			LetterAnimation letterAnimation = m_master_animations[animation_index];
			int num = -1;
			ActionLoopCycle actionLoopCycle = null;
			LetterSetup letterSetup = null;
			int num2 = -1;
			int num3 = -1;
			int[] array = new int[letterAnimation.ActionLoopCycles.Count];
			bool flag = true;
			foreach (int item in letterAnimation.m_letters_to_animate)
			{
				LetterSetup letterSetup2 = m_letters[item];
				if (num2 == -1 || letterSetup2.ActionIndex > num2)
				{
					num2 = letterSetup2.ActionIndex;
				}
				if (num3 == -1 || letterSetup2.ActionProgress > num3)
				{
					num3 = letterSetup2.ActionProgress;
				}
				if (letterSetup2.ActiveLoopCycles.Count > 0)
				{
					for (int i = 0; i < letterSetup2.ActiveLoopCycles.Count; i++)
					{
						ActionLoopCycle actionLoopCycle2 = letterSetup2.ActiveLoopCycles[i];
						if (array[actionLoopCycle2.m_active_loop_index] == 0 || actionLoopCycle2.m_number_of_loops < array[actionLoopCycle2.m_active_loop_index])
						{
							array[actionLoopCycle2.m_active_loop_index] = actionLoopCycle2.m_number_of_loops;
						}
					}
				}
				if (letterSetup2.ActiveLoopCycles.Count > 0 && (num == -1 || letterSetup2.ActiveLoopCycles.Count < num))
				{
					num = letterSetup2.ActiveLoopCycles.Count;
					actionLoopCycle = letterSetup2.ActiveLoopCycles[0];
					letterSetup = letterSetup2;
				}
				else if (letterSetup2.ActiveLoopCycles.Count == 0)
				{
					flag = false;
				}
			}
			int num4 = -1;
			int num5 = 0;
			if (actionLoopCycle == null)
			{
				return;
			}
			if (actionLoopCycle.m_end_action_idx + 1 < letterAnimation.LetterActions.Count)
			{
				int j;
				for (j = 1; letterAnimation.GetAction(actionLoopCycle.m_end_action_idx + j).m_action_type == ACTION_TYPE.BREAK && actionLoopCycle.m_end_action_idx + j + 1 < letterAnimation.LetterActions.Count; j++)
				{
				}
				if (letterAnimation.GetAction(actionLoopCycle.m_end_action_idx + j).m_action_type != ACTION_TYPE.BREAK)
				{
					num4 = actionLoopCycle.m_end_action_idx + j;
					num5 = 0;
				}
			}
			if (actionLoopCycle.m_end_action_idx + 1 == letterAnimation.LetterActions.Count || num4 == -1)
			{
				int num6 = ((actionLoopCycle.m_loop_type != 0 && !actionLoopCycle.m_finish_at_end) ? actionLoopCycle.m_start_action_idx : actionLoopCycle.m_end_action_idx);
				int num7 = ((actionLoopCycle.m_loop_type != 0 && !actionLoopCycle.m_finish_at_end) ? 1 : (-1));
				int k;
				for (k = 0; num6 + num7 * k >= actionLoopCycle.m_start_action_idx && num6 + num7 * k <= actionLoopCycle.m_end_action_idx && letterAnimation.GetAction(num6 + num7 * k).m_action_type == ACTION_TYPE.BREAK; k++)
				{
				}
				if (num6 + num7 * k >= actionLoopCycle.m_start_action_idx && num6 + num7 * k <= actionLoopCycle.m_end_action_idx)
				{
					num4 = num6 + num7 * k;
					num5 = ((actionLoopCycle.m_loop_type == LOOP_TYPE.LOOP || actionLoopCycle.m_finish_at_end) ? 1 : 0);
				}
			}
			if (!flag && num2 > ((actionLoopCycle.m_loop_type != 0 && !actionLoopCycle.m_finish_at_end) ? actionLoopCycle.m_start_action_idx : actionLoopCycle.m_end_action_idx))
			{
				num4 = num2;
				num5 = 1;
				num = 0;
			}
			else
			{
				for (int l = 1; l < letterSetup.ActiveLoopCycles.Count; l++)
				{
					if (num4 > letterSetup.ActiveLoopCycles[l].m_end_action_idx)
					{
						num--;
					}
				}
			}
			if (passNextInfiniteLoop)
			{
				bool flag2 = false;
				for (int m = 0; m < letterSetup.ActiveLoopCycles.Count - num + 1; m++)
				{
					if (letterSetup.ActiveLoopCycles[m].m_number_of_loops <= 0)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					for (int n = m_letters[0].ActiveLoopCycles.Count - num; n < m_letters[0].ActiveLoopCycles.Count; n++)
					{
						if (m_letters[0].ActiveLoopCycles[n].m_number_of_loops <= 0)
						{
							actionLoopCycle = m_letters[0].ActiveLoopCycles[n];
							num -= n - (m_letters[0].ActiveLoopCycles.Count - num);
							letterSetup = m_letters[0];
						}
					}
				}
			}
			foreach (int item2 in letterAnimation.m_letters_to_animate)
			{
				LetterSetup letterSetup2 = m_letters[item2];
				letterSetup2.ContinueFromCurrentToAction(letterAnimation, num4, num5 == 0, (actionLoopCycle != null) ? (actionLoopCycle.m_end_action_idx + 1) : num4, m_animate_per, m_animation_timer, lerpSyncDuration, num3, num, continueType, trimInterimLoops, array);
			}
			if (continueType == ContinueType.EndOfLoop)
			{
				m_runtime_animation_speed_factor = animationSpeedOverride;
			}
			if (finishedCallback != null)
			{
				m_animation_continue_callback = finishedCallback;
			}
			letterAnimation.CurrentAnimationState = LETTER_ANIMATION_STATE.CONTINUING;
		}

		public void PopulateDefaultMeshData(bool forcePopulate = false)
		{
			if (!forcePopulate && m_current_mesh_verts != null && m_current_mesh_verts.Length == m_num_letters * NumVertsPerLetter)
			{
				return;
			}
			m_current_mesh_verts = new Vector3[m_num_letters * NumVertsPerLetter];
			m_current_mesh_colours = new Color[m_num_letters * NumVertsPerLetter];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < m_letters.Length; i++)
			{
				LetterSetup letterSetup = m_letters[i];
				if (!letterSetup.StubInstance)
				{
					Vector3[] baseVertices = letterSetup.BaseVertices;
					Vector3[] baseExtraVertices = letterSetup.BaseExtraVertices;
					Color[] baseExtraColours = letterSetup.BaseExtraColours;
					num2 = num * m_num_extra_verts_per_letter;
					for (int j = 0; j < m_num_extra_verts_per_letter; j++)
					{
						m_current_mesh_verts[num2 + j] = baseExtraVertices[j];
					}
					for (int k = 0; k < m_num_extra_verts_per_letter; k++)
					{
						m_current_mesh_colours[num2 + k] = baseExtraColours[k];
					}
					num2 = m_num_letters * m_num_extra_verts_per_letter + num * 4;
					m_current_mesh_verts[num2] = baseVertices[0];
					m_current_mesh_verts[num2 + 1] = baseVertices[1];
					m_current_mesh_verts[num2 + 2] = baseVertices[2];
					m_current_mesh_verts[num2 + 3] = baseVertices[3];
					m_current_mesh_colours[num2] = letterSetup.BaseMeshColour(0);
					m_current_mesh_colours[num2 + 1] = letterSetup.BaseMeshColour(1);
					m_current_mesh_colours[num2 + 2] = letterSetup.BaseMeshColour(2);
					m_current_mesh_colours[num2 + 3] = letterSetup.BaseMeshColour(3);
					num++;
				}
			}
		}

		public bool UpdateMesh(bool use_timer, bool force_render, int action_idx = 0, float action_progress = 0f, float delta_time = 0f)
		{
			all_letter_anims_finished = true;
			PopulateDefaultMeshData();
			if (m_master_animations != null)
			{
				lowest_action_progress = -1;
				for (int i = 0; i < m_master_animations.Count; i++)
				{
					letterAnimation = m_master_animations[i];
					last_letter_idx = -1;
					all_letter_anims_waiting = true;
					all_letter_anims_waiting_infinitely = true;
					all_letter_anims_continuing_finished = true;
					if (letterAnimation.m_letters_to_animate == null)
					{
						letterAnimation.m_letters_to_animate = new List<int>();
					}
					for (int j = 0; j < letterAnimation.m_letters_to_animate.Count; j++)
					{
						int num = letterAnimation.m_letters_to_animate[j];
						if (num == last_letter_idx || num >= m_letters.Length)
						{
							continue;
						}
						letter_setup = m_letters[num];
						if (lowest_action_progress == -1 || letter_setup.ActionProgress < lowest_action_progress)
						{
							lowest_action_progress = letter_setup.ActionProgress;
						}
						if (letter_verts == null || letter_verts.Length != NumVertsPerLetter)
						{
							letter_verts = new Vector3[NumVertsPerLetter];
						}
						if (letter_colours == null || letter_colours.Length != NumVertsPerLetter)
						{
							letter_colours = new Color[NumVertsPerLetter];
						}
						for (int k = 0; k < NumVertsPerLetter; k++)
						{
							letter_verts[k] = m_current_mesh_verts[(k >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letter_setup.MeshIndex * 4 + (k - m_num_extra_verts_per_letter)) : (letter_setup.MeshIndex * m_num_extra_verts_per_letter + k)];
							letter_colours[k] = m_current_mesh_colours[(k >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letter_setup.MeshIndex * 4 + (k - m_num_extra_verts_per_letter)) : (letter_setup.MeshIndex * m_num_extra_verts_per_letter + k)];
						}
						if (use_timer)
						{
							letter_setup.AnimateMesh(this, force_render, m_animation_timer, m_lowest_action_progress, letterAnimation, m_animate_per, delta_time, ref letter_verts, ref letter_colours);
							LETTER_ANIMATION_STATE currentAnimationState = letter_setup.CurrentAnimationState;
							if (currentAnimationState != LETTER_ANIMATION_STATE.CONTINUING_FINISHED)
							{
								all_letter_anims_continuing_finished = false;
							}
							if (currentAnimationState == LETTER_ANIMATION_STATE.STOPPED)
							{
								lowest_action_progress = letter_setup.ActionProgress;
							}
							else
							{
								all_letter_anims_finished = false;
							}
							if (currentAnimationState != LETTER_ANIMATION_STATE.WAITING_INFINITE)
							{
								all_letter_anims_waiting_infinitely = false;
							}
							if (currentAnimationState != LETTER_ANIMATION_STATE.WAITING && currentAnimationState != LETTER_ANIMATION_STATE.WAITING_INFINITE)
							{
								all_letter_anims_waiting = false;
							}
						}
						else
						{
							letter_setup.SetMeshState(this, Mathf.Clamp(action_idx, 0, letterAnimation.NumActions - 1), action_progress, letterAnimation, m_animate_per, ref letter_verts, ref letter_colours);
						}
						for (int l = 0; l < NumVertsPerLetter; l++)
						{
							m_current_mesh_verts[(l >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letter_setup.MeshIndex * 4 + (l - m_num_extra_verts_per_letter)) : (letter_setup.MeshIndex * m_num_extra_verts_per_letter + l)] = letter_verts[l];
						}
						for (int m = 0; m < NumVertsPerLetter; m++)
						{
							int num2 = ((m >= m_num_extra_verts_per_letter) ? (m_num_letters * m_num_extra_verts_per_letter + letter_setup.MeshIndex * 4 + (m - m_num_extra_verts_per_letter)) : (letter_setup.MeshIndex * m_num_extra_verts_per_letter + m));
							m_current_mesh_colours[num2] = letter_colours[m];
						}
						last_letter_idx = num;
					}
					if (letterAnimation.m_letters_to_animate.Count > 0)
					{
						if (use_timer)
						{
							if (letterAnimation.CurrentAnimationState == LETTER_ANIMATION_STATE.CONTINUING)
							{
								if (all_letter_anims_continuing_finished)
								{
									letterAnimation.CurrentAnimationState = LETTER_ANIMATION_STATE.PLAYING;
									m_runtime_animation_speed_factor = 1f;
									for (int n = 0; n < letterAnimation.m_letters_to_animate.Count; n++)
									{
										int num = letterAnimation.m_letters_to_animate[n];
										m_letters[num].SetPlayingState();
									}
									m_lowest_action_progress = m_letters[0].ActionProgress;
									if (m_animation_continue_callback != null)
									{
										m_animation_continue_callback(m_letters[0].ActionIndex);
									}
									m_animation_continue_callback = null;
								}
							}
							else if (all_letter_anims_waiting_infinitely)
							{
								letterAnimation.CurrentAnimationState = LETTER_ANIMATION_STATE.WAITING_INFINITE;
							}
							else if (all_letter_anims_waiting)
							{
								letterAnimation.CurrentAnimationState = LETTER_ANIMATION_STATE.WAITING;
							}
							else if (!all_letter_anims_finished)
							{
								letterAnimation.CurrentAnimationState = LETTER_ANIMATION_STATE.PLAYING;
							}
						}
					}
					else
					{
						letterAnimation.CurrentAnimationState = LETTER_ANIMATION_STATE.STOPPED;
					}
					if (lowest_action_progress > m_lowest_action_progress)
					{
						m_lowest_action_progress = lowest_action_progress;
					}
				}
			}
			m_what_just_changed = ANIMATION_DATA_TYPE.NONE;
			return all_letter_anims_finished;
		}

		private void PauseAllParticleEffects(bool paused)
		{
			if (m_particle_effect_managers == null)
			{
				return;
			}
			foreach (ParticleEffectInstanceManager particle_effect_manager in m_particle_effect_managers)
			{
				particle_effect_manager.Pause(paused);
			}
		}

		private void StopAllParticleEffects(bool force_stop = false)
		{
			if (m_particle_effect_managers != null)
			{
				foreach (ParticleEffectInstanceManager particle_effect_manager in m_particle_effect_managers)
				{
					particle_effect_manager.Stop(force_stop);
				}
				m_particle_effect_managers = new List<ParticleEffectInstanceManager>();
			}
			if (m_particle_systems == null)
			{
				return;
			}
			foreach (ParticleSystem particle_system in m_particle_systems)
			{
				if (!(particle_system == null))
				{
					particle_system.Stop();
					particle_system.Clear();
				}
			}
		}

		public void ClearCachedAudioParticleInstances()
		{
			m_audio_sources = new List<AudioSource>(m_gameObect.GetComponentsInChildren<AudioSource>());
			m_particle_systems = new List<ParticleSystem>(m_gameObect.GetComponentsInChildren<ParticleSystem>());
			foreach (AudioSource audio_source in m_audio_sources)
			{
				if (audio_source != null && audio_source.gameObject != null)
				{
					UnityEngine.Object.DestroyImmediate(audio_source.gameObject);
				}
			}
			m_audio_sources = new List<AudioSource>();
			foreach (ParticleSystem particle_system in m_particle_systems)
			{
				if (particle_system != null && particle_system.gameObject != null)
				{
					UnityEngine.Object.DestroyImmediate(particle_system.gameObject);
				}
			}
			m_particle_systems = new List<ParticleSystem>();
			m_particle_effect_managers = new List<ParticleEffectInstanceManager>();
		}

		private AudioSource AddNewAudioChild()
		{
			GameObject gameObject = new GameObject("TextFx_AudioSource");
			gameObject.transform.parent = m_transform;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			if (m_audio_sources == null)
			{
				m_audio_sources = new List<AudioSource>();
			}
			m_audio_sources.Add(audioSource);
			return audioSource;
		}

		private void PlayClip(AudioSource a_source, AudioClip clip, float delay, float start_time, float volume, float pitch)
		{
			a_source.clip = clip;
			a_source.time = start_time;
			a_source.volume = volume;
			a_source.pitch = pitch;
			a_source.PlayDelayed(delay);
		}

		public void PlayAudioClip(AudioEffectSetup effect_setup, AnimationProgressionVariables progression_vars, AnimatePerOptions animate_per)
		{
			bool flag = false;
			AudioSource a_source = null;
			if (m_audio_sources != null)
			{
				foreach (AudioSource audio_source in m_audio_sources)
				{
					if (!audio_source.isPlaying)
					{
						a_source = audio_source;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					a_source = AddNewAudioChild();
				}
			}
			else
			{
				a_source = AddNewAudioChild();
			}
			PlayClip(a_source, effect_setup.m_audio_clip, effect_setup.m_delay.GetValue(progression_vars, animate_per), effect_setup.m_offset_time.GetValue(progression_vars, animate_per), effect_setup.m_volume.GetValue(progression_vars, animate_per), effect_setup.m_pitch.GetValue(progression_vars, animate_per));
		}

		public void PlayParticleEffect(LetterSetup letter_setup, ParticleEffectSetup effect_setup, AnimationProgressionVariables progression_vars, AnimatePerOptions animate_per)
		{
			bool flag = false;
			if (!(effect_setup.m_shuriken_particle_effect != null))
			{
				return;
			}
			if (m_particle_systems == null)
			{
				m_particle_systems = new List<ParticleSystem>();
			}
			foreach (ParticleSystem particle_system in m_particle_systems)
			{
				if (!particle_system.isPlaying && particle_system.particleCount == 0 && particle_system.name.Equals(effect_setup.m_shuriken_particle_effect.name + "(Clone)"))
				{
					m_particle_effect_managers.Add(new ParticleEffectInstanceManager(this, letter_setup, effect_setup, progression_vars, animate_per, particle_system));
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				ParticleSystem particleSystem = UnityEngine.Object.Instantiate(effect_setup.m_shuriken_particle_effect);
				m_particle_systems.Add(particleSystem);
				particleSystem.gameObject.SetActive(true);
				ParticleSystem.MainModule main = particleSystem.main;
				main.playOnAwake = false;
				particleSystem.Stop();
				particleSystem.transform.parent = m_transform;
				m_particle_effect_managers.Add(new ParticleEffectInstanceManager(this, letter_setup, effect_setup, progression_vars, animate_per, particleSystem));
			}
		}

		public LetterAnimation AddAnimation()
		{
			if (m_master_animations == null)
			{
				m_master_animations = new List<LetterAnimation>();
			}
			LetterAnimation letterAnimation = new LetterAnimation();
			m_master_animations.Add(letterAnimation);
			return letterAnimation;
		}

		public void RemoveAnimation(int index)
		{
			if (m_master_animations != null && index >= 0 && index < NumAnimations)
			{
				m_master_animations.RemoveAt(index);
			}
		}

		public LetterAnimation GetAnimation(int index)
		{
			if (m_master_animations != null && m_master_animations.Count > index && index >= 0)
			{
				return m_master_animations[index];
			}
			return null;
		}

		public LetterSetup GetLetter(int letterIdx)
		{
			if (m_letters != null && letterIdx < m_letters.Length)
			{
				return m_letters[letterIdx];
			}
			return null;
		}

		public Vector3 GetLetterPosition(int letter_idx, OBJ_POS position_requested = OBJ_POS.CENTER, TRANSFORM_SPACE transform_space = TRANSFORM_SPACE.WORLD)
		{
			if (m_letters == null || m_letters.Length == 0)
			{
				return Vector3.zero;
			}
			letter_idx = Mathf.Clamp(letter_idx, 0, m_letters.Length - 1);
			switch (position_requested)
			{
			case OBJ_POS.CENTER:
				return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].CenterLocal : m_letters[letter_idx].Center;
			case OBJ_POS.BOTTOM_LEFT:
				return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].BottomLeftLocal : m_letters[letter_idx].BottomLeft;
			case OBJ_POS.BOTTOM_RIGHT:
				return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].BottomRightLocal : m_letters[letter_idx].BottomRight;
			case OBJ_POS.TOP_LEFT:
				return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].TopLeftLocal : m_letters[letter_idx].TopLeft;
			case OBJ_POS.TOP_RIGHT:
				return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].TopRightLocal : m_letters[letter_idx].TopRight;
			default:
				return Vector3.zero;
			}
		}

		public Quaternion GetLetterRotation(int letter_idx, TRANSFORM_SPACE transform_space = TRANSFORM_SPACE.WORLD)
		{
			if (m_letters == null || m_letters.Length == 0)
			{
				return Quaternion.identity;
			}
			letter_idx = Mathf.Clamp(letter_idx, 0, m_letters.Length - 1);
			return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].RotationLocal : m_letters[letter_idx].Rotation;
		}

		public Vector3 GetLetterScale(int letter_idx, TRANSFORM_SPACE transform_space = TRANSFORM_SPACE.WORLD)
		{
			if (m_letters == null || m_letters.Length == 0)
			{
				return Vector3.one;
			}
			letter_idx = Mathf.Clamp(letter_idx, 0, m_letters.Length - 1);
			return (transform_space != TRANSFORM_SPACE.WORLD) ? m_letters[letter_idx].ScaleLocal : m_letters[letter_idx].Scale;
		}

		private IEnumerator TimeDelay(float delay, AnimationTime time_type)
		{
			if (time_type == AnimationTime.GAME_TIME)
			{
				yield return new WaitForSeconds(delay);
				yield break;
			}
			float timer = 0f;
			float last_time = Time.realtimeSinceStartup;
			while (timer < delay)
			{
				float delta_time = Time.realtimeSinceStartup - last_time;
				if (delta_time > 0.1f)
				{
					delta_time = 0.1f;
				}
				timer += delta_time;
				last_time = Time.realtimeSinceStartup;
				yield return false;
			}
		}

		public void ImportData(string data, PresetAnimationSection animationSection, PRESET_ANIMATION_SECTION section, bool force_clear_old_audio_particles = false)
		{
			if (force_clear_old_audio_particles)
			{
				ClearCachedAudioParticleInstances();
			}
			int num_actions_added = 0;
			int num_loops_added = 0;
			int action_insert_index = 0;
			int loop_insert_index = 0;
			switch (section)
			{
			case PRESET_ANIMATION_SECTION.MAIN:
				action_insert_index = m_preset_intro.m_start_action + m_preset_intro.m_num_actions + (m_preset_intro.m_active ? 1 : 0);
				loop_insert_index = m_preset_intro.m_start_loop + m_preset_intro.m_num_loops;
				break;
			case PRESET_ANIMATION_SECTION.OUTRO:
				action_insert_index = m_preset_main.m_start_action + m_preset_main.m_num_actions + (m_preset_main.m_active ? 1 : 0);
				loop_insert_index = m_preset_main.m_start_loop + m_preset_main.m_num_loops;
				break;
			}
			tfxJSONObject tfxJSONObject = tfxJSONObject.Parse(data, true);
			if (tfxJSONObject != null)
			{
				if (m_master_animations == null || m_master_animations.Count == 0)
				{
					m_master_animations = new List<LetterAnimation>
					{
						new LetterAnimation()
					};
				}
				m_master_animations[0].ImportPresetSectionData(tfxJSONObject["LETTER_ANIMATIONS_DATA"].Obj, m_letters, action_insert_index, loop_insert_index, ref num_actions_added, ref num_loops_added, (m_animation_interface_reference == null) ? string.Empty : m_animation_interface_reference.AssetNameSuffix);
				animationSection.m_preset_effect_settings = new List<PresetEffectSetting>();
				if (tfxJSONObject.ContainsKey("PRESET_EFFECT_SETTINGS"))
				{
					foreach (tfxJSONValue item in tfxJSONObject["PRESET_EFFECT_SETTINGS"].Array)
					{
						PresetEffectSetting presetEffectSetting = new PresetEffectSetting();
						presetEffectSetting.ImportData(item.Obj);
						animationSection.m_preset_effect_settings.Add(presetEffectSetting);
					}
				}
			}
			else
			{
				Debug.LogError("TextFx animation import failed. Non-valid JSON data provided");
			}
			if (!Application.isPlaying && m_current_text.Equals(string.Empty))
			{
				m_animation_interface_reference.SetText("TextFx");
			}
			PrepareAnimationData();
			ResetAnimation();
			if (m_animation_interface_reference != null)
			{
				m_animation_interface_reference.UpdateTextFxMesh();
			}
			animationSection.m_num_actions = num_actions_added;
			animationSection.m_num_loops = num_loops_added;
		}

		public void ImportData(string data, bool force_clear_old_audio_particles = false)
		{
			if (force_clear_old_audio_particles)
			{
				ClearCachedAudioParticleInstances();
			}
			tfxJSONObject tfxJSONObject = tfxJSONObject.Parse(data, true);
			if (tfxJSONObject != null)
			{
				m_animate_per = (AnimatePerOptions)tfxJSONObject["m_animate_per"].Number;
				if (tfxJSONObject.ContainsKey("m_begin_delay"))
				{
					m_begin_delay = (float)tfxJSONObject["m_begin_delay"].Number;
				}
				if (tfxJSONObject.ContainsKey("m_begin_on_start"))
				{
					m_begin_on_start = tfxJSONObject["m_begin_on_start"].Boolean;
				}
				if (tfxJSONObject.ContainsKey("m_on_finish_action"))
				{
					m_on_finish_action = (ON_FINISH_ACTION)tfxJSONObject["m_on_finish_action"].Number;
				}
				if (tfxJSONObject.ContainsKey("m_time_type"))
				{
					m_time_type = (AnimationTime)tfxJSONObject["m_time_type"].Number;
				}
				m_master_animations = new List<LetterAnimation>();
				foreach (tfxJSONValue item in tfxJSONObject["LETTER_ANIMATIONS_DATA"].Array)
				{
					LetterAnimation letterAnimation = new LetterAnimation();
					letterAnimation.ImportData(item.Obj, m_animation_interface_reference.AssetNameSuffix);
					m_master_animations.Add(letterAnimation);
				}
			}
			else
			{
				Debug.LogError("TextFx animation import failed. Non-valid JSON data provided");
				this.ImportLegacyData(data);
			}
			if (!Application.isPlaying && m_current_text.Equals(string.Empty))
			{
				m_animation_interface_reference.SetText("TextFx");
			}
			PrepareAnimationData();
			ResetAnimation();
			m_animation_interface_reference.UpdateTextFxMesh();
		}
	}
}
