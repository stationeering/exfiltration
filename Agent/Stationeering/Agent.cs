using UnityEngine;
using Assets.Scripts;

namespace Stationeering
{
    public class Agent : Object
    {
        public static void Exfiltrate()
        {
            if (GameManager.GameState == Assets.Scripts.GridSystem.GameState.Joining)
            {
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Exfiltration Agent Joining!");
            }

            if (GameManager.GameState == Assets.Scripts.GridSystem.GameState.Running)
            {
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Exfiltration Agent Running!");
            }
        }
    }
}
