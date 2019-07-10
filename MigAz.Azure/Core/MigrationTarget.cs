// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Core
{
    public abstract class MigrationTarget
    {
        Guid _MigrationTargetGuid = Guid.NewGuid();
        String _ApiVersion = String.Empty;
        String _ProviderNamespace = String.Empty;
        String _ResourceType = String.Empty;
        ILogProvider _LogProvider = null;

        private MigrationTarget() { }

        public MigrationTarget(string providerNamespace, string resourceType, ILogProvider logProvider)
        {
            _ProviderNamespace = providerNamespace;
            _ResourceType = resourceType;
            _LogProvider = logProvider;
        }

        public Guid MigrationTargetGuid { get { return _MigrationTargetGuid; } }

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
            set { _LogProvider = value; }
        }

        public void WriteLog(string function, string message)
        {
            if (this.LogProvider != null)
            {
                this.LogProvider.WriteLog(function, message);
            }
            else
            {
                MessageBox.Show(message, function);
            }
        }

        public string ApiVersion 
        {
            get { return _ApiVersion; }
            set { _ApiVersion = value; }
        }

        public bool DefaultApiVersion(AzureSubscription targetSubscription)
        {
            if (this.ApiVersion != null && this.ApiVersion.Length > 0)
                return false;

            if (targetSubscription == null)
                return false;

            Arm.ProviderResourceType providerResourceType = targetSubscription.GetProviderResourceType(this.ProviderNamespace, this.ResourceType);

            if (providerResourceType == null)
                return false;

            this.ApiVersion = providerResourceType.GetMaxApiVersionMigAzTested();

            return this.ApiVersion != null && this.ApiVersion.Length > 0; // Success if has Version
        }

        public string ProviderNamespace { get { return _ProviderNamespace; } }
        public string ResourceType { get { return _ResourceType; } }

        public string TargetName { get; set; }
        public string TargetNameResult { get; set; }
        public abstract void SetTargetName(string targetName, TargetSettings targetSettings);

        public override string ToString()
        {
            return this.TargetNameResult;
        }

        public abstract string ImageKey { get; }
        public abstract string FriendlyObjectName { get; }

        public static String GetImageKey(Type migrationType)
        {
            return migrationType.ToString().Substring(migrationType.ToString().LastIndexOf(".") + 1);
        }
    }
}

