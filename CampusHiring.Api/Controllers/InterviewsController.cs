using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InterviewsController : ControllerBase
{
    private readonly CampusHiringDbContext _context;

    public InterviewsController(CampusHiringDbContext context)
    {
        _context = context;
    }

    // GET: api/Interviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Interview>>> GetInterviews()
    {
        return await _context.Interviews.ToListAsync();
    }

    // GET: api/Interviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Interview>> GetInterview(int id)
    {
        var interview = await _context.Interviews.FindAsync(id);

        if (interview == null)
        {
            return NotFound();
        }

        return interview;
    }

    // PUT: api/Interviews/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInterview(int id, Interview interview)
    {
        if (id != interview.Id)
        {
            return BadRequest();
        }

        _context.Entry(interview).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InterviewExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Interviews
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Interview>> PostInterview(Interview interview)
    {
        _context.Interviews.Add(interview);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetInterview", new { id = interview.Id }, interview);
    }

    // DELETE: api/Interviews/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInterview(int id)
    {
        var interview = await _context.Interviews.FindAsync(id);
        if (interview == null)
        {
            return NotFound();
        }

        _context.Interviews.Remove(interview);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool InterviewExists(int id)
    {
        return _context.Interviews.Any(e => e.Id == id);
    }
}
