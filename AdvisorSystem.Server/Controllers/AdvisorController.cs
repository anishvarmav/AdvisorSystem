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

        [HttpGet]
        public ActionResult<List<Advisor>> GetAdvisors()
        {
            return _advisorRepository.GetAdvisors();
        }

        [HttpGet("{id}")]
        public ActionResult<Advisor> GetAdvisor(int id)
        {
            Advisor? currentAdvisor = _advisorRepository.GetAdvisorById(id);

            if (currentAdvisor == null)
            {
                return NotFound();
            }

            return currentAdvisor;
        }

        [HttpPut]
        public ActionResult<Advisor?> CreateAdvisor(Advisor advisor)
        {
            Advisor? currentAdvisor = _advisorRepository.CreateAdvisor(advisor);

            if (currentAdvisor == null)
            {
                return BadRequest();
            }

            return currentAdvisor;
        }

        [HttpPut("{id}")]
        public ActionResult<Advisor?> UpdateAdvisor(int id, Advisor advisor)
        {
            Advisor? currentAdvisor = _advisorRepository.UpdateAdvisor(id, advisor);

            if (currentAdvisor == null)
            {
                return BadRequest();
            }

            return currentAdvisor;
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteAdvisor(int id)
        {
            bool result = _advisorRepository.DeleteAdvisor(id);

            if (!result)
            {
                return NotFound();
            }

            return result;
        }
    }
}
