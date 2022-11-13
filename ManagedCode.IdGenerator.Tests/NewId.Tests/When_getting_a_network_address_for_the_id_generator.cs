using System;
using System.Linq;
using System.Net.NetworkInformation;
using ManagedCode.IdGenerator.NewId.NewIdProviders;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.NewId.Tests;

public class When_getting_a_network_address_for_the_id_generator
{
    [Fact]
    public void Should_pull_all_adapters()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .ToArray();

        foreach (var networkInterface in interfaces)
        {
            Console.WriteLine("Network Interface: {0} - {1}", networkInterface.Description,
                networkInterface.NetworkInterfaceType);
        }
    }

    [Fact]
    public void Should_pull_the_network_adapter_mac_address()
    {
        var networkIdProvider = new NetworkAddressWorkerIdProvider();

        var networkId = networkIdProvider.GetWorkerId(0);

        Assert.NotNull(networkId);
        Assert.Equal(6, networkId.Length);
    }

    [Fact]
    public void Should_pull_using_host_name()
    {
        var networkIdProvider = new HostNameHashWorkerIdProvider();

        var networkId = networkIdProvider.GetWorkerId(0);

        Assert.NotNull(networkId);
        Assert.Equal(6, networkId.Length);
    }

    [Fact]
    public void Should_pull_using_net()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                        x.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet ||
                        x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        x.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx ||
                        x.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT
            )
            .ToArray();

        foreach (var networkInterface in interfaces)
        {
            Console.WriteLine("Network Interface: {0}", networkInterface.Description);
        }
    }
}