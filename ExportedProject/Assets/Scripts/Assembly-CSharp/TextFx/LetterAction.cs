using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class LetterAction
	{
		private bool m_editor_folded;

		public bool m_offset_from_last;

		public ACTION_TYPE m_action_type;

		public bool m_colour_transition_active;

		public ActionColorProgression m_start_colour = new ActionColorProgression(new VertexColour(Color.black), true);

		public ActionColorProgression m_end_colour = new ActionColorProgression(new VertexColour(Color.black), true);

		public bool m_position_transition_active;

		public AxisEasingOverrideData m_position_axis_ease_data = new AxisEasingOverrideData();

		public ActionPositionVector3Progression m_start_pos = new ActionPositionVector3Progression(Vector3.zero);

		public ActionPositionVector3Progression m_end_pos = new ActionPositionVector3Progression(Vector3.zero);

		public bool m_global_rotation_transition_active;

		public AxisEasingOverrideData m_global_rotation_axis_ease_data = new AxisEasingOverrideData();

		public ActionVector3Progression m_global_start_euler_rotation = new ActionVector3Progression(Vector3.zero);

		public ActionVector3Progression m_global_end_euler_rotation = new ActionVector3Progression(Vector3.zero);

		public bool m_local_rotation_transition_active;

		public AxisEasingOverrideData m_rotation_axis_ease_data = new AxisEasingOverrideData();

		public ActionVector3Progression m_start_euler_rotation = new ActionVector3Progression(Vector3.zero);

		public ActionVector3Progression m_end_euler_rotation = new ActionVector3Progression(Vector3.zero);

		public bool m_global_scale_transition_active;

		public AxisEasingOverrideData m_global_scale_axis_ease_data = new AxisEasingOverrideData();

		public ActionVector3Progression m_global_start_scale = new ActionVector3Progression(Vector3.one);

		public ActionVector3Progression m_global_end_scale = new ActionVector3Progression(Vector3.one);

		public bool m_local_scale_transition_active;

		public AxisEasingOverrideData m_scale_axis_ease_data = new AxisEasingOverrideData();

		public ActionVector3Progression m_start_scale = new ActionVector3Progression(Vector3.one);

		public ActionVector3Progression m_end_scale = new ActionVector3Progression(Vector3.one);

		public bool m_force_same_start_time;

		public bool m_delay_with_white_space_influence;

		public ActionFloatProgression m_delay_progression = new ActionFloatProgression(0f);

		public ActionFloatProgression m_duration_progression = new ActionFloatProgression(1f);

		public EasingEquation m_ease_type;

		public int m_letter_anchor_start = 4;

		public int m_letter_anchor_end = 4;

		public bool m_letter_anchor_2_way;

		[SerializeField]
		private List<ParticleEffectSetup> m_particle_effects = new List<ParticleEffectSetup>();

		[SerializeField]
		private List<AudioEffectSetup> m_audio_effects = new List<AudioEffectSetup>();

		[SerializeField]
		private Vector3 m_anchor_offset;

		[SerializeField]
		private Vector3 m_anchor_offset_end;

		private AudioEffectSetup _audio_effect_setup;

		private ParticleEffectSetup _particle_effect_setup;

		public bool FoldedInEditor
		{
			get
			{
				return m_editor_folded;
			}
			set
			{
				m_editor_folded = value;
			}
		}

		public Vector3 AnchorOffsetStart
		{
			get
			{
				return m_anchor_offset;
			}
		}

		public Vector3 AnchorOffsetEnd
		{
			get
			{
				return m_anchor_offset_end;
			}
		}

		public int NumParticleEffectSetups
		{
			get
			{
				return (m_particle_effects != null) ? m_particle_effects.Count : 0;
			}
		}

		public int NumAudioEffectSetups
		{
			get
			{
				return (m_audio_effects != null) ? m_audio_effects.Count : 0;
			}
		}

		public List<ParticleEffectSetup> ParticleEffectSetups
		{
			get
			{
				return m_particle_effects;
			}
		}

		public List<AudioEffectSetup> AudioEffectSetups
		{
			get
			{
				return m_audio_effects;
			}
		}

		public bool ParticleEffectsEditorDisplay { get; set; }

		public bool AudioEffectsEditorDisplay { get; set; }

		public ParticleEffectSetup GetParticleEffectSetup(int index)
		{
			if (index >= 0 && index < m_particle_effects.Count)
			{
				return m_particle_effects[index];
			}
			return null;
		}

		public ParticleEffectSetup AddParticleEffectSetup()
		{
			if (m_particle_effects == null)
			{
				m_particle_effects = new List<ParticleEffectSetup>();
			}
			ParticleEffectSetup particleEffectSetup = new ParticleEffectSetup();
			m_particle_effects.Add(particleEffectSetup);
			return particleEffectSetup;
		}

		public void AddParticleEffectSetup(ParticleEffectSetup particle_setup)
		{
			if (m_particle_effects == null)
			{
				m_particle_effects = new List<ParticleEffectSetup>();
			}
			m_particle_effects.Add(particle_setup);
		}

		public void RemoveParticleEffectSetup(int index)
		{
			if (m_particle_effects != null && index >= 0 && index < m_particle_effects.Count)
			{
				m_particle_effects.RemoveAt(index);
			}
		}

		public void ClearParticleEffectSetups()
		{
			m_particle_effects.Clear();
		}

		public AudioEffectSetup GetAudioEffectSetup(int index)
		{
			if (index >= 0 && index < m_audio_effects.Count)
			{
				return m_audio_effects[index];
			}
			return null;
		}

		public AudioEffectSetup AddAudioEffectSetup()
		{
			if (m_audio_effects == null)
			{
				m_audio_effects = new List<AudioEffectSetup>();
			}
			AudioEffectSetup audioEffectSetup = new AudioEffectSetup();
			m_audio_effects.Add(audioEffectSetup);
			return audioEffectSetup;
		}

		public void AddAudioEffectSetup(AudioEffectSetup audio_setup)
		{
			if (m_audio_effects == null)
			{
				m_audio_effects = new List<AudioEffectSetup>();
			}
			m_audio_effects.Add(audio_setup);
		}

		public void RemoveAudioEffectSetup(int index)
		{
			if (m_audio_effects != null && index >= 0 && index < m_audio_effects.Count)
			{
				m_audio_effects.RemoveAt(index);
			}
		}

		public void ClearAudioEffectSetups()
		{
			m_audio_effects.Clear();
		}

		public void SoftReset(LetterAction prev_action, AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per, bool first_action = false)
		{
			if (!m_offset_from_last && !first_action)
			{
				if (m_start_colour.UniqueRandom)
				{
					m_start_colour.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_colour.Values);
				}
				if (m_start_pos.UniqueRandom)
				{
					m_start_pos.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_pos.Values);
				}
				if (m_start_euler_rotation.UniqueRandom)
				{
					m_start_euler_rotation.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_euler_rotation.Values);
				}
				if (m_global_start_euler_rotation.UniqueRandom)
				{
					m_global_start_euler_rotation.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_global_end_euler_rotation.Values);
				}
				if (m_start_scale.UniqueRandom)
				{
					m_start_scale.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_scale.Values);
				}
				if (m_global_start_scale.UniqueRandom)
				{
					m_global_start_scale.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_global_end_scale.Values);
				}
			}
			if (m_end_colour.UniqueRandom)
			{
				m_end_colour.CalculateUniqueRandom(progression_variables, animate_per, m_start_colour.Values);
			}
			if (m_end_pos.UniqueRandom)
			{
				m_end_pos.CalculateUniqueRandom(progression_variables, animate_per, m_start_pos.Values);
			}
			if (m_end_euler_rotation.UniqueRandom)
			{
				m_end_euler_rotation.CalculateUniqueRandom(progression_variables, animate_per, m_start_euler_rotation.Values);
			}
			if (m_end_scale.UniqueRandom)
			{
				m_end_scale.CalculateUniqueRandom(progression_variables, animate_per, m_start_scale.Values);
			}
			if (m_global_end_euler_rotation.UniqueRandom)
			{
				m_global_end_euler_rotation.CalculateUniqueRandom(progression_variables, animate_per, m_global_start_euler_rotation.Values);
			}
			if (m_global_end_scale.UniqueRandom)
			{
				m_global_end_scale.CalculateUniqueRandom(progression_variables, animate_per, m_global_start_scale.Values);
			}
			if (m_delay_progression.UniqueRandom)
			{
				m_delay_progression.CalculateUniqueRandom(progression_variables, animate_per);
			}
			if (m_duration_progression.UniqueRandom)
			{
				m_duration_progression.CalculateUniqueRandom(progression_variables, animate_per);
			}
			if (m_audio_effects != null)
			{
				for (int i = 0; i < m_audio_effects.Count; i++)
				{
					_audio_effect_setup = m_audio_effects[i];
					if (_audio_effect_setup.m_delay.UniqueRandom)
					{
						_audio_effect_setup.m_delay.CalculateUniqueRandom(progression_variables, animate_per);
					}
					if (_audio_effect_setup.m_offset_time.UniqueRandom)
					{
						_audio_effect_setup.m_offset_time.CalculateUniqueRandom(progression_variables, animate_per);
					}
					if (_audio_effect_setup.m_volume.UniqueRandom)
					{
						_audio_effect_setup.m_volume.CalculateUniqueRandom(progression_variables, animate_per);
					}
					if (_audio_effect_setup.m_pitch.UniqueRandom)
					{
						_audio_effect_setup.m_pitch.CalculateUniqueRandom(progression_variables, animate_per);
					}
				}
			}
			if (m_particle_effects == null)
			{
				return;
			}
			for (int j = 0; j < m_particle_effects.Count; j++)
			{
				_particle_effect_setup = m_particle_effects[j];
				if (_particle_effect_setup.m_position_offset.UniqueRandom)
				{
					_particle_effect_setup.m_position_offset.CalculateUniqueRandom(progression_variables, animate_per, null);
				}
				if (_particle_effect_setup.m_rotation_offset.UniqueRandom)
				{
					_particle_effect_setup.m_rotation_offset.CalculateUniqueRandom(progression_variables, animate_per, null);
				}
				if (_particle_effect_setup.m_delay.UniqueRandom)
				{
					_particle_effect_setup.m_delay.CalculateUniqueRandom(progression_variables, animate_per);
				}
				if (_particle_effect_setup.m_duration.UniqueRandom)
				{
					_particle_effect_setup.m_duration.CalculateUniqueRandom(progression_variables, animate_per);
				}
			}
		}

		public void SoftResetStarts(LetterAction prev_action, AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per)
		{
			if (!m_offset_from_last && m_start_colour.UniqueRandom)
			{
				m_start_colour.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_colour.Values);
			}
			if (!m_offset_from_last)
			{
				if (m_start_pos.UniqueRandom)
				{
					m_start_pos.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_pos.Values);
				}
				if (m_start_euler_rotation.UniqueRandom)
				{
					m_start_euler_rotation.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_euler_rotation.Values);
				}
				if (m_start_scale.UniqueRandom)
				{
					m_start_scale.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_end_scale.Values);
				}
				if (m_global_start_euler_rotation.UniqueRandom)
				{
					m_global_start_euler_rotation.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_global_end_euler_rotation.Values);
				}
				if (m_global_start_scale.UniqueRandom)
				{
					m_global_start_scale.CalculateUniqueRandom(progression_variables, animate_per, (prev_action == null) ? null : prev_action.m_global_end_scale.Values);
				}
			}
		}

		public LetterAction ContinueActionFromThis()
		{
			LetterAction letterAction = new LetterAction();
			letterAction.m_offset_from_last = true;
			letterAction.m_editor_folded = true;
			letterAction.m_position_axis_ease_data = m_position_axis_ease_data.Clone();
			letterAction.m_rotation_axis_ease_data = m_rotation_axis_ease_data.Clone();
			letterAction.m_scale_axis_ease_data = m_scale_axis_ease_data.Clone();
			letterAction.m_start_colour = m_end_colour.Clone();
			letterAction.m_end_colour = m_end_colour.Clone();
			letterAction.m_start_pos = m_end_pos.CloneThis();
			letterAction.m_end_pos = m_end_pos.CloneThis();
			letterAction.m_start_euler_rotation = m_end_euler_rotation.Clone();
			letterAction.m_end_euler_rotation = m_end_euler_rotation.Clone();
			letterAction.m_start_scale = m_end_scale.Clone();
			letterAction.m_end_scale = m_end_scale.Clone();
			letterAction.m_global_start_euler_rotation = m_global_end_euler_rotation.Clone();
			letterAction.m_global_end_euler_rotation = m_global_end_euler_rotation.Clone();
			letterAction.m_global_start_scale = m_global_end_scale.Clone();
			letterAction.m_global_end_scale = m_global_end_scale.Clone();
			letterAction.m_delay_progression = new ActionFloatProgression(0f);
			letterAction.m_duration_progression = new ActionFloatProgression(1f);
			letterAction.m_letter_anchor_start = ((!m_letter_anchor_2_way) ? m_letter_anchor_start : m_letter_anchor_end);
			letterAction.m_ease_type = m_ease_type;
			return letterAction;
		}

		private int GetProgressionTotal(int num_letters, int num_words, int num_lines, AnimatePerOptions animate_per_default, AnimatePerOptions animate_per_override, bool overriden)
		{
			switch ((!overriden) ? animate_per_default : animate_per_override)
			{
			case AnimatePerOptions.LETTER:
				return num_letters;
			case AnimatePerOptions.WORD:
				return num_words;
			case AnimatePerOptions.LINE:
				return num_lines;
			default:
				return num_letters;
			}
		}

		public void PrepareData(TextFxAnimationManager anim_manager, ref LetterSetup[] letters, LetterAnimation animation_ref, int action_idx, ANIMATION_DATA_TYPE what_to_update, int num_letters, int num_white_space_chars_to_include, int num_words, int num_lines, LetterAction prev_action, AnimatePerOptions animate_per, ActionColorProgression defaultTextColour, bool prev_action_end_state = true)
		{
			m_start_colour.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.COLOUR, true);
			m_start_euler_rotation.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.LOCAL_ROTATION, true);
			m_start_pos.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.POSITION, true);
			m_start_scale.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.LOCAL_SCALE, true);
			m_global_start_euler_rotation.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.GLOBAL_ROTATION, true);
			m_global_start_scale.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.GLOBAL_SCALE, true);
			m_end_colour.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.COLOUR, false);
			m_end_euler_rotation.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.LOCAL_ROTATION, false);
			m_end_pos.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.POSITION, false);
			m_end_scale.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.LOCAL_SCALE, false);
			m_global_end_euler_rotation.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.GLOBAL_ROTATION, false);
			m_global_end_scale.SetReferenceData(action_idx, ANIMATION_DATA_TYPE.GLOBAL_SCALE, false);
			if (what_to_update == ANIMATION_DATA_TYPE.DURATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_duration_progression.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, m_duration_progression.AnimatePer, m_duration_progression.OverrideAnimatePerOption));
			}
			if ((what_to_update == ANIMATION_DATA_TYPE.AUDIO_EFFECTS || what_to_update == ANIMATION_DATA_TYPE.ALL) && m_audio_effects != null)
			{
				foreach (AudioEffectSetup audio_effect in m_audio_effects)
				{
					audio_effect.m_delay.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, audio_effect.m_delay.AnimatePer, audio_effect.m_delay.OverrideAnimatePerOption));
					audio_effect.m_offset_time.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, audio_effect.m_offset_time.AnimatePer, audio_effect.m_offset_time.OverrideAnimatePerOption));
					audio_effect.m_volume.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, audio_effect.m_volume.AnimatePer, audio_effect.m_volume.OverrideAnimatePerOption));
					audio_effect.m_pitch.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, audio_effect.m_pitch.AnimatePer, audio_effect.m_pitch.OverrideAnimatePerOption));
				}
			}
			if ((what_to_update == ANIMATION_DATA_TYPE.PARTICLE_EFFECTS || what_to_update == ANIMATION_DATA_TYPE.ALL) && m_particle_effects != null)
			{
				foreach (ParticleEffectSetup particle_effect in m_particle_effects)
				{
					particle_effect.m_position_offset.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, particle_effect.m_position_offset.AnimatePer, particle_effect.m_position_offset.OverrideAnimatePerOption), null);
					particle_effect.m_rotation_offset.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, particle_effect.m_rotation_offset.AnimatePer, particle_effect.m_rotation_offset.OverrideAnimatePerOption), null);
					particle_effect.m_delay.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, particle_effect.m_delay.AnimatePer, particle_effect.m_delay.OverrideAnimatePerOption));
					particle_effect.m_duration.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, particle_effect.m_duration.AnimatePer, particle_effect.m_duration.OverrideAnimatePerOption));
				}
			}
			if (m_action_type == ACTION_TYPE.BREAK)
			{
				if (prev_action != null)
				{
					m_start_colour.SetValueReference((!prev_action_end_state) ? prev_action.m_start_colour : prev_action.m_end_colour);
					m_start_pos.SetValueReference((!prev_action_end_state) ? prev_action.m_start_pos : prev_action.m_end_pos);
					m_start_euler_rotation.SetValueReference((!prev_action_end_state) ? prev_action.m_start_euler_rotation : prev_action.m_end_euler_rotation);
					m_start_scale.SetValueReference((!prev_action_end_state) ? prev_action.m_start_scale : prev_action.m_end_scale);
					m_global_start_euler_rotation.SetValueReference((!prev_action_end_state) ? prev_action.m_global_start_euler_rotation : prev_action.m_global_end_euler_rotation);
					m_global_start_scale.SetValueReference((!prev_action_end_state) ? prev_action.m_global_start_scale : prev_action.m_global_end_scale);
					m_end_colour.SetValueReference((!prev_action_end_state) ? prev_action.m_start_colour : prev_action.m_end_colour);
					m_end_pos.SetValueReference((!prev_action_end_state) ? prev_action.m_start_pos : prev_action.m_end_pos);
					m_end_euler_rotation.SetValueReference((!prev_action_end_state) ? prev_action.m_start_euler_rotation : prev_action.m_end_euler_rotation);
					m_end_scale.SetValueReference((!prev_action_end_state) ? prev_action.m_start_scale : prev_action.m_end_scale);
					m_global_end_euler_rotation.SetValueReference((!prev_action_end_state) ? prev_action.m_global_start_euler_rotation : prev_action.m_global_end_euler_rotation);
					m_global_end_scale.SetValueReference((!prev_action_end_state) ? prev_action.m_global_start_scale : prev_action.m_global_end_scale);
				}
				return;
			}
			if (animation_ref.m_letters_to_animate_option != 0 || (m_delay_progression.Progression != 2 && m_delay_progression.Progression != 3))
			{
				m_delay_with_white_space_influence = false;
			}
			if (what_to_update == ANIMATION_DATA_TYPE.DELAY || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_delay_progression.CalculateProgressions(GetProgressionTotal(num_letters + (m_delay_with_white_space_influence ? num_white_space_chars_to_include : 0), num_words, num_lines, animate_per, m_delay_progression.AnimatePer, m_delay_progression.OverrideAnimatePerOption));
			}
			if (what_to_update == ANIMATION_DATA_TYPE.COLOUR || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				if (m_offset_from_last && prev_action != null)
				{
					m_start_colour.SetValueReference((!prev_action_end_state) ? prev_action.m_start_colour : prev_action.m_end_colour);
				}
				else
				{
					m_start_colour.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, m_start_colour.AnimatePer, m_start_colour.OverrideAnimatePerOption), (prev_action == null) ? defaultTextColour : prev_action.m_end_colour, prev_action == null || m_colour_transition_active);
				}
				m_end_colour.CalculateProgressions(GetProgressionTotal(num_letters, num_words, num_lines, animate_per, m_end_colour.AnimatePer, m_end_colour.OverrideAnimatePerOption), m_start_colour, prev_action == null || m_colour_transition_active);
			}
			if (m_offset_from_last && prev_action != null)
			{
				if (what_to_update == ANIMATION_DATA_TYPE.POSITION || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_start_pos.SetValueReference((!prev_action_end_state) ? prev_action.m_start_pos : prev_action.m_end_pos);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.LOCAL_ROTATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_start_euler_rotation.SetValueReference((!prev_action_end_state) ? prev_action.m_start_euler_rotation : prev_action.m_end_euler_rotation);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.LOCAL_SCALE || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_start_scale.SetValueReference((!prev_action_end_state) ? prev_action.m_start_scale : prev_action.m_end_scale);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.GLOBAL_ROTATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_global_start_euler_rotation.SetValueReference((!prev_action_end_state) ? prev_action.m_global_start_euler_rotation : prev_action.m_global_end_euler_rotation);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.GLOBAL_SCALE || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_global_start_scale.SetValueReference((!prev_action_end_state) ? prev_action.m_global_start_scale : prev_action.m_global_end_scale);
				}
			}
			else
			{
				if (what_to_update == ANIMATION_DATA_TYPE.POSITION || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_start_pos.CalculatePositionProgressions(anim_manager, animation_ref, letters, GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_start_pos.AnimatePer, m_start_pos.OverrideAnimatePerOption), (prev_action == null) ? null : prev_action.m_end_pos, prev_action == null || m_position_transition_active);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.LOCAL_ROTATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_start_euler_rotation.CalculateRotationProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_start_euler_rotation.AnimatePer, m_start_euler_rotation.OverrideAnimatePerOption), (prev_action == null) ? null : prev_action.m_end_euler_rotation, prev_action == null || m_local_rotation_transition_active);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.LOCAL_SCALE || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_start_scale.CalculateProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_start_scale.AnimatePer, m_start_scale.OverrideAnimatePerOption), (prev_action == null) ? null : prev_action.m_end_scale, prev_action == null || m_local_scale_transition_active);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.GLOBAL_ROTATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_global_start_euler_rotation.CalculateRotationProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_global_start_euler_rotation.AnimatePer, m_global_start_euler_rotation.OverrideAnimatePerOption), (prev_action == null) ? null : prev_action.m_global_end_euler_rotation, prev_action == null || m_global_rotation_transition_active);
				}
				if (what_to_update == ANIMATION_DATA_TYPE.GLOBAL_SCALE || what_to_update == ANIMATION_DATA_TYPE.ALL)
				{
					m_global_start_scale.CalculateProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_global_start_scale.AnimatePer, m_global_start_scale.OverrideAnimatePerOption), (prev_action == null) ? null : prev_action.m_global_end_scale, prev_action == null || m_global_scale_transition_active);
				}
			}
			if (what_to_update == ANIMATION_DATA_TYPE.POSITION || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_end_pos.CalculatePositionProgressions(anim_manager, animation_ref, letters, GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_end_pos.AnimatePer, m_end_pos.OverrideAnimatePerOption), m_start_pos, prev_action == null || m_position_transition_active);
			}
			if (what_to_update == ANIMATION_DATA_TYPE.LOCAL_ROTATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_end_euler_rotation.CalculateRotationProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_end_euler_rotation.AnimatePer, m_end_euler_rotation.OverrideAnimatePerOption), m_start_euler_rotation, prev_action == null || m_local_rotation_transition_active);
			}
			if (what_to_update == ANIMATION_DATA_TYPE.LOCAL_SCALE || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_end_scale.CalculateProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_end_scale.AnimatePer, m_end_scale.OverrideAnimatePerOption), m_start_scale, prev_action == null || m_local_scale_transition_active);
			}
			if (what_to_update == ANIMATION_DATA_TYPE.GLOBAL_ROTATION || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_global_end_euler_rotation.CalculateRotationProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_global_end_euler_rotation.AnimatePer, m_global_end_euler_rotation.OverrideAnimatePerOption), m_global_start_euler_rotation, prev_action == null || m_global_rotation_transition_active);
			}
			if (what_to_update == ANIMATION_DATA_TYPE.GLOBAL_SCALE || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				m_global_end_scale.CalculateProgressions(GetProgressionTotal(num_letters + num_white_space_chars_to_include, num_words, num_lines, animate_per, m_global_end_scale.AnimatePer, m_global_end_scale.OverrideAnimatePerOption), m_global_start_scale, prev_action == null || m_global_scale_transition_active);
			}
			if (what_to_update == ANIMATION_DATA_TYPE.POSITION || what_to_update == ANIMATION_DATA_TYPE.POSITION || what_to_update == ANIMATION_DATA_TYPE.LETTER_ANCHOR || what_to_update == ANIMATION_DATA_TYPE.ALL)
			{
				CalculateLetterAnchorOffset();
			}
		}

		public void CalculateLetterAnchorOffset()
		{
			m_anchor_offset = AnchorOffsetToVector3((TextfxTextAnchor)m_letter_anchor_start);
			m_anchor_offset_end = ((!m_letter_anchor_2_way) ? m_anchor_offset : AnchorOffsetToVector3((TextfxTextAnchor)m_letter_anchor_end));
		}

		private Vector3 AnchorOffsetToVector3(TextfxTextAnchor anchor)
		{
			Vector3 zero = Vector3.zero;
			switch (anchor)
			{
			case TextfxTextAnchor.UpperRight:
			case TextfxTextAnchor.MiddleRight:
			case TextfxTextAnchor.LowerRight:
				zero.x = 1f;
				break;
			case TextfxTextAnchor.UpperCenter:
			case TextfxTextAnchor.MiddleCenter:
			case TextfxTextAnchor.LowerCenter:
				zero.x = 0.5f;
				break;
			}
			switch (anchor)
			{
			case TextfxTextAnchor.MiddleLeft:
			case TextfxTextAnchor.MiddleCenter:
			case TextfxTextAnchor.MiddleRight:
				zero.y = 0.5f;
				break;
			case TextfxTextAnchor.LowerLeft:
			case TextfxTextAnchor.LowerCenter:
			case TextfxTextAnchor.LowerRight:
				zero.y = 1f;
				break;
			}
			return zero;
		}

		public tfxJSONValue ExportData()
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["m_action_type"] = (double)m_action_type;
			tfxJSONObject["m_ease_type"] = (double)m_ease_type;
			tfxJSONObject["m_force_same_start_time"] = m_force_same_start_time;
			tfxJSONObject["m_letter_anchor_start"] = m_letter_anchor_start;
			tfxJSONObject["m_letter_anchor_end"] = m_letter_anchor_end;
			tfxJSONObject["m_letter_anchor_2_way"] = m_letter_anchor_2_way;
			tfxJSONObject["m_offset_from_last"] = m_offset_from_last;
			tfxJSONObject["m_position_axis_ease_data"] = m_position_axis_ease_data.ExportData();
			tfxJSONObject["m_rotation_axis_ease_data"] = m_rotation_axis_ease_data.ExportData();
			tfxJSONObject["m_scale_axis_ease_data"] = m_scale_axis_ease_data.ExportData();
			tfxJSONObject["m_colour_transition_active"] = m_colour_transition_active;
			tfxJSONObject["m_start_colour"] = m_start_colour.ExportData();
			tfxJSONObject["m_position_transition_active"] = m_position_transition_active;
			tfxJSONObject["m_start_pos"] = m_start_pos.ExportData();
			tfxJSONObject["m_local_rotation_transition_active"] = m_local_rotation_transition_active;
			tfxJSONObject["m_start_euler_rotation"] = m_start_euler_rotation.ExportData();
			tfxJSONObject["m_local_scale_transition_active"] = m_local_scale_transition_active;
			tfxJSONObject["m_start_scale"] = m_start_scale.ExportData();
			tfxJSONObject["m_global_rotation_transition_active"] = m_global_rotation_transition_active;
			tfxJSONObject["m_global_start_euler_rotation"] = m_global_start_euler_rotation.ExportData();
			tfxJSONObject["m_global_scale_transition_active"] = m_global_scale_transition_active;
			tfxJSONObject["m_global_start_scale"] = m_global_start_scale.ExportData();
			tfxJSONObject["m_end_colour"] = m_end_colour.ExportData();
			tfxJSONObject["m_end_pos"] = m_end_pos.ExportData();
			tfxJSONObject["m_end_euler_rotation"] = m_end_euler_rotation.ExportData();
			tfxJSONObject["m_end_scale"] = m_end_scale.ExportData();
			tfxJSONObject["m_global_end_euler_rotation"] = m_global_end_euler_rotation.ExportData();
			tfxJSONObject["m_global_end_scale"] = m_global_end_scale.ExportData();
			tfxJSONObject["m_delay_progression"] = m_delay_progression.ExportData();
			tfxJSONObject["m_duration_progression"] = m_duration_progression.ExportData();
			tfxJSONArray tfxJSONArray = new tfxJSONArray();
			foreach (AudioEffectSetup audio_effect in m_audio_effects)
			{
				if (!(audio_effect.m_audio_clip == null))
				{
					tfxJSONArray.Add(audio_effect.ExportData());
				}
			}
			tfxJSONObject["AUDIO_EFFECTS_DATA"] = tfxJSONArray;
			tfxJSONArray tfxJSONArray2 = new tfxJSONArray();
			foreach (ParticleEffectSetup particle_effect in m_particle_effects)
			{
				if (!(particle_effect.m_shuriken_particle_effect == null))
				{
					tfxJSONArray2.Add(particle_effect.ExportData());
				}
			}
			tfxJSONObject["PARTICLE_EFFECTS_DATA"] = tfxJSONArray2;
			return new tfxJSONValue(tfxJSONObject);
		}

		public void ImportData(tfxJSONObject json_data, string assetNameSuffix = "", float timing_scale = -1f)
		{
			m_action_type = (ACTION_TYPE)json_data["m_action_type"].Number;
			m_ease_type = (EasingEquation)json_data["m_ease_type"].Number;
			m_force_same_start_time = json_data["m_force_same_start_time"].Boolean;
			m_letter_anchor_start = (int)json_data["m_letter_anchor_start"].Number;
			m_letter_anchor_end = (int)json_data["m_letter_anchor_end"].Number;
			m_letter_anchor_2_way = json_data["m_letter_anchor_2_way"].Boolean;
			m_offset_from_last = json_data["m_offset_from_last"].Boolean;
			m_position_axis_ease_data.ImportData(json_data["m_position_axis_ease_data"].Obj);
			m_rotation_axis_ease_data.ImportData(json_data["m_rotation_axis_ease_data"].Obj);
			m_scale_axis_ease_data.ImportData(json_data["m_scale_axis_ease_data"].Obj);
			m_colour_transition_active = !json_data.ContainsKey("m_colour_transition_active") || json_data["m_colour_transition_active"].Boolean;
			m_position_transition_active = !json_data.ContainsKey("m_position_transition_active") || json_data["m_position_transition_active"].Boolean;
			m_local_rotation_transition_active = !json_data.ContainsKey("m_local_rotation_transition_active") || json_data["m_local_rotation_transition_active"].Boolean;
			m_local_scale_transition_active = !json_data.ContainsKey("m_local_scale_transition_active") || json_data["m_local_scale_transition_active"].Boolean;
			m_global_rotation_transition_active = !json_data.ContainsKey("m_global_rotation_transition_active") || json_data["m_global_rotation_transition_active"].Boolean;
			m_global_scale_transition_active = !json_data.ContainsKey("m_global_scale_transition_active") || json_data["m_global_scale_transition_active"].Boolean;
			if (json_data.ContainsKey("m_start_colour"))
			{
				m_start_colour.ImportData(json_data["m_start_colour"].Obj);
			}
			if (json_data.ContainsKey("m_end_colour"))
			{
				m_end_colour.ImportData(json_data["m_end_colour"].Obj);
			}
			if (json_data.ContainsKey("m_start_vertex_colour"))
			{
				m_start_colour.ImportData(json_data["m_start_vertex_colour"].Obj);
			}
			if (json_data.ContainsKey("m_end_vertex_colour"))
			{
				m_start_colour.ImportData(json_data["m_end_vertex_colour"].Obj);
			}
			if (json_data.ContainsKey("m_use_gradient_start"))
			{
				m_start_colour.UseColourGradients = json_data["m_use_gradient_start"].Boolean;
			}
			if (json_data.ContainsKey("m_use_gradient_end"))
			{
				m_end_colour.UseColourGradients = json_data["m_use_gradient_end"].Boolean;
			}
			m_start_pos.ImportData(json_data["m_start_pos"].Obj);
			m_end_pos.ImportData(json_data["m_end_pos"].Obj);
			m_start_euler_rotation.ImportData(json_data["m_start_euler_rotation"].Obj);
			m_end_euler_rotation.ImportData(json_data["m_end_euler_rotation"].Obj);
			m_start_scale.ImportData(json_data["m_start_scale"].Obj);
			m_end_scale.ImportData(json_data["m_end_scale"].Obj);
			if (json_data.ContainsKey("m_global_start_euler_rotation"))
			{
				m_global_start_euler_rotation.ImportData(json_data["m_global_start_euler_rotation"].Obj);
				m_global_end_euler_rotation.ImportData(json_data["m_global_end_euler_rotation"].Obj);
			}
			if (json_data.ContainsKey("m_global_start_scale"))
			{
				m_global_start_scale.ImportData(json_data["m_global_start_scale"].Obj);
				m_global_end_scale.ImportData(json_data["m_global_end_scale"].Obj);
			}
			m_duration_progression.ImportData(json_data["m_duration_progression"].Obj);
			m_delay_progression.ImportData(json_data["m_delay_progression"].Obj);
			if (timing_scale != -1f && m_delay_progression.Progression != 0)
			{
				float valueFrom = m_delay_progression.ValueFrom;
				float valueTo = m_delay_progression.ValueTo;
				float valueThen = m_delay_progression.ValueThen;
				if (m_delay_progression.Progression == 2)
				{
					if (m_delay_progression.UsingThirdValue)
					{
						m_delay_progression.SetEased(valueFrom * timing_scale, valueTo * timing_scale, valueThen * timing_scale);
					}
					else
					{
						m_delay_progression.SetEased(valueFrom * timing_scale, valueTo * timing_scale);
					}
				}
				else if (m_delay_progression.Progression == 3)
				{
					m_delay_progression.SetEasedCustom(valueFrom * timing_scale, valueTo * timing_scale);
				}
				else if (m_delay_progression.Progression == 1)
				{
					m_delay_progression.SetRandom(valueFrom * timing_scale, valueTo * timing_scale, m_delay_progression.UniqueRandomRaw);
				}
			}
			m_audio_effects = new List<AudioEffectSetup>();
			foreach (tfxJSONValue item in json_data["AUDIO_EFFECTS_DATA"].Array)
			{
				AudioEffectSetup audioEffectSetup = new AudioEffectSetup();
				audioEffectSetup.ImportData(item.Obj);
				m_audio_effects.Add(audioEffectSetup);
			}
			m_particle_effects = new List<ParticleEffectSetup>();
			foreach (tfxJSONValue item2 in json_data["PARTICLE_EFFECTS_DATA"].Array)
			{
				ParticleEffectSetup particleEffectSetup = new ParticleEffectSetup();
				particleEffectSetup.ImportData(item2.Obj, assetNameSuffix);
				m_particle_effects.Add(particleEffectSetup);
			}
		}
	}
}
