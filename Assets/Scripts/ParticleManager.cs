using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public List<GameObject> _explosions = new List<GameObject>();
    public List<GameObject> _jumps = new List<GameObject>();
    public List<GameObject> _sustained = new List<GameObject>();

    // Singleton instance.
    public static ParticleManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayRandomExplosionEffect(Vector2 position, Transform parent)
    {
        PlayEffect(_explosions, Random.Range(0, _explosions.Count), position);
    }

    public void PlayJumpEffect(int index, Vector2 position, Transform paren, float size = 1)
    {
        PlayEffect(_jumps, index, position, size:size, jump:true);
    }

    public void PlayEffect(List<GameObject> _effects, int index, Vector2 position, Transform parent = null, float timeDelay = 0, float size = 1, bool jump = false)
    {
        StartCoroutine(Play(_effects[index], position, parent, timeDelay, size, jump));
    }

    // Play a single clip through the sound effects source.
    IEnumerator Play(GameObject clip, Vector2 position, Transform parent, float timeDelay, float size, bool jump)
    {
        yield return new WaitForSeconds(timeDelay);

        GameObject particleObject = Instantiate(clip, parent);
        particleObject.transform.parent = parent;
        particleObject.transform.position = position;
        particleObject.name = clip.name + "Particle";
        particleObject.transform.localScale = Vector3.one * size;
        if (jump)
            particleObject.AddComponent<Rigidbody2D>().velocity = Vector2.down * 2;

        StartCoroutine(DestroyAudioSource(particleObject, 2f));
    }

    IEnumerator DestroyAudioSource(GameObject audioObject, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(audioObject);
    }
}
