using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace TDIDMay2014
{
    [HubName("statusFeedHub")]
    public class StatusFeedHub : Hub
    {
        /// <summary>
        /// The list of connected users.
        /// </summary>
        public static List<string> Users = new List<string>();

        public void Send(string name, string message)
        {
            string userName = Context.User.Identity.GetUserName();
            int emailCount = DateTime.Now.TimeOfDay.Seconds;
            Clients.All.broadcastMessage(userName, message, emailCount);
        }

        /// <summary>
        /// Register a workflow task
        /// </summary>
        /// <param name="status">What you're doing: 'Started' or 'Completed'</param>
        /// <param name="type">They type of workflow activity: ie, 'Task'</param>
        /// <param name="name">The name of the item you're working on.</param>
        public void SendWorkflowItem(string status, string type, string name)
        {
            string userName = Context.User.Identity.GetUserName();
            string message = status + " " + type + " " + name;
            Clients.All.broadcastMessage(userName, message);
        }

        /// <summary>
        /// Sends the user list to the listening view on startup.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void GetUserList()
        {
            // Call the addNewMessageToPage method to update clients.
            //var context = GlobalHost.ConnectionManager.GetHubContext<StatusFeedHub>();
            //context.Clients.All.updateUsersOnlineCount(Users);
            Clients.All.updateOnlineUsersList(Users);
        }

        /// <summary>
        /// Sends the update user count to the listening view.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void SendUserList()
        {
            // Call the addNewMessageToPage method to update clients.
            //var context = GlobalHost.ConnectionManager.GetHubContext<StatusFeedHub>();
            //context.Clients.All.updateUsersOnlineCount(Users);
            Clients.All.updateOnlineUsersList(Users);
        }

        /// <summary>
        /// The OnConnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            string clientName = GetClientName();

            if (Users.IndexOf(clientName) == -1)
            {
                Users.Add(clientName);
            }

            // Send the current list of users
            SendUserList();

            return base.OnConnected();
        }

        /// <summary>
        /// The OnReconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            string clientName = GetClientName();
            if (Users.IndexOf(clientName) == -1)
            {
                Users.Add(clientName);
            }

            // Send the current list of users
            SendUserList();

            return base.OnReconnected();
        }

        /// <summary>
        /// The OnDisconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnDisconnected()
        {
            string clientName = GetClientName();

            if (Users.IndexOf(clientName) > -1)
            {
                Users.Remove(clientName);
            }

            // Send the current list of users
            SendUserList();

            return base.OnDisconnected();
        }

        /// <summary>
        /// Get's the currently connected Id of the client.
        /// This is unique for each client and is used to identify
        /// a connection.
        /// </summary>
        /// <returns>The client Id.</returns>
        private string GetClientName()
        {
            string clientName = "";
            if (Context.QueryString["clientName"] != null)
            {
                // clientName passed from application 
                clientName = this.Context.QueryString["clientName"];
            }

            if (string.IsNullOrEmpty(clientName.Trim()))
            {
                clientName = Context.User.Identity.Name;
            }

            return clientName;
        }

    }
}