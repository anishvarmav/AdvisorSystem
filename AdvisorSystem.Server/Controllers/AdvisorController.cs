using Microsoft.AspNetCore.Mvc;

namespace AdvisorSystem.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvisorController : ControllerBase
    {
        private readonly IAdvisorRepository _advisorRepository;

        public AdvisorController(IAdvisorRepository advisorRepository)
        {
            _advisorRepository = advisorRepository;
        }

        // GET: /advisor - Retrieves a list of all advisors
        [HttpGet]
        public ActionResult<List<Advisor>> GetAdvisors()
        {
            return _advisorRepository.GetAdvisors();
        }

        // GET: /advisor/{id} - Retrieves a specific advisor by Id
        [HttpGet("{id}")]
        public ActionResult<Advisor> GetAdvisor(int id)
        {
            Advisor? currentAdvisor = _advisorRepository.GetAdvisorById(id);

            // Return 404 if advisor is not found
            if (currentAdvisor == null)
            {
                return NotFound();
            }

            return currentAdvisor;
        }

        // PUT: /advisor - Creates a new advisor
        [HttpPut]
        public ActionResult<Advisor?> CreateAdvisor(Advisor advisor)
        {
            Advisor? currentAdvisor = _advisorRepository.CreateAdvisor(advisor);

            // Return 400 if the creation fails
            if (currentAdvisor == null)
            {
                return BadRequest();
            }

            return currentAdvisor;
        }

        // PUT: /advisor/{id} - Updates an existing advisor
        [HttpPut("{id}")]
        public ActionResult<Advisor?> UpdateAdvisor(int id, Advisor advisor)
        {
            Advisor? currentAdvisor = _advisorRepository.UpdateAdvisor(id, advisor);

            // Return 400 if the update fails
            if (currentAdvisor == null)
            {
                return BadRequest();
            }

            return currentAdvisor;
        }

        // DELETE: /advisor/{id} - Deletes a specific advisor
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteAdvisor(int id)
        {
            bool result = _advisorRepository.DeleteAdvisor(id);

            // Return 404 if the advisor to delete is not found
            if (!result)
            {
                return NotFound();
            }

            return result;
        }
    }
}
