using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LogScreen : MonoBehaviour
{
    public Text consoleText;

    public DateTime time;
    public float logTimeout;

    private DateTime startGameTime;
    private DateTime startRealTime;

    public string[] sourceIPs =
    {
        "87.254.398.64",
        "164.345.81.73",
        "8.8.8.8",
        "167.294.345.15",
        "172.398.18.75"
    };
    public string[] destinationIPs =
    {
        "164.256.398.48",
        "1.208.147.193",
        "92.42.215.143",
        "92.42.31.254",
        "95.161.127.198"
    };
    public string[] protocol =
    {
        "tcp",
        "udp",
        "icmp"
    };

    public int consoleMaxLines = 21;

    int consoleLines = 0;
    IEnumerator Start()
    {
        startGameTime = new DateTime(2074, 11, 17, 21, 47, 39, 54);
        startRealTime = DateTime.Now;
        consoleText.text = "";

        while (true)
        {
            yield return new WaitForSeconds(logTimeout);
            if(consoleLines >= consoleMaxLines)
            {
                consoleLines = 0;
                consoleText.text = "";
            }
            var gameTimeNow = startGameTime + (DateTime.Now - startRealTime);
            var finalOut = 
           gameTimeNow.ToString() + " : " + 
           sourceIPs[Random.Range(0, sourceIPs.Length)] +" -> " + 
           destinationIPs[Random.Range(0, destinationIPs.Length)] + 
           " " + protocol[Random.Range(0, protocol.Length)] + " Success\n";
            consoleText.text += finalOut;
            consoleLines++;
        }
    }
}
