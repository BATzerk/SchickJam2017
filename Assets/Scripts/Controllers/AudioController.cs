using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// SOUND ID BANK
public enum SoundClipId
{
	ERROR,
	SFX_PADDLE_HIT,
	SFX_CORE_HIT,
	SFX_GAME_START,
	SFX_GAME_OVER,





	MUS_BACKGROUND_1,
	MUS_BACKGROUND_2,

	MUS_ERROR,
}
	
public class AudioController : MonoBehaviour
{
	Dictionary<SoundClipId, string> clips;

	private static readonly string PrefKey_MuteSFX = "muteSFX";
	private static readonly string PrefKey_MuteBGM = "muteBGM";

	static private bool _muteBGM = false;
	static private bool _muteSFX = false;

	private static AudioController singleton;

	static float bgmVol;

	string preveusBgMusic;
	string currentMusic;

	public AudioSource bgm;

	public static AudioController getSingleton ()
	{
		if (singleton == null) {
			
			GameObject go = new GameObject ("| AudioController |");
			singleton = go.AddComponent<AudioController> ();
			singleton.TryInit ();

		}
			
		return singleton;
	}

	void Start ()
	{

		if (singleton != this) {
			Debug.LogError ("Extra Audio Controller: " + gameObject.name, gameObject);
			Destroy (this);
		}
	}

	void TryInit ()
	{

		DontDestroyOnLoad (gameObject);

		/*
			 * CHECK USER PREFS
			 */
		_muteBGM = PlayerPrefsUtil.readBoolPref (PrefKey_MuteBGM, false); // isIntTrue(PlayerPrefs.GetInt("muteBGM" , 0));
		_muteSFX = PlayerPrefsUtil.readBoolPref (PrefKey_MuteSFX, false);



		if (_muteBGM) {
			bgmVol = 0.0f;
		} else {
			bgmVol = 1.0f;
		}


		this.SetupSoundClips ();
		singleton = this;
	}

	void SetupSoundClips ()
	{


		clips = new Dictionary<SoundClipId, string> ();
	
		clips.Add (SoundClipId.ERROR, "Audio/sfx/Paddle Hits a Good Object");
		clips.Add (SoundClipId.SFX_PADDLE_HIT, "Audio/sfx/Paddle_Hit-Bad-01a");
		clips.Add (SoundClipId.SFX_CORE_HIT, "Audio/sfx/Good_Particle_Get-01");
		clips.Add (SoundClipId.SFX_GAME_OVER, "Audio/sfx/GameOver");
		clips.Add (SoundClipId.SFX_GAME_START, "Audio/sfx/GameStart");




		clips.Add( SoundClipId.MUS_BACKGROUND_1, "Audio/music/SessionLoop");
		clips.Add( SoundClipId.MUS_BACKGROUND_2, "Audio/music/TitleScreenLoop");
	

	}

	public void muteSFX (bool setMuteSFX)
	{
		if (setMuteSFX == _muteSFX) {
			return;
		}
		//if(_muteSFX)
		_muteSFX = setMuteSFX;
		PlayerPrefsUtil.writeBoolPref (PrefKey_MuteSFX, _muteSFX);
		if (!_muteSFX) {
			// let user know sound effects are one
			AudioController.getSingleton ().PlaySFX ("Sounds/Tiny Button Push-SoundBible.com-513260752");
		}

		//sync ();
	}

	public void muteBGM (bool setMuteBGM)
	{
		if (setMuteBGM == _muteBGM) {
			return;
		}
		//if(_muteSFX)
		_muteBGM = setMuteBGM;
		PlayerPrefsUtil.writeBoolPref (PrefKey_MuteBGM, _muteBGM);
		if (!_muteBGM) {
			// let user know sound effects are one
			AudioController.getSingleton ().PlaySFX ("Sounds/Tiny Button Push-SoundBible.com-513260752");
		}

		sync ();
	}

	public bool getMuteSFX ()
	{
		return _muteSFX;
	}

	public bool getMuteBGM ()
	{
		return _muteBGM;
	}


	void OnDestroy ()
	{
		if (singleton == this) {
			singleton = null;
		}
	}

	public void PlaySFX (string url, float volume = 1.0f, float pitch = 1.0f, float delay = 0)
	{
 		StartCoroutine (CoroutinePlaySFX (url, volume, pitch, delay));
	}

	public void PlaySFX (SoundClipId id, float volume = 1.0f, float pitch = 1.0f, float delay = 0)
	{
		// Get url based on key
		string url = GetSoundClip (id);


		StartCoroutine (CoroutinePlaySFX (url, volume, pitch, delay));
	}

	public void PlaySFXIfOnScreen (SoundClipId id, GameObject go, float volume = 1.0f, float pitch = 1.0f, float delay = 0)
	{

		// Re-write?

	}

	public void PlaySFXRandomFromArray (string[] arrayOfUrls, float volume = 1.0f, float pitch = 1.0f, float delay = 0)
	{

		string name = arrayOfUrls [Random.Range (0, arrayOfUrls.Length)];
		AudioController.getSingleton ().PlaySFX (name, volume, pitch, delay);
	}

	string GetSoundClip (SoundClipId id)
	{

		string url;

		if (clips.ContainsKey (id)) {
			url = clips [id];
		} else {
			Debug.LogError ("Sound not found id= " + id);
			url = clips [SoundClipId.ERROR];
		}

		return url;
	}

/*	public SoundClipId GetWorldMusic (WorldType id)
	{

		SoundClipId url;

		if (worldMusic.ContainsKey (id)) {
			url = worldMusic [id];
		} else {
			Debug.LogError ("Sound not found id= " + id);
			url = SoundClipId.MUS_ERROR;
		}

		return url;
	}*/

	IEnumerator CoroutinePlaySFX (string url, float volume, float pitch, float delay)
	{
		//stuff
		yield return new WaitForSeconds (Time.timeScale * delay);

		if (_muteSFX) { 
			// It's muted
			//return;
		} else {
			
			AudioClip ac = Resources.Load (url) as AudioClip;

			if (ac == null) {

				Debug.Log ("MISSING Sound effect url");
				//return;
			} else {

				GameObject sfx = new GameObject (); // create the temp object
				sfx.transform.position = Vector3.zero; // set its position
				AudioSource aSource = sfx.AddComponent<AudioSource> (); // add an audio source
				aSource.clip = ac; // define the clip
				aSource.volume = volume;
				aSource.Play (); // start the sound
				aSource.pitch = pitch;
				DontDestroyOnLoad (sfx);
				Destroy (sfx, ac.length + 0.5f); // destroy object after clip duration
			}
		}
	}


	public string getPreveusBGMusic()
	{
		return preveusBgMusic;
	}

	//static GameObject this;
	public void PlayBGSoundClip (SoundClipId id, float volume = 1.0f, bool loop = true, float startTime = 0){

		string url = GetSoundClip (id);

		PlayBG(url,volume,loop, startTime );
	}


	public void PlayBG (string url, float volume = 1.0f, bool loop = true, float startTime = 0)
	{
		//if(bgm.clip != null)

		preveusBgMusic = currentMusic; 
		currentMusic = url;

		AudioClip ac = Resources.Load (url) as AudioClip;

		if (ac == null) {

			Debug.Log ("MISSING Sound effect url");
			return;
		}




		//bg = new GameObject(); // create the temp object
		//bg.transform.position = Vector3.zero; // set its position
		//bgm = bgm; // add an audio source
		if (bgm == null) {
			bgm = this.gameObject.AddComponent<AudioSource> (); 
		}


			// save preveus trake

		//}

		bgm.clip = ac; // define the clip
		bgm.loop = loop;
		bgm.time = startTime;
		bgm.volume = (_muteBGM) ? 0.0f : bgmVol;
		bgm.Play (); // start the sound
		//DontDestroyOnLoad(bg);

		//Destroy(sfx, ac.length + 0.5f); // destroy object after clip duration
	}



	public void PlayBGInterlude(SoundClipId id){

		PlaySoundWithCallback(id,AudioFinished);
	}

	Coroutine audioCallBackCoroutine = null;

	public delegate void AudioCallback();
	public void PlaySoundWithCallback(SoundClipId clipId, AudioCallback callback)
	{
		PlayBGSoundClip(clipId);

		if(audioCallBackCoroutine != null){
			StopCoroutine(audioCallBackCoroutine);
		}

		audioCallBackCoroutine = StartCoroutine(DelayedCallback(bgm.clip.length, callback));
	}
	private IEnumerator DelayedCallback(float time, AudioCallback callback)
	{
		yield return new WaitForSeconds(time);
		callback();
	}

	void AudioFinished()
	{

		//PlaySoundWithCallback( GetSoundIdForBackgroundMusic() ,AudioFinished);
	
		//GetSoundIdForBackgroundMusic


		Debug.Log("Audio Done!");
	}


	public void StopBG ()
	{
		if (bgm) {
			bgm.Stop ();
		}
		//if(bg){

		//Destroy(bg);
		//}
	}

	void sync ()
	{
		bgmVol = (_muteBGM) ? 0.0f : 1.0f;
		bgm.volume = bgmVol;
	}


}