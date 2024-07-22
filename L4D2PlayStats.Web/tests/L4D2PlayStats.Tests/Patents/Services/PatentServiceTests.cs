using FluentAssertions;
using L4D2PlayStats.Patents.Services;

namespace L4D2PlayStats.Tests.Patents.Services;

[TestClass]
public class PatentServiceTests
{
    private readonly PatentService _patentService = new();

    [TestMethod]
    public void GetPatentTest()
    {
        _patentService.GetPatent(0).Level.Should().Be(1);
        _patentService.GetPatent(19).Level.Should().Be(1);

        _patentService.GetPatent(20).Level.Should().Be(2);
        _patentService.GetPatent(99).Level.Should().Be(2);

        _patentService.GetPatent(100).Level.Should().Be(3);
        _patentService.GetPatent(219).Level.Should().Be(3);

        _patentService.GetPatent(4999).Level.Should().Be(14);

        _patentService.GetPatent(5000).Level.Should().Be(15);
        _patentService.GetPatent(100_000).Level.Should().Be(15);
    }

    [TestMethod]
    public void GetNextPatent()
    {
        _patentService.GetNextPatent(0)!.Level.Should().Be(2);
        _patentService.GetNextPatent(19)!.Level.Should().Be(2);

        _patentService.GetNextPatent(20)!.Level.Should().Be(3);
        _patentService.GetNextPatent(99)!.Level.Should().Be(3);

        _patentService.GetNextPatent(100)!.Level.Should().Be(4);
        _patentService.GetNextPatent(219)!.Level.Should().Be(4);

        _patentService.GetNextPatent(4999)!.Level.Should().Be(15);

        _patentService.GetNextPatent(5000).Should().BeNull();
        _patentService.GetNextPatent(100_000).Should().BeNull();
    }
}