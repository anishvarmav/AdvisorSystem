public class AdvisorRepository : IAdvisorRepository
{
    private readonly AdvisorDbContext _context;
    private readonly AdvisorCache<int, Advisor> _advisorCache;
    public AdvisorRepository(AdvisorDbContext context, AdvisorCache<int, Advisor> advisorCache)
    {
        _context = context;
        _advisorCache = advisorCache;
    }

    // Retrieves all advisors from the database and updates the cache
    public List<Advisor> GetAdvisors()
    {
        List<Advisor> advisorList = _context.Advisors.ToList();
        // Cache each advisor for faster subsequent access
        foreach (Advisor advisor in advisorList)
        {
            _advisorCache.Put(advisor.Id, advisor);
        }
        return _context.Advisors.ToList();
    }

    // Retrieves an advisor by their Id, checking the cache first
    public Advisor? GetAdvisorById(int id)
    {
        // Attempt to get the advisor from the cache
        Advisor? cachedAdvisor = _advisorCache.Get(id);
        if (cachedAdvisor != null)
        {
            return cachedAdvisor;
        }

        // If not found in cache, retrieve from the database
        Advisor? currentAdvisor = _context.Advisors.Find(id);
        if (currentAdvisor != null)
        {
            _advisorCache.Put(id, currentAdvisor);
        }

        return currentAdvisor;
    }

    // Creates a new advisor
    public Advisor? CreateAdvisor(Advisor advisor)
    {
        Advisor? currentAdvisor = _context.Advisors.Find(advisor.Id);

        //Unique index is not working for in-memory database, so using below checks to determine create
        // Check for existing advisor with the same SIN
        Advisor? sinAdvisor = _context.Advisors.Where(x => x.SIN == advisor.SIN).FirstOrDefault();

        // If any advisor or an advisor with the same SIN exists, return null
        if ((currentAdvisor != null) || (currentAdvisor == null && sinAdvisor != null))
        {
            return null;
        }
        else
        {
            // Generate health status if not provided
            if (string.IsNullOrEmpty(advisor.HealthStatus))
            {
                advisor.HealthStatus = Advisor.GenerateHealthStatus();
            }

            // Add the advisor to the database and cache
            _context.Advisors.Add(advisor);
            _advisorCache.Put(advisor.Id, advisor);
            _context.SaveChanges();
            return advisor;
        }
    }

    // Updates an existing advisor
    public Advisor? UpdateAdvisor(int id, Advisor advisor)
    {
        Advisor? currentAdvisor = _context.Advisors.Find(id);

        //Unique index is not working for in-memory database. So using below checks to determine update
        // Check for existing advisor with the same SIN
        Advisor? sinAdvisor = _context.Advisors.Where(x => x.SIN == advisor.SIN).FirstOrDefault();

        // If the advisor doesn't exist or there's a conflict with the SIN, return null
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

            // Update the cache with the modified advisor
            _advisorCache.Put(id, currentAdvisor);
            _context.SaveChanges();
            return currentAdvisor;
        }
    }

    // Deletes an advisor by Id
    public bool DeleteAdvisor(int id)
    {
        Advisor? currentAdvisor = _context.Advisors.Find(id);
        if (currentAdvisor != null)
        {
            // Remove the advisor from the database and cache
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