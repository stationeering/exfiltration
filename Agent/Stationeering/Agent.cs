using Assets.Scripts;
using Assets.Scripts.GridSystem;
using UnityEngine;

namespace Stationeering
{
    public class Agent
    {
        public static void Exfiltrate()
        {
            if (GameManager.GameState == (GameManager.IsServer ? GameState.Joining : GameState.Running))
            {
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Exfiltration Agent Starting...");

                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Outputting LogicTypes...");
                Task.ExfilrateLogicTypes.Exfiltrate();
            }
        }
    }
}
