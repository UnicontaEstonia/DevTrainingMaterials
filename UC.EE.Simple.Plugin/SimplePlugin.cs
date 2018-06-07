// ***********************************************************************
// Assembly         : UC.EE.Simple.Plugin
// Author           : Jaan
// Created          : 5/23/2018 11:35:11 PM
//
// Last Modified By : Jaan
// Last Modified On : 5/23/2018 11:35:11 PM
// ***********************************************************************
// <copyright file="UC.EE.Simple.Plugin.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary>UC.EE.Simple.Plugin Plugin for Uniconta</summary>
// ***********************************************************************

namespace UC.EE.Simple.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Uniconta.API.Plugin;
    using Uniconta.API.Service;
    using Uniconta.API.System;
    using Uniconta.ClientTools.DataModel;
    using Uniconta.Common;

    /// <summary>
    /// Class UC.EE.Simple.Plugin.
    /// </summary>
    /// <seealso cref="Uniconta.API.Plugin.IPluginBase" />
    public class SimplePlugin : IPluginBase
    {
        /// <summary>
        /// The crud API holds the api for further use
        /// </summary>
        private CrudAPI crudApi;
        /// <summary>
        /// The error description to set it and use it in GetErrorDescription method
        /// </summary>
        private string errorDescription = "An unexpected error occurred in the UC.EE.Simple.Plugin plugin.";

        /// <summary>
        /// Gets the error description.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetErrorDescription()
        {
            return this.errorDescription;
        }

        /// <summary>
        /// Actual execution of this plugin.
        /// </summary>
        /// <param name="master">The master.</param>
        /// <param name="currentRow">The current row.</param>
        /// <param name="source">The source.</param>
        /// <param name="command">The command.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>ErrorCodes.</returns>
        public ErrorCodes Execute(
            UnicontaBaseEntity master,
            UnicontaBaseEntity currentRow,
            IEnumerable<UnicontaBaseEntity> source,
            string command,
            string args)
        {
            //TODO: Write code here. Remember to set and use errorDescription.
            if (currentRow == null)
            {
                errorDescription = "Row has to be selected";
                return ErrorCodes.NoLinesToUpdate;
            }
            var selectedItem = currentRow as InvItemClient;
            if (selectedItem == null)
            {
                errorDescription = "Selected row has to be item type";
                return ErrorCodes.InvalidObject;
            }

            if (selectedItem.CostPrice > selectedItem.SalesPrice1)
            {
                selectedItem.SalesPrice1 = selectedItem.CostPrice * 1.14;
                //crudApi.Update(selectedItem);
                errorDescription = String.Empty;
            }
            else
            {
                MessageBox.Show("No changes were made.");
            }
            return ErrorCodes.Succes;
        }

        /// <summary>
        /// Setting the master(s) for the entity
        /// </summary>
        /// <param name="masters">The masters.</param>
        public void SetMaster(List<UnicontaBaseEntity> masters)
        {
        }

        /// <summary>
        /// Sets the API to Query the Uniconta database. This is already done within the Uniconta client.
        /// </summary>
        /// <param name="api">The api that is being used.</param>
        public void SetAPI(BaseAPI api)
        {
            this.crudApi = api as CrudAPI;
        }

        /// <summary>
        /// Initializes this instance so it should be used to initialize any values. After this the <seealso cref="OnExecute" /> is called.
        /// </summary>
        public void Intialize()
        {
        }

        /// <summary>
        /// Gets the name(s) of any dependent assembly / assemblies.
        /// </summary>
        /// <returns>System.String[].</returns>
        public string[] GetDependentAssembliesName()
        {
            return null;
        }

        /// <summary>
        /// Gets the name of this plugin. This will also show up within Uniconta.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "UC.EE.Simple.Plugin";

        /// <summary>
        /// Occurs when [on execute].
        /// </summary>
        public event EventHandler OnExecute;
    }
}
