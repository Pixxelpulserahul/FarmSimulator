using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FarmSimulator.Managers.Audio
{
    public class SoundTrackManager
    {
 
        private Dictionary<string, SoundEffect> soundEffects;
        private List<SoundEffectInstance> activeInstances;
        
        private float masterVolume;
        
        private int maxSimultaneousSounds;

        public SoundTrackManager(int maxSimultaneousSounds = 32)
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            activeInstances = new List<SoundEffectInstance>();
            masterVolume = 1.0f;
            this.maxSimultaneousSounds = maxSimultaneousSounds;
        }

        public void RegisterSound(string name, SoundEffect soundEffect)
        {
            Console.WriteLine("Gotcha");
            if (soundEffect == null)
            {
                Console.WriteLine($"Cannot register null sound effect: {name}");
                return;
            }

            if (soundEffects.ContainsKey(name))
            {
                Console.WriteLine($"Sound effect '{name}' already registered. Replacing...");
                soundEffects[name] = soundEffect;
            }
            else
            {
                soundEffects.Add(name, soundEffect);
                Console.WriteLine($"Registered sound effect: {name}");
            }
        }

        public void PlaySound(string name, float volume = 1.0f, float pitch = 0f, float pan = 0f)
        {
            if (!soundEffects.ContainsKey(name))
            {
                Console.WriteLine($"Sound effect '{name}' not found!");
                return;
            }


            // Simple fire-and-forget playback
            float finalVolume = MathHelper.Clamp(volume * masterVolume, 0f, 1f);
            soundEffects[name].Play(finalVolume, pitch, pan);
        }

        public SoundEffectInstance PlaySoundAdvanced(string name, float volume = 1.0f, float pitch = 0f, float pan = 0f, bool looping = false)
        {
            if (!soundEffects.ContainsKey(name))
            {
                Console.WriteLine($"Sound effect '{name}' not found!");
                return null;
            }

            if (activeInstances.Count >= maxSimultaneousSounds)
            {
                Console.WriteLine($"Max simultaneous sounds reached ({maxSimultaneousSounds}). Skipping sound.");
                return null;
            }

            SoundEffectInstance instance = soundEffects[name].CreateInstance();
            instance.Volume = MathHelper.Clamp(volume * masterVolume, 0f, 1f);
            instance.Pitch = MathHelper.Clamp(pitch, -1f, 1f);
            instance.Pan = MathHelper.Clamp(pan, -1f, 1f);
            instance.IsLooped = looping;
            
            instance.Play();
            activeInstances.Add(instance);

            return instance;
        }

        public void PlayRandomSound(List<string> soundNames, float volume = 1.0f, float pitch = 0f, float pan = 0f)
        {
            if (soundNames == null || soundNames.Count == 0)
            {
                Console.WriteLine("No sound names provided for random playback");
                return;
            }

            Random random = new Random();
            string randomSound = soundNames[random.Next(soundNames.Count)];
            PlaySound(randomSound, volume, pitch, pan);
        }

        public void StopSound(SoundEffectInstance instance)
        {
            if (instance != null)
            {
                instance.Stop();
                activeInstances.Remove(instance);
                instance.Dispose();
            }
        }

        public void StopAllSounds()
        {
            foreach (var instance in activeInstances)
            {
                instance.Stop();
                instance.Dispose();
            }
            activeInstances.Clear();
            Console.WriteLine("All sounds stopped");
        }

        public void Update(GameTime gameTime)
        {
            // Clean up finished sound instances
            for (int i = activeInstances.Count - 1; i >= 0; i--)
            {
                if (activeInstances[i].State == SoundState.Stopped)
                {
                    activeInstances[i].Dispose();
                    activeInstances.RemoveAt(i);
                }
            }
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = MathHelper.Clamp(volume, 0f, 1f);
            
            // Update volume for all active instances
            foreach (var instance in activeInstances)
            {
                instance.Volume = masterVolume;
            }
        }

        public void PauseAll()
        {
            foreach (var instance in activeInstances)
            {
                if (instance.State == SoundState.Playing)
                {
                    instance.Pause();
                }
            }
        }

        public void ResumeAll()
        {
            foreach (var instance in activeInstances)
            {
                if (instance.State == SoundState.Paused)
                {
                    instance.Resume();
                }
            }
        }

        public bool HasSound(string name)
        {
            return soundEffects.ContainsKey(name);
        }
    }
}