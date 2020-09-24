using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HandlingRefreshTokens.WebAPIs.DbContexts;
using HandlingRefreshTokens.WebAPIs.DbContexts.Entities;

namespace HandlingRefreshTokens.WebAPIs.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly LocalDbContext context;

        public PeopleController(LocalDbContext _context)
        {
            context = _context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return context.People.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return context.People.Where(f => f.Id == id).FirstOrDefault();
        }



    }
}
