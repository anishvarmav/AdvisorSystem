using AdvisorSystem.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdvisorControllerTests
{
    private AdvisorRepository CreateRepository(string dbName)
    {
        DbContextOptions<AdvisorDbContext> options = new DbContextOptionsBuilder<AdvisorDbContext>().UseInMemoryDatabase(dbName).Options;
        AdvisorDbContext context = new AdvisorDbContext(options);
        AdvisorCache<int, Advisor> cache = new AdvisorCache<int, Advisor>(5);

        return new AdvisorRepository(context, cache);
    }

    [Fact]
    public void CheckIfAllAdvisorsReturned()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfAllAdvisorsReturned"));
        Advisor advisor1 = new Advisor { Name = "John Doe 1", SIN = "123456787", Address = "122 Main Street, Toronto, ON", Phone = "12345677" };
        Advisor advisor2 = new Advisor { Name = "Jane Doe 1", SIN = "123456788", Address = "123 Main Street, Toronto, ON", Phone = "12345678" };
        Advisor advisor3 = new Advisor { Name = "Jack Doe 1", SIN = "123456789", Address = "124 Main Street, Toronto, ON", Phone = "12345679" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);
        controller.CreateAdvisor(advisor3);

        ActionResult<List<Advisor>> result = controller.GetAdvisors();

        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.Count);
        Assert.Equal("John Doe 1", result.Value[0].Name);
        Assert.Equal("Jane Doe 1", result.Value[1].Name);
        Assert.Equal("Jack Doe 1", result.Value[2].Name);

    }

    [Fact]
    public void CheckIfCorrectAdvisorReturnedById()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfCorrectAdvisorReturnedById"));
        Advisor advisor1 = new Advisor { Name = "John Doe 2", SIN = "123456787", Address = "122 Main Street, Victoria, BC", Phone = "12345677" };
        Advisor advisor2 = new Advisor { Name = "Jane Doe 2", SIN = "123456788", Address = "123 Main Street, Victoria, BC", Phone = "12345678" };
        Advisor advisor3 = new Advisor { Name = "Jack Doe 2", SIN = "123456789", Address = "124 Main Street, Victoria, BC", Phone = "12345679" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);
        controller.CreateAdvisor(advisor3);

        ActionResult<Advisor> result = controller.GetAdvisor(1);
        Assert.NotNull(result.Value);
        Assert.Equal("John Doe 2", result.Value.Name);
    }

    [Fact]
    public void CheckIfReturnNotFoundForAdvisorById()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfReturnNotFoundForAdvisorById"));
        Advisor advisor1 = new Advisor { Name = "John Doe 3", SIN = "123456787", Address = "122 Main Street, Calgary, AB", Phone = "12345677" };
        Advisor advisor2 = new Advisor { Name = "Jane Doe 3", SIN = "123456788", Address = "123 Main Street, Calgary, AB", Phone = "12345678" };
        Advisor advisor3 = new Advisor { Name = "Jack Doe 3", SIN = "123456789", Address = "124 Main Street, Calgary, AB", Phone = "12345679" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);
        controller.CreateAdvisor(advisor3);

        ActionResult<Advisor> result = controller.GetAdvisor(4);
        Assert.True(result.Value == null);
    }

    [Fact]
    public void CheckIfNewAdvisorCreated()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfNewAdvisorCreated"));
        Advisor advisor1 = new Advisor { Name = "Jack Reacher 4", SIN = "000000000", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };

        ActionResult<Advisor?> result = controller.CreateAdvisor(advisor1);
        Assert.NotNull(result.Value);
        Assert.Equal("Jack Reacher 4", result.Value.Name);
        Assert.Contains<string>(result.Value.HealthStatus, new List<string> { "Green", "Yellow", "Red" });
    }

    [Fact]
    public void CheckIfNewAdvisorCreatedWithSameId()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfNewAdvisorCreatedWithSameId"));
        Advisor advisor1 = new Advisor { Id = 1, Name = "Jack Reacher 5", SIN = "000000000", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };
        Advisor advisor2 = new Advisor { Id = 1, Name = "Jack Reacher 5", SIN = "000000001", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);

        ActionResult<Advisor> result = controller.GetAdvisor(2);
        Assert.Null(result.Value);
    }

    [Fact]
    public void CheckIfNewAdvisorCreatedWithSameSIN()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfNewAdvisorCreatedWithSameSIN"));
        Advisor advisor1 = new Advisor { Name = "Jack Reacher 6", SIN = "000000000", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };
        Advisor advisor2 = new Advisor { Name = "Jack Reacher 6", SIN = "000000000", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);

        ActionResult<Advisor> result = controller.GetAdvisor(2);
        Assert.Null(result.Value);
    }

    [Fact]
    public void CheckIfAdvisorUpdated()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfAdvisorUpdated"));
        Advisor advisor1 = new Advisor { Name = "John Doe 7", SIN = "123456787", Address = "122 Main Street, Montreal, QC", Phone = "12345677" };
        Advisor advisor2 = new Advisor { Name = "Jane Doe 7", SIN = "123456788", Address = "123 Main Street, Montreal, QC", Phone = "12345678" };
        Advisor advisor3 = new Advisor { Name = "Jack Doe 7", SIN = "123456789", Address = "124 Main Street, Montreal, QC", Phone = "12345679" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);
        controller.CreateAdvisor(advisor3);

        advisor2 = new Advisor { Name = "Jane Doe 7", SIN = "123456788", Address = "123 Main Street, Montreal, QC", Phone = "00000000", HealthStatus = "Red" };
        ActionResult<Advisor?> result = controller.UpdateAdvisor(2, advisor2);
        Assert.NotNull(result.Value);
        Assert.Equal("00000000", result.Value.Phone);
        Assert.Equal("Red", result.Value.HealthStatus);
    }

    [Fact]
    public void CheckIfAdvisorUpdatedWithDifferentId()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfAdvisorUpdatedWithDifferentId"));
        Advisor advisor1 = new Advisor { Name = "John Doe 8", SIN = "123456787", Address = "122 Main Street, Montreal, QC", Phone = "12345677" };
        controller.CreateAdvisor(advisor1);

        advisor1 = new Advisor { Name = "Jane Doe 8", SIN = "123456787", Address = "122 Main Street, Montreal, QC", Phone = "00000000" };
        ActionResult<Advisor?> result = controller.UpdateAdvisor(2, advisor1);
        Assert.Null(result.Value);
    }

    [Fact]
    public void CheckIfAdvisorUpdatedWithDuplicateSIN()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfAdvisorUpdatedWithDuplicateSIN"));
        Advisor advisor1 = new Advisor { Name = "John Doe 9", SIN = "123456787", Address = "122 Main Street, Montreal, QC", Phone = "12345677" };
        Advisor advisor2 = new Advisor { Name = "Jane Doe 9", SIN = "123456788", Address = "123 Main Street, Montreal, QC", Phone = "12345678" };
        Advisor advisor3 = new Advisor { Name = "Jack Doe 9", SIN = "123456789", Address = "124 Main Street, Montreal, QC", Phone = "12345679" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);
        controller.CreateAdvisor(advisor3);

        advisor2 = new Advisor { Name = "Jane Doe 9", SIN = "123456789", Address = "123 Main Street, Toronto, ON", Phone = "00000000" };
        ActionResult<Advisor?> result = controller.UpdateAdvisor(2, advisor2);
        Assert.Null(result.Value);
    }

    [Fact]
    public void CheckIfAdvisorDeleted()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfAdvisorDeleted"));
        Advisor advisor1 = new Advisor { Name = "Jack Reacher 10", SIN = "000000000", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };
        Advisor advisor2 = new Advisor { Name = "Jack Reacher 10", SIN = "000000001", Address = "2 West Lane, Oshawa, ON", Phone = "00000001" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);

        ActionResult<bool> result = controller.DeleteAdvisor(2);
        Assert.True(result.Value);

        ActionResult<Advisor> advisorRecord = controller.GetAdvisor(2);
        Assert.Null(advisorRecord.Value);
    }

    [Fact]
    public void CheckIfNoAdvisorToDelete()
    {
        AdvisorController controller = new AdvisorController(CreateRepository("CheckIfNoAdvisorToDelete"));
        Advisor advisor1 = new Advisor { Name = "John Doe 11", SIN = "000000000", Address = "2 West Lane, Oshawa, ON", Phone = "00000000" };
        Advisor advisor2 = new Advisor { Name = "John Doe 11", SIN = "000000001", Address = "2 West Lane, Oshawa, ON", Phone = "00000001" };
        controller.CreateAdvisor(advisor1);
        controller.CreateAdvisor(advisor2);

        ActionResult<bool> result = controller.DeleteAdvisor(3);
        Assert.False(result.Value);
    }
}
