public class AdvisorRepository : IAdvisorRepository
{
    private readonly AdvisorDbContext _context;
    private readonly AdvisorCache<int, Advisor> _advisorCache;
    public AdvisorRepository(AdvisorDbContext context, AdvisorCache<int, Advisor> advisorCache)
    {
        _context = context;
        _advisorCache = advisorCache;
    }

    public List<Advisor> GetAdvisors()
    {
        List<Advisor> advisorList = _context.Advisors.ToList();
        foreach (Advisor advisor in advisorList)
        {
            _advisorCache.Put(advisor.Id, advisor);
        }
        return _context.Advisors.ToList();
    }

    public Advisor? GetAdvisorById(int id)
    {
        Advisor? cachedAdvisor = _advisorCache.Get(id);
        if (cachedAdvisor != null)
        {
            return cachedAdvisor;
        }

        Advisor? currentAdvisor = _context.Advisors.Find(id);
        if (currentAdvisor != null)
        {
            _advisorCache.Put(id, currentAdvisor);
        }

        return currentAdvisor;
    }

    public Advisor? CreateAdvisor(Advisor advisor)
    {
        Advisor? currentAdvisor = _context.Advisors.Find(advisor.Id);

        //Unique index is not working for in-memory database, so using below checks to determine create
        Advisor? sinAdvisor = _context.Advisors.Where(x => x.SIN == advisor.SIN).FirstOrDefault();

        if ((currentAdvisor != null) || (currentAdvisor == null && sinAdvisor != null))
        {
            return null;
        }
        else
        {
            if (string.IsNullOrEmpty(advisor.HealthStatus))
            {
                advisor.HealthStatus = Advisor.GenerateHealthStatus();
            }

            _context.Advisors.Add(advisor);
            _advisorCache.Put(advisor.Id, advisor);
            _context.SaveChanges();
            return advisor;
        }
    }

    public Advisor? UpdateAdvisor(int id, Advisor advisor)
    {
        Advisor? currentAdvisor = _context.Advisors.Find(id);

        //Unique index is not working for in-memory database. So using below checks to determine update
        Advisor? sinAdvisor = _context.Advisors.Where(x => x.SIN == advisor.SIN).FirstOrDefault();

        if ((currentAdvisor == null) || (currentAdvisor != null && currentAdvisor.SIN != advisor.SIN && sinAdvisor != null))
        {
            return null;
        }
        else
        {
            currentAdvisor.Name = advisor.Name;
            currentAdvisor.SIN = advisor.SIN;
            currentAdvisor.Address = advisor.Address;
            currentAdvisor.Phone = advisor.Phone;
            currentAdvisor.HealthStatus = advisor.HealthStatus;

            _advisorCache.Put(id, currentAdvisor);
            _context.SaveChanges();
            return currentAdvisor;
        }
    }

    public bool DeleteAdvisor(int id)
    {
        Advisor? currentAdvisor = _context.Advisors.Find(id);
        if (currentAdvisor != null)
        {
            _context.Advisors.Remove(currentAdvisor);
            _advisorCache.Delete(id);
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }
}