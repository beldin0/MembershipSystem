using MembershipSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MembershipSystem.Controllers
{
    [Route("api/[controller]")]
    public class MembersController : Controller
    {

        DbContextOptions<MembersContext> options = new DbContextOptionsBuilder<MembersContext>()
            .UseInMemoryDatabase("dummy_database")
            .Options;

        private MembersContext GetContext()
        {
            MembersContext mc = new MembersContext(options);
            if (mc.Members.Find("ABC123")==null)
            {
                mc.Members.Add(new Member()
                {
                    Id = "ABC123",
                    Name = "John Smith",
                    EmailAddress = "jon@smith.com",
                    MobilePhoneNumber = "01234567890",
                    PinCode = "0000",
                    Balance = 100
                });
                mc.SaveChanges();
            }
            return mc;
        }

        // GET api/members/5
        [HttpGet("{id}")]
        private IActionResult Get(string id)
        {
            Member m = GetContext().Members.Find(id);
            if (m == null)
            {
                return new NotFoundObjectResult("ID not recognised. Please register");
            }
            else
            {
                return Ok(new { name = m.Name });
            }
        }

        // GET api/5
        [HttpGet("{id}")]
        public IActionResult Get(string id, string pin)
        {
            if (pin == null) return Get(id);
            Member m = GetContext().Members.Find(id);
            if (m == null)
            {
                return new NotFoundObjectResult("ID not recognised");
            }
            else
            {

                if (m.PinCode == pin)
                {
                    return Ok(new { name = m.Name, balance = m.Balance });
                }
                else
                {
                    return new UnauthorizedResult();
                }
            }
        }

        // PUT api/members/5
        [Route("{id}")]
        [HttpPut]
        public IActionResult Put(string id, string name, string email, string mobile, string pin)
        {
            if (name == null || email == null || mobile == null || pin == null)
            {
                return new BadRequestObjectResult(new { Function = "PUT", Id = id, Name = name, Email = email, Mobile = mobile, Pin = pin });
            }
            using (MembersContext db = GetContext())
            {
                Member m = db.Members.Find(id);
                if (m == null) {
                    db.Members.Add(new Member() {
                        Id = id,
                        Name = name,
                        EmailAddress = email,
                        MobilePhoneNumber = mobile,
                        PinCode = pin,
                        Balance = 0
                    });
                    db.SaveChanges();
                    return new StatusCodeResult(201);
                } else {
                    return new StatusCodeResult(409);
                }
                
            }
        }

        //POST api/members/5
        [Route("{id}")]
        [HttpPost]
        public IActionResult Post(string id, string pin, int? value)
        {
            if (pin == null || value == null)
            {
                return new BadRequestObjectResult(new { Function = "POST", Id = id, Pin = pin, Value=value });
            }

            using (MembersContext db = GetContext()) {
                Member m = db.Members.Find(id);
                if (m == null)
                {
                    return new NotFoundObjectResult("ID not recognised");
                }
                else
                {
                    if (m.PinCode == pin)
                    {
                        if (m.Balance + value < 0)
                        {
                            return new StatusCodeResult(412);
                        }
                        else
                        {
                            m.Balance += (int)value;
                            db.SaveChanges();
                            return Ok(new { balance = m.Balance });
                        }
                    }
                    else
                    {
                        return new UnauthorizedResult();
                    }
                }
            }
        }
    }
}
