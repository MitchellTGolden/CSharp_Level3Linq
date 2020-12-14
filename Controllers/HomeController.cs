using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsORM.Models;


namespace SportsORM.Controllers
{
    public class HomeController : Controller
    {

        private static Context _context;

        public HomeController(Context DBContext)
        {
            _context = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.BaseballLeagues = _context.Leagues
                .Where(l => l.Sport.Contains("Baseball"))
                .ToList();
            return View();
        }

        [HttpGet("level_1")]
        public IActionResult Level1()
        {
                ViewBag.Woman = _context.Leagues
                .Where(l => l.Name.Contains("Women"))
                .ToList();
                ViewBag.Hockey = _context.Leagues
                .Where(l => l.Name.Contains("Hockey"))
                .ToList();
                ViewBag.NotFoot = _context.Leagues
                .Where(l => !l.Name.Contains("Football"))
                .ToList();
                ViewBag.Conf = _context.Leagues
                .Where(l => l.Name.Contains("Conference"))
                .ToList();
                ViewBag.Atlantic = _context.Leagues
                .Where(l => l.Name.Contains("Atlantic"))
                .ToList();
                ViewBag.Dallas = _context.Teams
                .Where(l => l.Location.Contains("Dallas"))
                .ToList();
                ViewBag.Raptors = _context.Teams
                .Where(l => l.TeamName.Contains("Raptors"))
                .ToList();
                ViewBag.City = _context.Teams
                .Where(l => l.Location.Contains("City"))
                .ToList();
                ViewBag.T = _context.Teams
                .Where(l => l.TeamName.StartsWith('T'))
                .ToList();
                ViewBag.OrderedAlpha = _context.Teams
                .OrderBy(l => l.Location)
                .ToList();
                ViewBag.RevAlpha = _context.Teams
                .OrderByDescending(l => l.TeamName)
                .ToList();
                                ViewBag.Cooper = _context.Players
                .Where(l => l.LastName.Contains("Cooper"))
                .ToList();
                ViewBag.Joshua = _context.Players
                .Where(l => l.FirstName.Contains("Joshua"))
                .ToList();
                ViewBag.Jooper = _context.Players
                .Where(l => !l.FirstName.Contains("Joshua"))
                .Where(l => l.LastName.Contains("Cooper"))
                .ToList(); 
                ViewBag.AlOrWy = _context.Players
                .Where(l => l.FirstName.Contains("Alexander") || l.FirstName.Contains("Wyatt"))
                .ToList();                                  
            return View();
        }

        [HttpGet("level_2")]
        public IActionResult Level2()
        {
            ViewBag.AllSoccer = _context.Leagues 
                .Where(l => l.Sport.Contains("Soccer"))
                .Where(l => l.Name.Contains("Atlantic"))
                .ToList();
            ViewBag.AllTeams = _context.Teams.ToList();
            ViewBag.AllPlayers = _context.Players.ToList();
            ViewBag.BP = _context.Teams
                .Where(t => t.TeamName.Contains("Penguins"))
                .Where(t => t.Location.Contains("Boston"))
                .ToList();
            ViewBag.IFBC = _context.Leagues
                .Where(l => l.Name.Contains("International Collegiate Baseball Conference"))
                .ToList();
            ViewBag.AllPInFTeams = _context.Players
                .Where(p => p.CurrentTeam.CurrLeague.Sport.Contains("Football"))
                .ToList();
            ViewBag.AllFootballTeams = _context.Teams
                .Where(l => l.CurrLeague.Sport.Contains("Football"))
                .ToList();
            ViewBag.TeamWithPlayerSophia = _context.Teams
                .Where(t => t.CurrentPlayers.Any(p => p.FirstName.Contains("Sophia")))
                .ToList();
            ViewBag.LeagueWithPlayerSophia = _context.Leagues
                .Where(l => l.Teams.Any(t=> t.CurrentPlayers.Any(p => p.FirstName.Contains("Sophia"))))
                .ToList();
            ViewBag.PlayersWithLastNameFloresNotWR = _context.Players
                .Where(p => p.LastName.Contains("Flores"))
                .Where(p => p.CurrentTeam.TeamName != "Roughriders" && p.CurrentTeam.Location != "Washington")
                .ToList();
            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {   
            ViewBag.Sam = _context.Players.Include( p => p.AllTeams )
                                            .ThenInclude( pt => pt.TeamOfPlayer)
                                            .Include(p => p.CurrentTeam)
                                            .FirstOrDefault( p => p.FirstName == "Samuel" && p.LastName == "Evans");
            
            ViewBag.AllWV = _context.Teams.Include( p => p.AllPlayers)
                                            .ThenInclude( pt => pt.PlayerOnTeam)
                                            .Include(p => p.CurrentPlayers)
                                            .FirstOrDefault( t => t.TeamName == "Tiger-Cats" && t.Location == "Manitoba");
            ViewBag.AllCV = _context.Teams.Include( p => p.AllPlayers)
                                            .ThenInclude( pt => pt.PlayerOnTeam)
                                            .FirstOrDefault( t => t.TeamName == "Vikings" && t.Location == "Wichita");
            ViewBag.JG = _context.Players.Include( p => p.AllTeams )
                                            .ThenInclude( pt => pt.TeamOfPlayer)
                                            .FirstOrDefault( p => p.FirstName == "Jacob" && p.LastName == "Gray");
            ViewBag.AFAB = _context.Leagues.Include(l => l.Teams)
                                            .ThenInclude(t => t.AllPlayers)
                                            .ThenInclude(pt => pt.PlayerOnTeam.FirstName == "Joshua")
                                            .Where(l => l.Name == "Atlantic Federation of Amateur Baseball Players");
            ViewBag.Joshua = _context.Players.Include(p => p.CurrentTeam )
                                            .ThenInclude( t => t.CurrLeague)
                                            .Include( p => p.AllTeams )
                                            .ThenInclude( pt => pt.TeamOfPlayer)
                                            .ThenInclude( t => t.CurrLeague)
                                            .Where( p => p.CurrentTeam.CurrLeague.Name == "Atlantic Federation of Amateur Baseball Players" || p.AllTeams.Any( t => t.TeamOfPlayer.CurrLeague.Name == "Atlantic Federation of Amateur Baseball Players"))
                                            .Where( p => p.FirstName.StartsWith("J"))
                                            .ToList();
            ViewBag.TwelveOrMore = _context.Teams.Include(t => t.CurrentPlayers)
                                                    .Where(t => t.CurrentPlayers.Count >= 8)
                                                    .Include(pt => pt.AllPlayers)
                                                    .Where(t => t.AllPlayers.Count >= 8)
                                                    .ToList();
            ViewBag.PlayersByTeamNum = _context.Players.Include( p => p.AllTeams )
                                            .ThenInclude( pt => pt.TeamOfPlayer)
                                            .Include(p => p.CurrentTeam)
                                            .OrderByDescending(p => p.AllTeams.Count)
                                            .ToList();
                        return View();
                    
        }

    }
}