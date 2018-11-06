using Assets.Scripts;
using Assets.Scripts.GridSystem;
using UnityEngine;
using Stationeering.Task;

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
                ExfilrateLogicTypes.Exfiltrate();
                
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Outputting LogicSlotTypes...");
                ExfilrateLogicSlotTypes.Exfiltrate();
                
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Outputting Instructions...");
                ExfilrateInstructions.Exfiltrate();
            }
        }
    }
}
