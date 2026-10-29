using Microsoft.AspNetCore.Mvc;

namespace GestionOrdinateurs.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ParcInformatiqueContext _db;

    public UsersController(ParcInformatiqueContext db)
    {
        _db = db;
    }

    // retourne tous les users
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _db.Users.ToList();
        return Ok(users);
    }

    // cherche un user par son login
    [HttpGet("{login}")]
    public IActionResult GetByLogin(string login)
    {
        var user = _db.Users.FirstOrDefault(u => u.Login == login);
        if (user == null)
            return NotFound("utilisateur introuvable");

        return Ok(user);
    }
}
