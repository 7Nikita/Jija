using Microsoft.AspNetCore.Components.Server;

namespace Jija.Services
{
    public class StateChangedService : ServerAuthenticationStateProvider
    {
        public StateChangedService() : base()
        {
        }

        public void NotifyStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}