using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Stationeering.Task;
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
                ExfiltrateLogicTypes.Exfiltrate();
                
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Outputting LogicSlotTypes...");
                ExfiltrateLogicSlotTypes.Exfiltrate();
                
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Outputting Instructions...");
                ExfiltrateInstructions.Exfiltrate();
                
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Outputting Things...");
                ExfiltrateThings.Exfiltrate();
            }
        }
    }
}
