using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FarmSimulator.Managers.Audio
{
    /// <summary>
    /// Manages multiple sound effects that can play simultaneously
    /// </summary>
    public class SoundTrackManager
    {
        // Dictionary to store loaded sound effects by name
        private Dictionary<string, SoundEffect> soundEffects;
        
        // List of currently playing sound effect instances
        private List<SoundEffectInstance> activeInstances;
        
        // Master volume for all sound effects
        private float masterVolume;
        
        // Maximum number of simultaneous sounds (prevents audio overload)
        private int maxSimultaneousSounds;

        public SoundTrackManager(int maxSimultaneousSounds = 32)
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            activeInstances = new List<SoundEffectInstance>();
            masterVolume = 1.0f;
            this.maxSimultaneousSounds = maxSimultaneousSounds;
        }

        /// <summary>
        /// Register a sound effect with a name for easy access
        /// </summary>
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

        /// <summary>
        /// Play a sound effect by name (Fire and forget - plays once)
        /// </summary>
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

        /// <summary>
        /// Play a sound effect with more control (returns instance for advanced control)
        /// </summary>
        public SoundEffectInstance PlaySoundAdvanced(string name, float volume = 1.0f, float pitch = 0f, float pan = 0f, bool looping = false)
        {
            if (!soundEffects.ContainsKey(name))
            {
                Console.WriteLine($"Sound effect '{name}' not found!");
                return null;
            }

            // Check if we've hit the simultaneous sound limit
            if (activeInstances.Count >= maxSimultaneousSounds)
            {
                Console.WriteLine($"Max simultaneous sounds reached ({maxSimultaneousSounds}). Skipping sound.");
                return null;
            }

            // Create an instance for more control
            SoundEffectInstance instance = soundEffects[name].CreateInstance();
            instance.Volume = MathHelper.Clamp(volume * masterVolume, 0f, 1f);
            instance.Pitch = MathHelper.Clamp(pitch, -1f, 1f);
            instance.Pan = MathHelper.Clamp(pan, -1f, 1f);
            instance.IsLooped = looping;
            
            instance.Play();
            activeInstances.Add(instance);

            return instance;
        }

        /// <summary>
        /// Play a random sound from a list (useful for variations like footsteps, hits, etc.)
        /// </summary>
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

        /// <summary>
        /// Stop a specific sound effect instance
        /// </summary>
        public void StopSound(SoundEffectInstance instance)
        {
            if (instance != null)
            {
                instance.Stop();
                activeInstances.Remove(instance);
                instance.Dispose();
            }
        }

        /// <summary>
        /// Stop all currently playing sounds
        /// </summary>
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

        /// <summary>
        /// Update method - call this in your Game.Update() to clean up finished sounds
        /// </summary>
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

        /// <summary>
        /// Set master volume for all sound effects (0.0 to 1.0)
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = MathHelper.Clamp(volume, 0f, 1f);
            
            // Update volume for all active instances
            foreach (var instance in activeInstances)
            {
                instance.Volume = masterVolume;
            }
        }

        /// <summary>
        /// Pause all active sound effects
        /// </summary>
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

        /// <summary>
        /// Resume all paused sound effects
        /// </summary>
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

        /// <summary>
        /// Check if a sound effect is registered
        /// </summary>
        public bool HasSound(string name)
        {
            return soundEffects.ContainsKey(name);
        }

        // Properties
        public float MasterVolume => masterVolume;
        public int ActiveSoundCount => activeInstances.Count;
        public int RegisteredSoundCount => soundEffects.Count;
    }
}

/* 
 * USAGE EXAMPLE:
 * 
 * In your Game1.cs:
 * 
 * public class Game1 : Game
 * {
 *     private SoundEffectManager soundManager;
 *     
 *     protected override void LoadContent()
 *     {
 *         soundManager = new SoundEffectManager();
 *         
 *         // Load and register all your sound effects
 *         soundManager.RegisterSound("cow_moo", Content.Load<SoundEffect>("Audio/cow_moo"));
 *         soundManager.RegisterSound("chicken_cluck", Content.Load<SoundEffect>("Audio/chicken_cluck"));
 *         soundManager.RegisterSound("footstep", Content.Load<SoundEffect>("Audio/footstep"));
 *         soundManager.RegisterSound("harvest", Content.Load<SoundEffect>("Audio/harvest"));
 *         soundManager.RegisterSound("door_open", Content.Load<SoundEffect>("Audio/door_open"));
 *         soundManager.RegisterSound("water_splash", Content.Load<SoundEffect>("Audio/water_splash"));
 *     }
 *     
 *     protected override void Update(GameTime gameTime)
 *     {
 *         // Clean up finished sounds
 *         soundManager.Update(gameTime);
 *         
 *         // Play sounds anywhere in your code - they can all play at once!
 *         
 *         // Simple playback
 *         soundManager.PlaySound("cow_moo");
 *         
 *         // With volume control
 *         soundManager.PlaySound("footstep", volume: 0.5f);
 *         
 *         // With pitch variation (makes sounds less repetitive)
 *         soundManager.PlaySound("chicken_cluck", volume: 1.0f, pitch: 0.2f);
 *         
 *         // Advanced playback with looping
 *         var waterSound = soundManager.PlaySoundAdvanced("water_splash", looping: true);
 *         // Later you can stop it: soundManager.StopSound(waterSound);
 *         
 *         base.Update(gameTime);
 *     }
 * }
 * 
 * In your Cow class:
 * 
 * public void Update(GameTime gameTime, SoundEffectManager soundManager)
 * {
 *     mooTimer += gameTime.ElapsedGameTime.TotalSeconds;
 *     
 *     if (mooTimer >= mooInterval)
 *     {
 *         // Play cow moo sound with slight pitch variation for variety
 *         float randomPitch = (float)(random.NextDouble() * 0.4 - 0.2); // -0.2 to +0.2
 *         soundManager.PlaySound("cow_moo", volume: 0.7f, pitch: randomPitch);
 *         
 *         mooTimer = 0;
 *         mooInterval = 5.0 + (random.NextDouble() * 10.0);
 *     }
 * }
 * 
 * HOW TO ADD SOUND EFFECTS TO CONTENT PIPELINE:
 * 
 * 1. Right-click on Content.mgcb in your project
 * 2. Open with "MonoGame Pipeline Tool"
 * 3. Click Edit > Add > Existing Item
 * 4. Select your .wav or .mp3 file
 * 5. In properties, set "Processor" to "Sound Effect - MonoGame"
 * 6. Build the content
 * 7. Load in code using: Content.Load<SoundEffect>("YourFileName")
 * 
 * IMPORTANT NOTES:
 * - SoundEffect allows MULTIPLE sounds to play at the same time (unlike Song/MediaPlayer)
 * - Use SoundEffect for: footsteps, animal sounds, UI clicks, impacts, etc.
 * - Use Song (MediaPlayer) for: background music only
 * - Pitch: -1.0 (lower) to +1.0 (higher), 0 is normal
 * - Pan: -1.0 (left speaker) to +1.0 (right speaker), 0 is center
 * - PlaySound() is "fire and forget" - simple and automatic
 * - PlaySoundAdvanced() gives you control to stop/pause/modify the sound
 */