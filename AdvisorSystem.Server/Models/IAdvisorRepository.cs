public interface IAdvisorRepository
{
    public List<Advisor> GetAdvisors();
    public Advisor? GetAdvisorById(int id);
    public Advisor? CreateAdvisor(Advisor advisor);
    public Advisor? UpdateAdvisor(int id, Advisor advisor);
    public bool DeleteAdvisor(int id);
}