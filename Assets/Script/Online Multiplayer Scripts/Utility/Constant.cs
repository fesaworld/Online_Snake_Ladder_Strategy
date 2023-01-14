using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class Constant : MonoBehaviourPunCallbacks
{
    // Untuk login scene
    public static bool isGameStarted = false;

    // Untuk Roll Phase
    public static List<int> turnNumberAvailable4Players = new List<int>(new int[]{ 1,2,3,4 });
    public static List<int> turnNumberAvailable3Players = new List<int>(new int[]{ 1,2,3 });
    public static List<int> turnNumberAvailable2Players = new List<int>(new int[]{ 1,2 });

    // Untuk bagian Player Turn Manager
    public static string FirstTurnPlayerNickname = "Player 1";
    public static string SecondTurnPlayerNickname = "Player 2";
    public static string ThirdTurnPlayerNickname = "Player 3";
    public static string FourthTurnPlayerNickname = "Player 4";
}
