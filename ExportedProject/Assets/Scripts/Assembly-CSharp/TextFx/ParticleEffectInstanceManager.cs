using System;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class ParticleEffectInstanceManager
	{
		[NonSerialized]
		private TextFxAnimationManager m_animation_manager;

		[NonSerialized]
		private LetterSetup m_letter_setup_ref;

		private ParticleSystem m_particle_system;

		private float m_duration;

		private float m_delay;

		private Vector3 m_position_offset;

		private Quaternion m_rotation_offset;

		private bool m_rotate_with_letter = true;

		private bool m_follow_mesh;

		private bool m_active;

		private Transform m_transform;

		private Quaternion rotation;

		public ParticleEffectInstanceManager(TextFxAnimationManager animation_manager, LetterSetup letter_setup_ref, ParticleEffectSetup effect_setup, AnimationProgressionVariables progression_vars, AnimatePerOptions animate_per, ParticleSystem particle_system = null)
		{
			m_particle_system = particle_system;
			m_letter_setup_ref = letter_setup_ref;
			m_follow_mesh = effect_setup.m_follow_mesh;
			m_duration = effect_setup.m_duration.GetValue(progression_vars, animate_per);
			m_delay = effect_setup.m_delay.GetValue(progression_vars, animate_per);
			m_position_offset = effect_setup.m_position_offset.GetValue(null, progression_vars, animate_per);
			m_rotation_offset = Quaternion.Euler(effect_setup.m_rotation_offset.GetValue(null, progression_vars, animate_per));
			m_rotate_with_letter = effect_setup.m_rotate_relative_to_letter;
			m_animation_manager = animation_manager;
			m_active = false;
			if (m_particle_system != null)
			{
				m_transform = m_particle_system.transform;
				ParticleSystem.MainModule main = m_particle_system.main;
				main.playOnAwake = false;
				if (m_delay <= 0f)
				{
					m_particle_system.Play();
				}
				if (animation_manager.AnimationInterface.LayerOverride >= 0)
				{
					m_particle_system.gameObject.layer = animation_manager.AnimationInterface.LayerOverride;
				}
			}
		}

		private void OrientateEffectToMesh()
		{
			Vector3 normal = m_letter_setup_ref.Normal;
			if (!normal.Equals(Vector3.zero))
			{
				rotation = ((!m_rotate_with_letter) ? Quaternion.identity : Quaternion.LookRotation(normal * -1f, m_letter_setup_ref.UpVector));
				m_transform.position = m_animation_manager.Transform.position + m_animation_manager.Transform.rotation * Vector3.Scale(rotation * m_position_offset + m_letter_setup_ref.CenterLocal, m_animation_manager.Transform.lossyScale);
				rotation *= m_rotation_offset;
				m_transform.rotation = rotation;
			}
			else
			{
				m_transform.position = m_animation_manager.Transform.position + m_position_offset + m_letter_setup_ref.CenterLocal;
			}
		}

		public bool Update(float delta_time)
		{
			if (!m_active)
			{
				if (m_delay > 0f)
				{
					m_delay -= delta_time;
					if (m_delay < 0f)
					{
						m_delay = 0f;
					}
					return false;
				}
				m_active = true;
				OrientateEffectToMesh();
				if (m_duration <= 0f)
				{
					ParticleSystem.MainModule main = m_particle_system.main;
					m_duration = m_particle_system.main.duration + m_particle_system.main.startLifetime.constant;
					main.loop = false;
				}
				m_particle_system.Play(true);
			}
			if (m_follow_mesh)
			{
				OrientateEffectToMesh();
			}
			m_duration -= delta_time;
			if (m_duration > 0f)
			{
				return false;
			}
			if (m_particle_system != null)
			{
				if (Application.isPlaying)
				{
					m_particle_system.Stop(true);
				}
				if (m_particle_system.particleCount > 0)
				{
					return false;
				}
			}
			return true;
		}

		public void Pause(bool state)
		{
			if (m_particle_system != null)
			{
				if (state && !m_particle_system.isPaused)
				{
					m_particle_system.Pause(true);
				}
				else if (!state && m_particle_system.isPaused)
				{
					m_particle_system.Play(true);
				}
			}
		}

		public void Stop(bool force_stop)
		{
			if (m_particle_system != null)
			{
				m_particle_system.Stop(true);
				if (force_stop)
				{
					m_particle_system.Clear(true);
				}
			}
		}
	}
}
