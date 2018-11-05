using UnityEngine;
using Assets.Scripts;

namespace Stationeering
{
    public class Agent : UnityEngine.Object
    {
        public static void Exfiltrate()
        {
            if (GameManager.GameState == Assets.Scripts.GridSystem.GameState.Joining)
            {
                Debug.Log("STATIONEERINGEXFILTRATION:LOG:Exfiltration Agent Starting as Server is now Joining!");
            
                ExfiltrateLogicTypes();               
            }
        }

        private static void ExfiltrateLogicTypes()
        {
        }
    }
}
