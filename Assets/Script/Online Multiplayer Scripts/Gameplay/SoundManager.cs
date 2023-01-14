using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip ButtonClickSound, DiceRollSound, BuyCardSound, AttachLadderSound, AttachSnakeSound, GotLadderEffectSound, GotSnakeEffectSound, LadderDestroyedSound, SnakeDiedSound, WrongPlaceAttachItemSound, PlayerMove, PlayerWinSound;
    public static float sfxLength;
    static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        ButtonClickSound = Resources.Load<AudioClip>("Audio/ButtonClick");
        DiceRollSound = Resources.Load<AudioClip>("Audio/Dadu");
        BuyCardSound = Resources.Load<AudioClip>("Audio/jualbeli");
        AttachLadderSound = Resources.Load<AudioClip>("Audio/PasangTangga");
        AttachSnakeSound = Resources.Load<AudioClip>("Audio/PasangUlar");
        GotLadderEffectSound = Resources.Load<AudioClip>("Audio/Naik");
        GotSnakeEffectSound = Resources.Load<AudioClip>("Audio/turun");
        LadderDestroyedSound = Resources.Load<AudioClip>("Audio/TanggaHancur");
        SnakeDiedSound = Resources.Load<AudioClip>("Audio/UlarMati");
        WrongPlaceAttachItemSound = Resources.Load<AudioClip>("Audio/Salah");
        PlayerMove = Resources.Load<AudioClip>("Audio/jump");
        PlayerWinSound = Resources.Load<AudioClip>("Audio/win");

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySoundEffect(string clip)
    {
        switch(clip)
        {
            case "ButtonClick":
                audioSource.clip = ButtonClickSound;
                sfxLength = audioSource.clip.length;
                audioSource.Play();
                break;
            case "DiceRoll":
                audioSource.PlayOneShot(DiceRollSound);
                break;
            case "BuyCard":
                audioSource.PlayOneShot(BuyCardSound);
                break;
            case "AttachLadder":
                audioSource.PlayOneShot(AttachLadderSound);
                break;
            case "AttachSnake":
                audioSource.PlayOneShot(AttachSnakeSound);
                break;
            case "GotLadderEffect":
                audioSource.clip = GotLadderEffectSound;
                sfxLength = audioSource.clip.length;
                audioSource.Play();
                break;
            case "GotSnakeEffect":
                audioSource.clip = GotSnakeEffectSound;
                sfxLength = audioSource.clip.length;
                audioSource.Play();
                break;
            case "LadderDestroyed":
                audioSource.PlayOneShot(LadderDestroyedSound);
                break;
            case "SnakeDied":
                audioSource.PlayOneShot(SnakeDiedSound);
                break;
            case "WrongPlace":
                audioSource.PlayOneShot(WrongPlaceAttachItemSound);
                break;
            case "PlayerMove":
                audioSource.PlayOneShot(PlayerMove);
                break;
            case "PlayerWin":
                audioSource.PlayOneShot(PlayerWinSound);
                break;
            default:
                Debug.Log("Clip not found!");
                break;
        }
    }
}
