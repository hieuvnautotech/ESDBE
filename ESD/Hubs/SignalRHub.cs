using Microsoft.AspNetCore.SignalR;
using ESD.Services.Common;
using ESD.Services.MMS;

namespace ESD.Hubs
{
    public class SignalRHub : Hub
    {

        private readonly ILoginService _loginService;
        private readonly IVersionAppService _versionAppService;
        private readonly IAnalyticsService _analyticsService;
        public SignalRHub(ILoginService loginService, IVersionAppService versionAppService, IAnalyticsService analyticsService)
        {
            _loginService = loginService;
            _versionAppService = versionAppService;
            _analyticsService = analyticsService;
        }


        public async Task SendOnlineUsers()
        {
            var res = await _loginService.GetOnlineUsers();
            if (Clients != null)
                await Clients.All.SendAsync("ReceivedOnlineUsers", res.Data);
        }

        public async Task SendAppVersion()
        {
            var res = await _versionAppService.GetAll();
            if (Clients != null)
                await Clients.All.SendAsync("ReceivedAppVersion", res.Data);
        }

        public async Task GetDisplayWO()
        {
            var res = await _analyticsService.GetDisplay();
            if (Clients != null)
                await Clients.All.SendAsync("WorkOrderGetDisplay", res.Data);
        }
    }
}
