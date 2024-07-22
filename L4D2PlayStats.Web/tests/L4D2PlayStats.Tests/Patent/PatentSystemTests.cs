using FluentAssertions;
using L4D2PlayStats.Patent;

namespace L4D2PlayStats.Tests.Patent;

[TestClass]
public class PatentSystemTests
{
    [TestMethod]
    public void GetPatentTest()
    {
        PatentSystem.GetPatent(0).Level.Should().Be(1);
        PatentSystem.GetPatent(100).Level.Should().Be(1);

        PatentSystem.GetPatent(150).Level.Should().Be(2);
        PatentSystem.GetPatent(199).Level.Should().Be(2);

        PatentSystem.GetPatent(200).Level.Should().Be(3);
        PatentSystem.GetPatent(249).Level.Should().Be(3);

        PatentSystem.GetPatent(4999).Level.Should().Be(14);

        PatentSystem.GetPatent(5000).Level.Should().Be(15);
        PatentSystem.GetPatent(100_000).Level.Should().Be(15);
    }

    [TestMethod]
    public void GetNextPatent()
    {
        PatentSystem.GetNextPatent(0)!.Level.Should().Be(2);
        PatentSystem.GetNextPatent(100)!.Level.Should().Be(2);

        PatentSystem.GetNextPatent(150)!.Level.Should().Be(3);
        PatentSystem.GetNextPatent(199)!.Level.Should().Be(3);

        PatentSystem.GetNextPatent(200)!.Level.Should().Be(4);
        PatentSystem.GetNextPatent(249)!.Level.Should().Be(4);

        PatentSystem.GetNextPatent(4999)!.Level.Should().Be(15);

        PatentSystem.GetNextPatent(5000).Should().BeNull();
        PatentSystem.GetNextPatent(100_000).Should().BeNull();
    }
}