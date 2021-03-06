// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class LoadBalancer
    {
        private AzureSubscription _AzureSubscription;
        private VirtualNetwork _AsmVirtualNetwork;
        private Subnet _AsmSubnet;
        private XmlNode _XmlNode;
        private string _TargetName = String.Empty;
        
        public LoadBalancer(AzureSubscription azureSubscription, VirtualNetwork asmVirtualNetwork, XmlNode loadBalancerXml)
        {
            this._AzureSubscription = azureSubscription;
            this._AsmVirtualNetwork = asmVirtualNetwork;
            this._XmlNode = loadBalancerXml;

            if (_AsmVirtualNetwork != null)
            {
                foreach (Subnet subnet in _AsmVirtualNetwork.Subnets)
                {
                    if (subnet.Name == this.SubnetName)
                    {
                        _AsmSubnet = subnet;
                        break;
                    }
                }
            }
        }

        public Subnet Subnet
        {
            get { return _AsmSubnet; }
        }

        public override string ToString()
        {
            return this.SubnetName;
        }

        private string SubnetName
        {
            get { return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText; }
        }

        public string Type
        {
            get { return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").InnerText; }
        }
    }
}

