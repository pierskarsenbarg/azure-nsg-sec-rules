using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Network;

return await Pulumi.Deployment.RunAsync(() =>
{
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("pk-nsg-rg");

    var nsg2 = new NetworkSecurityGroup("NSG2", new NetworkSecurityGroupArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,

        SecurityRules = new Pulumi.AzureNative.Network.Inputs.SecurityRuleArgs()
        {
            Name = "NSG2-AllowVirtualNetworkRdpInbound-inline",
            Priority = 120,
            Direction = SecurityRuleDirection.Inbound,
            Access = SecurityRuleAccess.Allow,
            Protocol = SecurityRuleProtocol.Tcp,
            SourcePortRange = "*",
            DestinationPortRange = "3389",
            SourceAddressPrefix = "VirtualNetwork",
            DestinationAddressPrefix = "VirtualNetwork",
            Description = "Allows internal RDP Traffic"
        }
    });

     _ = new SecurityRule($"NSG-AllowVirtualNetworkRdpInbound-resource", new SecurityRuleArgs
    {
        Priority = 121,
        Direction = SecurityRuleDirection.Inbound,
        Access = SecurityRuleAccess.Allow,
        Protocol = SecurityRuleProtocol.Tcp,
        SourcePortRange = "*",
        DestinationPortRange = "3388",
        SourceAddressPrefix = "VirtualNetwork",
        DestinationAddressPrefix = "VirtualNetwork",
        ResourceGroupName = resourceGroup.Name,
        NetworkSecurityGroupName = nsg2.Name,
        Description = "Allows internal RDP Traffic"
    });
});