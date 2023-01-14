using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static int WaypointIndexContainPlayer = 0;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag.Equals("Player")) {
            if (this.gameObject.name == "Waypoint") {
                WaypointIndexContainPlayer = 1;
            } else {
                    string resultString = Regex.Match(this.gameObject.name, @"\d+").Value;
                    WaypointIndexContainPlayer = int.Parse(resultString) + 1;
            }
        }
    }
}
