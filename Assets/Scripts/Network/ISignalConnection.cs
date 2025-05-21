using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public abstract class SignalConnectorBase : MonoBehaviour
{
    public abstract Task<HubConnection> GetConnection();
    public abstract void SendShips(int fromId, int toId, int amount);
}
