using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip Porta;
    public AudioClip LoadingMusic;
    public AudioClip Fogueira;
    public AudioClip MouseClick;
    public AudioClip PassosForaCasa;

    public void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

}
