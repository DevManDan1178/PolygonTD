using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    private static bool muteAllSounds = false;
    public static bool changeMusicPitchWithTimescale = true;
    public Sound[] sounds;
    public Sound[] levelMusic;
    
    [HideInInspector] 
    public Sound[] pausedSounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake() {
        if (instance == null) instance = this;
        else{
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        gameObject.name = "AudioManager";

        foreach(Sound s in sounds){
            initializeSound(s);
        }
        foreach (Sound s in levelMusic)
        {
            initializeSound(s);
        }
    
    }

    private void initializeSound(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        setSoundSourceVolume(s);
    }

    private void setSoundSourceVolume(Sound s)
    {
        s.source.volume = !muteAllSounds ? s.volume : 0;
    }

    public void PlayLevelMusic(int LevelIndex)
    {
        Sound s = levelMusic[LevelIndex];
        if((s.singleInstanceOnly && s.source.isPlaying) || (GameManager.gameEnded && !s.ignoreGameEnded)) return;
        if (changeMusicPitchWithTimescale)
        {
            s.source.pitch = 1 + (Time.timeScale - 1) * 0.1f;
        }
        s.source.Play();
    }
    public void Play(string name){
       
        Sound s = Array.Find(sounds, sound => sound.name == name );
        if(s == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }
        if((s.singleInstanceOnly && s.source.isPlaying) || (GameManager.gameEnded && !s.ignoreGameEnded)) return;
        if(s.scalesWithTime){
            s.source.pitch = Mathf.Clamp(Time.timeScale, 0.1f, 3f) ;
        }
        s.source.Play();
    }
    public void PauseAllSounds(){
        pausedSounds = new Sound[sounds.Length];
        int index = 0;
        foreach(Sound s in sounds){
            if(!s.source.isPlaying) continue;
            s.source.Pause();
            pausedSounds[index] = s;
            index ++;
        }
        foreach(Sound s in levelMusic)
        {
            if(!s.source.isPlaying) continue;
            s.source.Pause();
            pausedSounds[index] = s;
            index ++;
        }
    }
    public void StopAllSounds(){
        foreach(Sound s in sounds){
            if(!s.source.isPlaying) continue;
            s.source.Stop();
        }
        foreach(Sound s in levelMusic){
            if(!s.source.isPlaying) continue;
            s.source.Stop();
        }
    }

    public void ResumeAllSounds(){
        if(pausedSounds.Length == 0) return;
        foreach(Sound s in pausedSounds){
            s?.source?.Play();
        }
    }

    public void Pause(string name){
        Sound s =Array.Find(sounds, sound => sound.name == name );
        if(s == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }
        s.source.Pause();
    }

    public void UpdateSoundTimeScale()
    {
        if (!changeMusicPitchWithTimescale)
        {
            return;
        }
        foreach (Sound s in levelMusic)
        {
            s.source.pitch = 1 + (Time.timeScale - 1) * 0.1f;
        }
    }

    public void SetMuteAllSounds(String muted)
    {
        if (muted != "true" && muted != "false"){
            Debug.LogWarning("AudioManager.SetMuteAllSounds called with invalid argument :" + muted + " is neither \"true\" nor \"false\"");
            return;
        }
        bool mutingAllSounds = muted == "true";
        if (muteAllSounds != mutingAllSounds)
        {
            muteAllSounds = mutingAllSounds;
            foreach(Sound s in sounds){
                setSoundSourceVolume(s);
            }
            foreach(Sound s in levelMusic){
                setSoundSourceVolume(s);
            }
        }
    }
}
