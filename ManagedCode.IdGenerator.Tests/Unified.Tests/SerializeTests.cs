using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using ManagedCode.IdGenerator.Unified;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ManagedCode.IdGenerator.Tests.Unified.Tests;

public class SerializeTests
{
    [Fact]
    public void NewtonsoftDefaultTest()
    {
        var contract = new TestContract { Id = UnifiedId.Empty };
        var json = JsonConvert.SerializeObject(contract);
        Assert.Equal("{\"Id\":{\"hash\":0}}", json);

        var deserialized = JsonConvert.DeserializeObject<TestContract>(json);
        Assert.Equal(contract.Id, deserialized.Id);
    }

    /*[Fact]
    public void NewtonsoftConvertorTest()
    {
        var contract = new TestContract { Id = UnifiedId.Empty };
        var settings = new JsonSerializerSettings
        { 
            Converters = new List<JsonConverter> 
            { 
                new UnifiedIdConverter() 
            }
        };
        var json = JsonConvert.SerializeObject(contract, settings);
        Assert.Equal("{\"Id\":\"0000000000000\"}", json);

        var deserialized = JsonConvert.DeserializeObject<TestContract>(json, settings);
        Assert.Equal(contract.Id, deserialized?.Id);
    }*/

    /*[Fact]
    public void DataContractTest()
    {
        var contract = new TestContract { Id = new UnifiedId(8U) };
        var dcs = new DataContractSerializer(typeof(TestContract));
        using (var ms = new MemoryStream())
        {
            dcs.WriteObject(ms, contract);
            var arr = ms.ToArray();
            var txt = Encoding.UTF8.GetString(arr);
            Assert.Equal(358, arr.Length);

            ms.Position = 0;
            var deserialized = (TestContract)dcs.ReadObject(ms);
            Assert.Equal(contract.Id, deserialized.Id);
        }
    }*/

    /*[Fact]
     public void BinaryFormatterTest()
     {
         var contract = new TestContract { Id = new UnifiedId(8U) };
         var bf = new BinaryFormatter();
         using(var ms = new MemoryStream())
         {
             bf.Serialize(ms, contract);
             var arr = ms.ToArray();
             Assert.Equal(289, arr.Length);

             ms.Position = 0;
             var deserialized = (TestContract)bf.Deserialize(ms);
             Assert.Equal(contract.Id, deserialized.Id);
         }

         var info = new SerializationInfo(typeof(UnifiedId), new FormatterConverter());
         contract.Id.GetObjectData(info, default);
         var hash = info.GetUInt64("hash");
         Assert.Equal(contract.Id.ToUInt64(), hash);
     }*/

    [Fact]
    public void SystemTextTest()
    {
        var contract = new TestContract { Id = new UnifiedId(8U) };

        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnifiedIdSystemJsonConverter());

        var json = JsonSerializer.Serialize(contract, options);
        Assert.Equal("{\"Id\":\"0000000000008\"}", json);

        var deserialized = JsonSerializer.Deserialize<TestContract>(json, options);
        Assert.Equal(contract.Id, deserialized.Id);
    }
}